using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Img { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
