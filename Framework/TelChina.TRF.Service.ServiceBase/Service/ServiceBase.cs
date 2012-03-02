using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.TRF.System.AOP;

namespace TelChina.TRF.System.Service
{
    /// <summary>
    /// 服务实现的基类,实现AOP引擎
    /// </summary>
    public abstract class ServiceBase : MarshalByRefObject
    {
        /// <summary>
        /// 当前用户登录信息上下文
        /// </summary>
        protected ServiceContext currentContext;

        /// <summary>
        /// 执行服务逻辑的包装方法,包含了AOP执行逻辑
        /// </summary>
        /// <returns></returns>
        protected object Excute(StrategyBase opObj)
        {
            var doMethod = opObj.GetType().GetMethod("Do");
            var aopFrames = doMethod.GetCustomAttributes(typeof(AOP.AOPAttribute), true)
                .Where(f => (f as AOPAttribute) != null)
                .Select<object, AOPAttribute>(f => f as AOPAttribute);

            Stack<AOP.AOPAttribute> frameStack = null;
            if (aopFrames != null && aopFrames.Count() > 0)
            {
                frameStack = new Stack<AOP.AOPAttribute>(aopFrames.Count());
                foreach (AOP.AOPAttribute frame in aopFrames)
                {
                    frame.BeforeInvoke(this);
                    frameStack.Push(frame);
                }
            }

            var result = opObj.Do();// return value...

            if (frameStack != null)
            {
                while (frameStack.Count > 0)
                {
                    var frame = frameStack.Pop();
                    frame.AfterInvoke(this);
                }
            }

            return result;
        }

        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <param name="currentContext"></param>
        protected void InitThreadContext(ServiceContext currentContext)
        {
            this.currentContext = currentContext;
        }

        ///// <summary>
        ///// 执行应用服务的业务逻辑
        ///// </summary>
        ///// <returns>返回模型中定义的类型对象</returns>
        //public abstract object Do();
    }
}
