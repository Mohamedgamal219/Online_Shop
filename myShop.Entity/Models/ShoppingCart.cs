using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "You Must Chose Between 1 to 100")]

        public int Count { get; set; }

        public string ApplicationUsersId { get; set; }
        [ForeignKey("ApplicationUsersId")]
        [ValidateNever]
        public ApplicationUsers ApplicationUsers { get; set; }
    }
}
