﻿using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList  { get; set; }
        public decimal Total { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}