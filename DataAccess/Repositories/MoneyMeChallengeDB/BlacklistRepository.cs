using DataAccess.DBContexts.MoneyMeChallengeDB;
using DataAccess.DBContexts.MoneyMeChallengeDB.Models;
using DataAccess.Repositories.Base;
using DataAccess.Repositories.MoneyMeChallengeDB.Interfaces;

namespace DataAccess.Repositories.MoneyMeChallengeDB
{
    public class BlacklistRepository : BaseRepository<Blacklist>, IBlacklistRepository
    {
        public BlacklistRepository(MoneyMeChallengeDBContext context) : base(context)
        {

        }
    }
}