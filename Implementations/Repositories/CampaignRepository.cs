

using fundit_server.Contexts;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;

namespace fundit_server.Implementations.Repositories
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}