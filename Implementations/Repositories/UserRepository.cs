

using fundit_server.Contexts;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;

namespace fundit_server.Implementations.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}