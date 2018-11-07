using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assitor
{
    public static class AppCache
    {
        public static string Host { get; set; }

        static AppCache()
        {
            Host = System.Configuration.ConfigurationManager.AppSettings["host"];
        }
    }
}
