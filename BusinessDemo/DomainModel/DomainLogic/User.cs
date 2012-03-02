using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelChina.TRF.Domain.Core;

namespace TelChina.TRF.Demo.DomainModel.Model
{
    partial class User : IEntity
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
            if (DateTime.MinValue == this.CreatedOn)
            {
                this.CreatedOn = DateTime.Now;
            }
            if (DateTime.MinValue == this.ModifiedOn)
            {
                this.ModifiedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// <see cref="TelChina.TRF.Domain.Core.IEntity"/>
        /// </summary>
        public void OnValidate()
        {
            if (this.Code == null)
            {
                throw new Exception("用户编号不能为空");
            }
            if (this.Name == null)
            {
                throw new Exception("用户姓名不能为空");
            }
        }
    }
}
