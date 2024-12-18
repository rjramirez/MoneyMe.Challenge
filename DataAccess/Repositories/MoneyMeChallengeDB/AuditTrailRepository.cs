using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;

namespace DataAccess.Repositories.MoneyMeChallengeDB
{
    public class AuditTrailRepository : BaseRepository<AuditTrail>, IAuditTrailRepository
    {
        public AuditTrailRepository(MoneyMeChallengeDBContext context) : base(context)
        {

        }
    }
}
