using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        public int Count { get; set; }
        public string ApplicationUsersId { get; set; }
    }


}
