using myShop.Entity.Models;
using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart> , IShoppingCartRepository
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public int DecreaseCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count; 
            return shoppingCart.Count;

        }

        public int IncreaseCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }
}
