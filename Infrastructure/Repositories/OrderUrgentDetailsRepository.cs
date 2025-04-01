using Infrastructure.Migrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderUrgentDetailsRepository : BaseRepository<OrderUrgentDetails>, IOrderUrgentDetailsRepository
    {
        private readonly ApplicationDbContext db;
        public OrderUrgentDetailsRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }
    }
}
