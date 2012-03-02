using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TelChina.TRF.System.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class DTOBase : ISerializable
    {
        protected DTOBase(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
