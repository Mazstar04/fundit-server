

using fundit_server.Contexts;
using fundit_server.Entities;
using fundit_server.Interfaces.Repositories;

namespace fundit_server.Implementations.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}