using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deja_vu.Utilities
{
    public static class PathUtility
    {
        public static string AddTrailingSlash(string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar.ToString()) ?
                path :
                path + Path.DirectorySeparatorChar;
        }
    }
}
