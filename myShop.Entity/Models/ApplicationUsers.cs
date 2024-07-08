using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace myShop.Entity.Models
{
    public class ApplicationUsers:IdentityUser
    {
        [Required , MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required , MaxLength(100)]
        public string City { get; set; }
    }
}
