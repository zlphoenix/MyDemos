using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Runtime.Remoting.Messaging;
using TelChina.TRF.Persistant.Core.Resource;

namespace TelChina.TRF.Domain.Core
{
    public class EDMRepositoryContext :
        IRepositoryContext, IDisposable
    {
        #region Members
        private const string REPOSITORYCONTEXT = "CacheKey_RepositoryContext";
        private ObjectContext objContext;
        private Dictionary<Type, object> repositoryCache = new Dictionary<Type, object>();
        #endregion Members
        #region Constructor

        private EDMRepositoryContext(ObjectContext ctx)
        {
            this.objContext = ctx;
        }
        #endregion Constructor
        #region API
        /// <summary>
        /// 获取当前RepositoryContext
        /// </summary>
        /// <param name="ctx">EDM ObjectContext</param>
        /// <returns></returns>
        public static EDMRepositoryContext GetCurrentContext(ObjectContext ctx)
        {
            if (ctx == null)
            {
                throw new ArgumentNullException("ctx", Messages.exception_ObjectContextIsNull);
            }
            //优先从线程级换从中查找,找不到就创建并加入缓存
            var repositoryContextCache =
                CallContext.GetData(REPOSITORYCONTEXT)
                as Dictionary<ObjectContext, EDMRepositoryContext>;
            if (repositoryContextCache == null)
            {
                repositoryContextCache = new Dictionary<ObjectContext, EDMRepositoryContext>();
                CallContext.SetData(REPOSITORYCONTEXT, repositoryContextCache);
            }
            EDMRepositoryContext edmContext;
            if (!repositoryContextCache.TryGetValue(ctx, out edmContext))
            {
                edmContext = new EDMRepositoryContext(ctx);
                repositoryContextCache.Add(ctx, edmContext);
            }
            return edmContext;
        }

        #endregion
        #region ImplementInterfaces
        /// <summary>
        /// 根据当前ObjectCotext创建Repository实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityObject
        {
            //优先找缓存
            if (repositoryCache.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)repositoryCache[typeof(TEntity)];
            }
            IRepository<TEntity> repository = new Repository<TEntity>(this.objContext);
            this.repositoryCache.Add(typeof(TEntity), repository);
            return repository;
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            //释放线程级缓存
            var repositoryContextCache =
                CallContext.GetData(REPOSITORYCONTEXT) as Dictionary<ObjectContext, EDMRepositoryContext>;
            if (repositoryContextCache != null)
            {
                //同一个线程访问多个EDMContext的情况应该不存在,
                //因为EDM是按照组件划分,组件间是通过分布式服务访问的,应该存在线程边界
                //保险起见还是加入了释放当前缓存后的判断
                repositoryContextCache.Remove(this.objContext);
                if (repositoryContextCache.Count == 0)
                {
                    //释放缓存
                    CallContext.FreeNamedDataSlot(REPOSITORYCONTEXT);
                }
            }
            //清空repository缓存
            this.repositoryCache.Clear();
            //释放ObjectContext
            this.objContext.Dispose();
        }
        #endregion
    }
}
