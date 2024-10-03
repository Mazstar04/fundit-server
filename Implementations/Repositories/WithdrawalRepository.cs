

using fundit_server.Contexts;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;

namespace fundit_server.Implementations.Repositories
{
    public class WithdrawalRepository : BaseRepository<Withdrawal>, IWithdrawalRepository
    {
        public WithdrawalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}