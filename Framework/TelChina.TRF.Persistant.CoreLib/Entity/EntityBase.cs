using System;
using System.Collections;
using System.Collections.Generic;
using TelChina.TRF.Persistant.CoreLib.Repository;

namespace TelChina.TRF.Persistant.CoreLib.Entity
{
    /// <summary>
    /// 领域实体需要需要实现的接口
    /// </summary>
    public abstract class EntityBase : IPersistableObject
    {
        #region 实体提交事件
        /// <summary>
        /// 设置本实体上的字段默认值,应用开发扩展点
        /// </summary>
        protected abstract void SetDefaultValue();
        /// <summary>
        /// 执行字段合法性检查,应用开发扩展点
        /// </summary>
        protected abstract void OnValidate();

        /// <summary>
        /// 提交相关的事件处理:设置本实体上的字段默认值
        /// </summary>
        void IPersistableObject.SetDefaultValue()
        {
            //TODO Set System property default  value...
            //this.Id =...
            //this.ModifiedBy=
            this.SetDefaultValue();
        }
        /// <summary>
        /// 提交相关的事件处理:执行字段合法性检查
        /// </summary>
        void IPersistableObject.OnValidate()
        {
            //ToDo 基于模型的非空检查
            //this.GetType().GetProperties(EntityAttribute);
            throw new NotImplementedException();
        }
        #endregion

        #region 基础属性

        /// <summary>
        /// 实体主键
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 版本控制字段,用于处理并发问题
        /// </summary>
        public virtual int SysVersion { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifiedOn { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public virtual string ModifiedBy { get; set; }

        /// <summary>
        /// 实体所在的组件名称
        /// </summary>
        public abstract string EntityComponent { get; }

        #endregion

        #region 系统属性
        /// <summary>
        /// 实体状态
        /// </summary>
        public EntityStateEnum SysState { get; set; }
        #endregion
    }
}
