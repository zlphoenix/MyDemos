using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TelChina.TRF.System.Exceptions
{
    [Serializable]
    public class UnhandledException : ServiceExceptionBase
    {
        public UnhandledException() : base() { }
        public UnhandledException(Exception ex)
            : base(ex)
        {
            this.MessageFormat = "未知非系统异常";
        }
        public UnhandledException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
