using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using TelChina.TRF.System.Exceptions;
using TelChina.TRF.System.Service;
using TelChina.TRF.Service.AOP;


namespace TelChina.TRF.Demo.DemoSV
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ServiceContract(Namespace = "http://www.telchina.com.cn/TRF/V4/Service/2011/03")]
    [PolicyInjectionBehavior]//提供PIAB接口
    public interface IDemoSV
    {
        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="param"></param>
        [OperationContract]
        [FaultContract(typeof(ServiceExceptionBase))]
        [FaultContract(typeof(DemoSVExecption))]
        [FaultContract(typeof(UnhandledException))]
        //允许接收来自客户端事务流
        [TransactionFlow(TransactionFlowOption.Allowed)]
        //如果客户端传入了事务,则沿用,否则新建事务   
        ResultDTO Do(ParamDTO param);
    }
}
