﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myShop.Entity.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [ValidateNever]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category  Category { get; set; }
    }
}