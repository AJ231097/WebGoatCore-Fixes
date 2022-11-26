using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGoatCore.Utils
{
    public static class Debugger
    {
        public static bool IsDebug
        {
            get
            {
                bool isDebug = false;
            #if DEBUG
                isDebug = true;
            #endif
                return isDebug;
            }
        }
    }
}
