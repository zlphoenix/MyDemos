using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.TRF.Util.Common
{
    public class ConfigHelper
    {
        /// <summary>
        /// 取AppSettings节中指定键的值
        /// </summary>
        /// <param name="ConfigKey"></param>
        /// <returns></returns>
        public static string GetConfigValue(string ConfigKey)
        {
            string result = "";
            if (!string.IsNullOrEmpty(ConfigKey))
            {
                result = (System.Configuration.ConfigurationManager.AppSettings[ConfigKey]);
            }
            return result;
        }
    }
}
