using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deja_vu.Properties;

namespace deja_vu.Utilities
{
    public static class MkvmergeUtility
    {
        private const string stretchMp4Args = "-o {0} \"--sync\" \"1:0,{1}\" \"--forced-track\" \"1:no\" \"--fix-bitstream-timing-information\" \"1:1\" \"-d\" \"1\" \"-A\" \"-S\" \"-T\" \"--no-global-tags\" \"--no-chapters\" \"(\" \"{2}\" \")\" \"--track-order\" \"0:1\"";

        public static string MkvmergeLocation
        {
            get
            {
                return Settings.Default.MkvmergePath;
            }
            set
            {
                Settings.Default.MkvmergePath = value;
                Settings.Default.Save();
            }
        }

        public static void StretchMp4(string input, string output, decimal rate)
        {
            StretchMp4(input, output, rate, null);
        }

        public static void StretchMp4(string input, string output, decimal rate, EventHandler exited)
        {
            // make sure the output string refers to an mkv file
            output = Path.ChangeExtension(output, ".mkv");

            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = MkvmergeLocation;
            process.StartInfo.Arguments = String.Format(stretchMp4Args, output, rate, input);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (exited != null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += exited;
            }
            process.Start();
            process.PriorityClass = ProcessPriorityClass.BelowNormal;
        }
    }
}
