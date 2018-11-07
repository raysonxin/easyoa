using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Assitor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            JsonSerializerSettings setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                setting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                setting.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
                setting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                return setting;
            });
        }
    }
}
