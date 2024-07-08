using myShop.Entity.Models;
using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails> , IOrderDetailsRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailsRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(OrderDetails orderDetails)
        {
            _context.OrderDetails.Update(orderDetails);
            _context.SaveChanges();
        }
    }
}
