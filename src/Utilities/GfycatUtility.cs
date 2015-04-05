using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;

// API info from http://gfycat.com/api

namespace deja_vu.Utilities
{
    public static class GfycatUtility
    {
        public static async Task<GfycatResult> Upload(string path)
        {
            var key = GenerateKey();

            using (var client = new HttpClient())
            {
                using (var fs = File.OpenRead(path))
                {
                    var req = BuildFormData(key, new StreamContent(fs));

                    var resp = await client.PostAsync("https://gifaffe.s3.amazonaws.com/", req);

                    var respContent = await resp.Content.ReadAsStringAsync();
                }

                // start the transcode
                var transcodeResp = await client.GetStringAsync("http://upload.gfycat.com/transcodeRelease/" + key);

                for (var i = 0; i < 600; i++)
                {
                    var json = await client.GetStringAsync("http://upload.gfycat.com/status/" + key);
                    var serializer = new JavaScriptSerializer();
                    var obj = serializer.Deserialize<Dictionary<string, string>>(json);
                    string result;
                    if (obj.TryGetValue("task", out result))
                    {
                        if (result == GfycatStatus.Complete.Value)
                        {
                            var gfyName = obj["gfyname"];
                            return new GfycatResult
                            {
                                Status = GfycatStatus.Complete,
                                Url = "https://gfycat.com/" + gfyName
                            };
                        }
                        if (result == GfycatStatus.Error.Value)
                        {
                            return new GfycatResult
                            {
                                Status = GfycatStatus.Error,
                                ErrorMessage = obj["error"]
                            };
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            return new GfycatResult
            {
                Status = GfycatStatus.Error,
                ErrorMessage = "An unknown error occurred. Key=" + key
            };
        }

        public static async Task<GfycatResult> UploadAndPost(string filePath, string endpoint)
        {
            var result = await Upload(filePath);
            if (result.Status == GfycatStatus.Complete)
            {
                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("url", result.Url)
                    });
                    var resp = await client.PostAsync(endpoint, content);
                }
            }
            return result;
        }

        private static MultipartFormDataContent BuildFormData(string key, StreamContent file)
        {
            var req = new MultipartFormDataContent();
            req.Add(new StringContent(key), "key");
            req.Add(new StringContent("private"), "acl");
            req.Add(new StringContent("AKIAIT4VU4B7G2LQYKZQ"), "AWSAccessKeyId");
            req.Add(new StringContent("eyAiZXhwaXJhdGlvbiI6ICIyMDIwLTEyLTAxVDEyOjAwOjAwLjAwMFoiLAogICAgICAgICAgICAiY29uZGl0aW9ucyI6IFsKICAgICAgICAgICAgeyJidWNrZXQiOiAiZ2lmYWZmZSJ9LAogICAgICAgICAgICBbInN0YXJ0cy13aXRoIiwgIiRrZXkiLCAiIl0sCiAgICAgICAgICAgIHsiYWNsIjogInByaXZhdGUifSwKCSAgICB7InN1Y2Nlc3NfYWN0aW9uX3N0YXR1cyI6ICIyMDAifSwKICAgICAgICAgICAgWyJzdGFydHMtd2l0aCIsICIkQ29udGVudC1UeXBlIiwgIiJdLAogICAgICAgICAgICBbImNvbnRlbnQtbGVuZ3RoLXJhbmdlIiwgMCwgNTI0Mjg4MDAwXQogICAgICAgICAgICBdCiAgICAgICAgICB9"), "policy");
            req.Add(new StringContent("200"), "success_action_status");
            req.Add(new StringContent("mk9t/U/wRN4/uU01mXfeTe2Kcoc="), "signature");
            req.Add(new StringContent("video/mp4"), "Content-Type");
            req.Add(file, "file");
            return req;
        }

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private static string GenerateKey()
        {
            var random = new Random();
            // key must be 5-10 characters
            var length = random.Next(5, 11);
            return new string(
                Enumerable.Repeat(chars, length)
                .Select(c => chars[random.Next(chars.Length)])
                .ToArray()
            );
        }
    }

    public class GfycatStatus
    {
        private GfycatStatus(string value) { Value = value; }

        public string Value { get; private set; }

        public static GfycatStatus Complete { get { return new GfycatStatus("complete"); } }
        public static GfycatStatus Error { get { return new GfycatStatus("error"); } }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Value == ((GfycatStatus)obj).Value;
        }

        // == and != is usually an identity comparison in C#, but since we're trying to
        // make something like an enum, we'll override it to check value. ReferenceEquals
        // can still be used to check identity.
        public static bool operator ==(GfycatStatus a, GfycatStatus b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(GfycatStatus a, GfycatStatus b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }

    public class GfycatResult
    {
        public GfycatStatus Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Url { get; set; }
    }
}
