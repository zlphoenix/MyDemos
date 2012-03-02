using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;

namespace TelChina.TRF.Domain.Core.Extentions
{
    internal static class ObjectStateManagerExtensions
    {
        /// <summary>
        /// 查询实体状态缓存中的所有托管实体
        /// </summary>
        /// <param name="osm">实体状态缓存</param>
        /// <returns></returns>
        public static IEnumerable<ObjectStateEntry>
            GetObjectStateEntries(this ObjectStateManager osm)
        {
            return osm.GetObjectStateEntries(~EntityState.Detached);
        }
        /// <summary>
        /// 在实体状态缓存中查找指定类型的实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="osm">实体状态缓存</param>
        /// <returns></returns>
        public static IEnumerable<ObjectStateEntry>
          GetObjectStateEntries<TEntity>(this ObjectStateManager osm) where TEntity : class,IEntity
        {
            return osm.GetObjectStateEntries().Where(entity => entity.Entity is TEntity);
        }

        /// <summary>
        /// 根据实体状态查找托管实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="osm">实体状态缓存</param>
        /// <param name="state">实体状态</param>
        /// <returns></returns>
        public static IEnumerable<ObjectStateEntry>
         GetObjectStateEntries<TEntity>(this ObjectStateManager osm, EntityState state) where TEntity : class,IEntity
        {
            return osm.GetObjectStateEntries(state).Where(entity => entity.Entity is TEntity);
        }
        /// <summary>
        /// 查询ObjectContext中指定类型的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity>
         GetManagedEntries<TEntity>(this ObjectContext ctx) where TEntity : class,IEntity
        {
            return ctx.ObjectStateManager.GetObjectStateEntries<TEntity>().Select(entity => entity.Entity as TEntity);
        }
    }
}
