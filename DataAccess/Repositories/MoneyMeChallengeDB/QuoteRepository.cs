using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;

namespace DataAccess.Repositories.MoneyMeChallengeDB
{
    public class QuoteRepository : BaseRepository<Quote>, IQuoteRepository
    {
        public QuoteRepository(MoneyMeChallengeDBContext context) : base(context)
        {

        }
    }
}