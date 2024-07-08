﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.DTO
{
    public class DTOOrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUsersId { get; set; }
        [ValidateNever]
        public ApplicationUsers ApplicationUsers { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; } 
        public decimal TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }
}
