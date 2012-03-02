using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using log4net.Config;

namespace TelChina.TRF.Util.Logging
{
    public static class LogManager
    {
        static LogManager()
        {
            var ass = System.Reflection.Assembly.GetExecutingAssembly();
            //file:///
            var path = ass.Location + ".config";
            if (!File.Exists(path))
            {
                var res = ass.FullName.Split(',')[0];
                var ResName = res + "." + res + ".config";
                var stream = ass.GetManifestResourceStream(ResName);
                //ass.GetManifestResourceStream("TelChina.TRF.Util.LogManager.TelChina.TRF.Util.LogManager.config");
                if (stream == null) throw new FileNotFoundException("config");
                XmlConfigurator.Configure(stream);
                //DOMConfigurator.Configure(stream);
            }
            else
            {
                XmlConfigurator.Configure(new FileInfo(path));
                //DOMConfigurator.Configure(new FileInfo(path));
            }

            //log4net.Config.DOMConfigurator.Configure();
        }
        public static ILogger GetLogger(string name)
        {
            return new Logger(log4net.LogManager.GetLogger(name));
        }
    }
}
