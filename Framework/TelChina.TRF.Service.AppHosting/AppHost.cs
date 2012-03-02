using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.IO;
using System.Reflection;
using System.ServiceModel.Description;

namespace TelChina.TRF.Service.AppHosting
{
    public class AppHost
    {
        /// <summary>
        /// 启动服务引擎
        /// </summary>
        public static void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += EndProcess;
            try
            {
                IEnumerable<Type> bpList = GetSVs();
                StartHost(bpList);
            }
            catch (Exception ex)
            {
                DealException(ex);
            }
            //Console.ReadLine();
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        private static void DealException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                DealException(ex.InnerException);
            }
            Console.WriteLine(string.Format("异常类型{0},\n消息{1},\n堆栈{2}",
                ex.GetType(), ex.Message, ex.StackTrace));
        }

        /// <summary>
        /// 启动ServiceHost
        /// </summary>
        /// <param name="bpList"></param>
        private static void StartHost(IEnumerable<Type> bpList)
        {
            var baseAddr = (System.Configuration.ConfigurationManager.AppSettings["ServiceBaseUri"]);


            foreach (Type bpType in bpList)
            {
                //
                var serviceContract = (from i in bpType.GetInterfaces()
                                       where i.GetCustomAttributes(typeof(ServiceContractAttribute), false).Count() > 0
                                       select i).FirstOrDefault();

                var host = new ServiceHost(bpType, new Uri(baseAddr)) { CloseTimeout = TimeSpan.FromMinutes(20d) };
                EnableMexGet(host);
                var wsBinding = new WSHttpBinding { TransactionFlow = true };
                //ContractDescription cd = new ContractDescription(bpType.FullName);

                //ServiceEndpoint endpoint = new ServiceEndpoint();
                //endpoint.Binding = wsBinding;
                //endpoint.Address = new EndpointAddress(serviceContract.FullName);
                //endpoint.Behaviors.Add(new ServiceDebugBehavior());
                if (serviceContract != null)
                    host.AddServiceEndpoint(serviceContract, wsBinding, serviceContract.FullName);
                //host.AddServiceEndpoint(endpoint);        

                host.Open();
                if (serviceContract != null) Console.WriteLine(string.Format("服务{0}已启动", serviceContract.Name));
            }
        }

        /// <summary>
        /// 启动元数据获取行为
        /// </summary>
        /// <param name="host"></param>
        private static void EnableMexGet(ServiceHost host)
        {
            var mdBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (mdBehavior == null)
            {
                mdBehavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                host.Description.Behaviors.Add(mdBehavior);
            }
        }

        /// <summary>
        /// 查找服务
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetSVs()
        {
            var result = new List<Type>();
            var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Labs");
            //找不到Lib目录就在当前路径下找
            if (!Directory.Exists(baseDirectory))
            {
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            var libFiles = Directory.GetFiles(baseDirectory, "*.Impl.dll");
            foreach (var libFile in libFiles)
            {
                var lib = Assembly.LoadFile(libFile);
                AppDomain.CurrentDomain.Load(lib.FullName);
                var svType = (from t in lib.GetTypes()
                              where
                                  (from i in t.GetInterfaces()
                                   where i.GetCustomAttributes(typeof(ServiceContractAttribute), false).Count() > 0
                                   select i).Count() > 0
                              select t).ToList();
                if (svType.Count > 0)
                {
                    result.AddRange(svType);
                }
            }
            return result;
        }

        /// <summary>
        /// 进程终止时释放非托管资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EndProcess(object sender, EventArgs e)
        {
            Console.WriteLine("Is Ending.Press any key to continue...");
            Console.Read();
        }
    }

}
