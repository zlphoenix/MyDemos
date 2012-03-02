using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace TelChina.TRF.Domain.Core
{

    public interface IRepositoryContext : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : EntityObject;
    }
}
