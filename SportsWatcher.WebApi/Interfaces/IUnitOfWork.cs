using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IUnitOfWork
    {
        DbContext GetContext();
        void DetectChanges();
        void SaveChanges();
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        void CommitTransaction();   
        void RollbackTransaction();
    }
}
