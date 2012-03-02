using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;

namespace TelChina.TRF.System.Service
{
    /// <summary>
    /// 服务上下文的管理器
    /// </summary>
    [DataContract]
    public class ServiceContextManager<T> where T : class
    {
        /// <summary>
        /// 需要进行消息传递的上下文实例
        /// </summary>
        [DataMember]
        public readonly T Value;
        #region  需要唯一确定一种类型,不一定非要用这种方式
        /// <summary>
        /// 用于Message契约传输的类型名称
        /// </summary>
        internal static string TypeName
        {
            get { return typeof(T).Name; }
        }
        /// <summary>
        /// 用于Message契约传输的类型命名空间
        /// </summary>
        internal static string TypeNamespace
        {
            get { return typeof(T).Namespace; }
        }

        public static string SESSIONCOTEXTKEY
        {
            get { return "TRF.Service.ServiceSessionKey" + typeof(T).FullName; }
        }

        #endregion
        static ServiceContextManager()
        {
            //检查是否可被序列化
            var t = typeof(T);
            Debug.Assert(IsDataContract(t) || t.IsSerializable);
        }

        /// <summary>
        /// 是否可以被序列化
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsDataContract(Type type)
        {
            return type.GetCustomAttributes(typeof(SerializableAttribute), false).Count() > 0;
        }
        public ServiceContextManager(T value)
        {
            this.Value = value;
        }
        public ServiceContextManager()
        {
            var context = CallContext.GetData(SESSIONCOTEXTKEY) as T;
            this.Value = context ?? default(T);
        }

        public static ServiceContextManager<T> Current
        {
            get
            {
                if (OperationContext.Current == null
                    || OperationContext.Current.IncomingMessageHeaders == null)
                {
                    return null;
                }
                return OperationContext.Current.IncomingMessageHeaders.
                    GetHeader<ServiceContextManager<T>>(TypeName, TypeNamespace);
            }
            set
            {
                Debug.Assert(OperationContext.Current != null);
                if (Current != null)
                {
                    throw new InvalidOperationException(string.Format("上下文已经存在:{0}", Current));
                }
                var header = new MessageHeader<ServiceContextManager<T>>(value);
                OperationContext.Current.OutgoingMessageHeaders.
                    Add(header.GetUntypedHeader(TypeName, TypeNamespace));
            }
        }


        public override string ToString()
        {
            if (this.Value == null)
                return string.Format("Context类型:{0},{1}; 内容为空!", 
                    TypeNamespace, TypeName);
            else
                return string.Format("Context类型:{0},{1}; 内容:{2}", 
                    TypeNamespace, TypeName, Value.ToString());
        }

    }
}
