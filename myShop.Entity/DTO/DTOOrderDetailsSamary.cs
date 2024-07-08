using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOOrderDetailsSamary
    {
        public DTOOrderHeader dTOOrderHeaders {  get; set; }
        public IEnumerable<DTOOrderDetails> dTOOrderDetails { get; set; }
        public decimal Total { get; set; }
    }
}
