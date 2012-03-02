using System;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace TelChina.TRF.System.Service
{
    public class ProxyBase
    {
        const bool isRemoteCall = true;
        private static T GetChannel<T>(Binding binding, EndpointAddress endpointAddress)
        {
            //超时时间
            //ToDo 需要使用配置或者从服务定位器获取...
            binding.ReceiveTimeout = TimeSpan.FromMinutes(20d);
            var channelFactory =
                new ChannelFactory<T>(binding, endpointAddress);

            T bpChannel = channelFactory.CreateChannel();
            return bpChannel;
        }
        protected static T GetChannel<T>()
        {
            if (isRemoteCall)
            {
                var binding = new WSHttpBinding { TransactionFlow = true };
                var result = GetChannel<T>(
                    binding,
                    new EndpointAddress("http://localhost:1234/" + typeof(T).FullName));
                return result;
            }
            //ToDo 添加本地调用方式
            //else
            //{
            //    //本地方式调用
            //    return null;
            //}

        }
    }
}
