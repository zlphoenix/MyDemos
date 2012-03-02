using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TelChina.TRF.Demo.DemoSV
{

    internal partial class DemoSVImplement
    {
        public ResultDTO Do_Ex()
        {
            Console.WriteLine(string.Format("服务DemoSV正在执行...参数:{0}", Param.ToString()));

            if (Param.IsSucceed)
            {
                ResultDTO result = new ResultDTO();
                result.ReturnValue = Param.ParamName + "," + Param.Value;
                return result;
            }
            else
            {
                throw new DemoSVExecption("服务调用异常!,测试用");
            }
        }
    }
}
