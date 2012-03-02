namespace TelChina.TRF.Persistant.CoreLib.Repository
{

    public interface IRepositoryTransactionContext : IRepositoryContext
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
