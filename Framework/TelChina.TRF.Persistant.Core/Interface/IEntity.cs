using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.TRF.Domain.Core
{
    /// <summary>
    /// 领域实体需要需要实现的接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 设置本实体上的字段默认值
        /// </summary>
        void SetDefaultValue();
        /// <summary>
        /// 执行字段合法性检查
        /// </summary>
        void OnValidate();
        /// <summary>
        /// 实体主键
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// 版本控制字段,用于处理并发问题
        /// </summary>
        int SysVersion { get; set; }
    }
}
