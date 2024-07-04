using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ToolExportVideo.Common
{
    public class ConfigUtil
    {
        public static IConfiguration Appsettings { get; set; }
        public static void SetConfigGlobal(IConfiguration config)
        {
            Appsettings = config;
        }
        public static T GetAppSettings<T>(string key)
        {
            string value = Appsettings[key];
            var oValue = Converter.ConvertValueByType<T>(value);
            return oValue;
        }
    }
}
