using myShop.Entity.Models;
using myShop.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.DataAccess.Implntation
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public void Update(Product product)
        {
            var productInDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productInDb != null)
            {
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.Img = product.Img;
                productInDb.CategoryId = product.CategoryId;
                
            }
        }
    }
}
