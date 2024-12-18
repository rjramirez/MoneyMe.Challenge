using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;

namespace DataAccess.Repositories.MoneyMeChallengeDB
{
    public class ErrorLogRepository : BaseRepository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLogRepository(MoneyMeChallengeDBContext context) : base(context)
        {

        }
    }
}