using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Transactions;
using TelChina.TRF.Util.Common;
using TelChina.TRF.System.Exceptions;
using TelChina.TRF.System.Service;
using TelChina.TRF.Service.AOP.AOPAttribute;
//using TelChina.TRF.System.AOP;

namespace TelChina.TRF.Demo.DemoSV
{
    /// <summary>
    /// 服务入口类
    /// </summary>
    [ServiceBehavior(
        //异常传播
        IncludeExceptionDetailInFaults = DebugHelper.IncludeExceptionDetailInFaults,
        //事务隔离级别
        TransactionIsolationLevel = IsolationLevel.Serializable)]
    [LogAttribute]
    public partial class DemoSV : MarshalByRefObject, IDemoSV
    {
        #region ISV Members
        /// <summary>
        /// 服务入口
        /// </summary>
        /// <param name="param">服务模型中定义的参数</param>
        /// <returns>服务模型中定义的返回值</returns>
        [OperationBehavior(TransactionScopeRequired = true)]
        public ResultDTO Do(ParamDTO param)
        {
            ResultDTO result = null;
            try
            {
                if (ServiceContextManager<ServiceContext>.Current != null)
                {
                    //传递上下文
                    Console.WriteLine(string.Format("<=Message Header 传输内容:{0}  =>",
                        ServiceContextManager<ServiceContext>.Current));
                    //this.currentContext = ServiceContextManager<ServiceContext>.Current.Value;
                    //base.InitThreadContext(currentContext);
                }
                //var header = OperationContext.Current.IncomingMessageHeaders.GetHeader<int>("Int32", "System");
                //Console.WriteLine(string.Format("<=Message Header 传输:{0}  =>", header));

                #region 创建和初始化操作对象
                var opObj = new DemoSVImplement { Param = param };
                #endregion
                //DealTransaction();
                //result = base.Excute(opObj) as ResultDTO;
                opObj.Do_Ex();
            }
            catch (Exception ex)
            {
                if (ex is ServiceExceptionBase)
                {
                    throw new FaultException<ServiceExceptionBase>(new ServiceExceptionBase(ex), new FaultReason("业务异常"));
                }
                throw new FaultException<UnhandledException>(new UnhandledException(ex), new FaultReason("未知异常"));
            }
            finally
            {
                //做一些清理工作
            }
            return result;
        }



        private void DealTransaction()
        {
            if (Transaction.Current != null)
            {
                Console.Write("[服务端进入事务处理模式]:");
                LogTransactionInfo(Transaction.Current);
                Transaction.Current.TransactionCompleted +=
                    new TransactionCompletedEventHandler((obj, args) =>
                    {
                        Console.Write("[服务端进入事务处理完成]:");
                        LogTransactionInfo(args.Transaction);
                    });
            }
            else
            {
                Console.Write("[服务处于无事务模式]:");
            }
        }

        private static void LogTransactionInfo(Transaction trans)
        {
            Console.WriteLine(string.Format("事务属性:\n事务状态:{0}\n创建时间{1}\n分布式标识{2}\n本地标识{3}",
                trans.TransactionInformation.Status,
                trans.TransactionInformation.CreationTime,
                trans.TransactionInformation.DistributedIdentifier,
                trans.TransactionInformation.LocalIdentifier));
        }

        #endregion

        //public override object Do()
        //{
        //    DemoSVImplement implObj = new DemoSVImplement();
        //    //implObj.Param = param;
        //    return null;
        //}
    }

    /// <summary>
    /// 服务业务实现
    /// </summary>
    internal partial class DemoSVImplement : StrategyBase
    {
        /// <summary>
        /// 服务参数
        /// </summary>
        public ParamDTO Param { get; set; }

        [LogAttribute]
        public override object Do()
        {
            return this.Do_Ex();
        }
    }

}
