using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOShoppingCartSamary
    {
        public IEnumerable<DTOShoppingCart> dTOShoppingCarts { get; set; }
        public decimal Total { get; set; }
        public DTOOrderHeader DTOOrderHeader { get; set; }
     }
}
