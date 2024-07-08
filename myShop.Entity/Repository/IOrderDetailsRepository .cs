﻿using myShop.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myShop.Entity.Repository
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        void Update(OrderDetails  orderDetails );
    }
}
