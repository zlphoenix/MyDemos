using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.TRF.Util.Logging;

namespace TelChina.TRF.Util.UtilTest
{
    [TestClass]
    public class LogMangerTest
    {
        [TestMethod]
        public void LoggerTest()
        {
            var logger = LogManager.GetLogger("Test");
            logger.Debug("Test! Logger");
        }
    }
}
