using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.ViewModels
{
    public class OrderVM
    {
        public OrderHeader  OrderHeader { get; set; }
        public IEnumerable<OrderDetails>  OrderDetails { get; set; }
    }
}
