using Common.Constants;
using Common.DataTransferObjects.AuditTrail;
using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.MoneyMeChallengeDB;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;
using DataAccess.Services;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataAccess.UnitOfWorks.MoneyMeChallengeDB
{
    public sealed class MoneyMeChallengeDBUnitOfWork : IMoneyMeChallengeDBUnitOfWork
    {
        private readonly MoneyMeChallengeDBContext _context;
        private readonly IDbContextChangeTrackingService _dbContextChangeTrackingService;
        public MoneyMeChallengeDBUnitOfWork(MoneyMeChallengeDBContext context, IDbContextChangeTrackingService dbContextChangeTrackingService)
        {
            _context = context;
            _dbContextChangeTrackingService = dbContextChangeTrackingService;

            ErrorLogRepository = new ErrorLogRepository(_context);
            AuditTrailRepository = new AuditTrailRepository(_context);
            QuoteRepository = new QuoteRepository(_context);
            BlacklistRepository = new BlacklistRepository(_context);
        }

        public IErrorLogRepository ErrorLogRepository { get; private set; }

        public IAuditTrailRepository AuditTrailRepository { get; private set; }
        public IQuoteRepository QuoteRepository { get; private set; }
        public IBlacklistRepository BlacklistRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(string transactionBy)
        {
            List<Tuple<ContextChangeTrackingDetail, EntityEntry>> contextChangeTrackingDetail = _dbContextChangeTrackingService.TrackRevisionDetails(_context);
            int result = await _context.SaveChangesAsync();

            if (contextChangeTrackingDetail.Any())
            {
                await _dbContextChangeTrackingService.SaveAuditTrail(transactionBy, contextChangeTrackingDetail);
            }

            return result;
        }
    }
}
