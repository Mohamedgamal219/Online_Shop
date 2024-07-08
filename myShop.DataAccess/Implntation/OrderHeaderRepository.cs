using myShop.Entity.Models;
using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader> , IOrderHeaderRepository
    {
        private readonly AppDbContext _context;

        public OrderHeaderRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(OrderHeader orderHeader)
        {
            _context.OrderHeaders.Update(orderHeader);
            _context.SaveChanges();
        }

        public void UpdateOrderStatus(int id, string orderStatus, string paymentStatus)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;

                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
    }
}
