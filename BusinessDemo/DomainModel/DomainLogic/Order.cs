using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.TRF.Domain.Core;

namespace TelChina.TRF.Demo.DomainModel.Model
{
    partial class Order : IEntity
    {
        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IEntity"/>
        /// </summary>
        public void SetDefaultValue()
        {
            if (string.IsNullOrEmpty(this.CreatedBy))
            {
                this.CreatedBy = "Admin";
            }
            if (string.IsNullOrEmpty(this.ModifiedBy))
            {
                this.ModifiedBy = "Admin";
            }
            if (this.CreatedOn == null || DateTime.MinValue == this.CreatedOn)
            {
                this.CreatedOn = DateTime.Now;
            }
            if (this.ModifiedOn == null || DateTime.MinValue == this.ModifiedOn)
            {
                this.ModifiedOn = DateTime.Now;
            }
        }


        public void OnValidate()
        {
            if (this.DocNo == null)
            {
                throw new Exception("订单编号不能为空");
            }
        }
      
    }
}
