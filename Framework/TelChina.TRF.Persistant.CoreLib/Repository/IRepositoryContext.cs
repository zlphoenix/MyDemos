using System;
using TelChina.TRF.Persistant.CoreLib.Entity;

namespace TelChina.TRF.Persistant.CoreLib.Repository
{
    /// <summary>
    /// 仓储对象管理器
    /// </summary>
    public interface IRepositoryContext : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityBase, IAgregateRoot;
    }
}
