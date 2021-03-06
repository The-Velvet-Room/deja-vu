﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace deja_vu.Utilities
{
    public static class FileUtility
    {
        public static void OnceDoneWriting(string path, Action<FileStream> action)
        {
            while (true)
            {
                Trace.TraceInformation("Polling for a write handle...");
                try
                {
                    using (var file = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        if (file.Length == 0)
                        {
                            throw new FileLoadException();
                        }
                        action(file);
                    }
                    break;
                }
                catch (FileLoadException fex)
                {
                    Trace.TraceInformation("Invalid filesize. Waiting for filestream to close.");
                }
                catch (IOException ex)
                {
                    if (!IsLockedException(ex))
                    {
                        Trace.TraceError("Exception was not a locked exception.", new { Error = ex });
                        throw;
                    }
                }
                Thread.Sleep(50);
            }
        }

        private const int ERROR_SHARING_VIOLATION = 32;
        private const int ERROR_LOCK_VIOLATION = 33;

        public static bool IsLockedException(IOException ex)
        {
            int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }
    }
}
