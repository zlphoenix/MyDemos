using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelChina.TRF.Domain.Core
{

    public interface IRepositoryTransactionContext : IRepositoryContext
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
