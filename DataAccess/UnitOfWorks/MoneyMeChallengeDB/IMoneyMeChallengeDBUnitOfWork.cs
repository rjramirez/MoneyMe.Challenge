using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;
using DataAccess.UnitOfWork.Base;

namespace DataAccess.UnitOfWorks.MoneyMeChallengeDB
{
    public interface IMoneyMeChallengeDBUnitOfWork : IBaseUnitOfWork
    {
        public IErrorLogRepository ErrorLogRepository { get; }
        public IAuditTrailRepository AuditTrailRepository { get; }
        public IQuoteRepository QuoteRepository { get; }
    }
}
