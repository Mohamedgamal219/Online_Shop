﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOOrderDetails
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }

        public decimal Price { get; set; }
        public int Count { get; set; }

        public decimal total { get; set; }
    }
}
