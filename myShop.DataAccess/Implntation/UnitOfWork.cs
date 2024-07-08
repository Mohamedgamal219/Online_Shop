using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader  { get; private set; }
        public IOrderDetailsRepository OrderDetails  { get; private set; }

        public IApplicationUsersRepository ApplicationUsers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetails = new OrderDetailsRepository(context);
            ApplicationUsers = new ApplicationUsersRepository(context);

        }
        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
