using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SportsWatcherDbContext _context;

        public UnitOfWork(SportsWatcherDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DbContext GetContext()
        {
            return _context;
        }

        public void DetectChanges()
        {
            _context.ChangeTracker.DetectChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
    }
}

