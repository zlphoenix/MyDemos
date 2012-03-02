using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.IO;
using TelChina.TRF.Demo.DemoSV;

namespace TelChina.TRF.Service.ServiceTest
{
    [TestClass]
    public class ExceptionSerializeUnitTest
    {
        [TestMethod]
        public void TestExceptionSerialize()
        {
            DemoSVExecption obj = new DemoSVExecption();
            obj.Containt = "BP异常";
            DataContractSerializer formater = new DataContractSerializer(typeof(DemoSVExecption));

            using (Stream stream = new MemoryStream())
            {
                formater.WriteObject(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                DemoSVExecption exception = (DemoSVExecption)formater.ReadObject(stream);
            }
        }
    }
}
