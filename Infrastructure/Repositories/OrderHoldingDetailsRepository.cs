using Infrastructure.Migrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderHoldingDetailsRepository : BaseRepository<OrderHoldingDetails>, IOrderHoldingDetailsRepository
    {
        private ApplicationDbContext _db;
        public OrderHoldingDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
