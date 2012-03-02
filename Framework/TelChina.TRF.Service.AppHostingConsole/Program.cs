using System;
using TelChina.TRF.Service.AppHosting;
using System.Security.Principal;

namespace TelChina.TRF.Service.AppHostingConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                Console.WriteLine("应用服务引擎 需要以管理员权限运行...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("按任意键启动服务...");
                Console.ReadLine();
                AppHost.Start();
                Console.ReadLine();
            }
        }
        /// <summary>
        /// 判断是否以管理员权限运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            if (identity != null)
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            else
            {

                return false;
            }
        }
    }
}