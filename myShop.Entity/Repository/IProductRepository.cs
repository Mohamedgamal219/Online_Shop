using myShop.Entity.Model;
using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
