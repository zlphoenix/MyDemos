using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.TRF.Domain.Core;

namespace TelChina.TRF.Demo.DomainModel.Model
{
    public partial class SysParam : IEntity
    {
        public void SetDefaultValue()
        {

        }

        public void OnValidate()
        {
            if (string.IsNullOrEmpty(this.sysparamName))
            {

            }
        }
        public long Id
        {
            get { return this.sysparamid; }
            set { this.sysparamid = value; }
        }
    }
}
