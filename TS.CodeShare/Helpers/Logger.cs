using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TS.CodeShare.Helpers
{
    public class Logger
    {
        public static log4net.ILog log = log4net.LogManager.GetLogger("Log");
    }
}