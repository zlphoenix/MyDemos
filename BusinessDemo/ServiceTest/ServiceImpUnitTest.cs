using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelChina.TRF.Demo.DemoSV;
using TelChina.TRF.System.Service;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;
using TelChina.TRF.Service.AppHosting;
using TelChina.TRF.Util.Common;
//using System.ServiceModel.Channels;

namespace TelChina.TRF.Test.ServiceTest
{
    [TestClass]
    public class ServiceImpUnitTest
    {
        [TestInitialize]
        public void MyTestInitialize()
        {
            if ("Integrate" == ConfigHelper.GetConfigValue("DeployType"))
                AppHost.Start();
        }

        /// <summary>
        /// 正常情况是不允许这么使用的,
        /// 此测试仅用于保证服务框架的结构稳定性
        /// </summary>
        [TestMethod]
        public void ServiceImpInvokeTest()
        {
            var svImp = new DemoSV();
            var result = svImp.Do(GetParam());
            Assert.IsNotNull(result);
        }

        private static ParamDTO GetParam()
        {
            return new ParamDTO
                       {
                           IsSucceed = true,
                           ParamName = "ParamName=P1",
                           Value = "ParamValue =V1"
                       };
        }

        private static ServiceContext GetProfileContext()
        {
            return new ServiceContext
                       {
                           LoginDate = DateTime.Now,
                           UserCode = "admin",
                           UserName = "admin",
                           LoginIP = "127.0.0.1",
                           UserID = "123",
                           Token = Guid.NewGuid().ToString(),
                           Content = new Dictionary<string, string>() { { "ContentKey1", "ContentValue1" }, { "ContentKey2", "ContentValue2" } }

                       };
        }

        [TestMethod]
        public void ServiceAgentInvokeTest()
        {
            var context = GetProfileContext();
            CallContext.SetData(ServiceContextManager<ServiceContext>.SESSIONCOTEXTKEY, context);

            var agent = new DemoSVAgent();
            agent.Param = GetParam();
            agent.Do();
        }
    }
}
