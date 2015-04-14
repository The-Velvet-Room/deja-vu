using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace deja_vu.Utilities
{
    public static class FileUtility
    {

        public static void OnceDoneWriting(string path, Action<FileStream> action)
        {
            while (true)
            {
                try
                {
                    using (var file = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        action(file);
                    }
                    break;
                }
                catch (IOException ex)
                {
                    if (!IsLockedException(ex))
                    {
                        throw;
                    }
                }
                Thread.Sleep(1000);
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
