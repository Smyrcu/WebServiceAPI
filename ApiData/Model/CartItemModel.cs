﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiData.Model
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Qty { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public int CurrencyId { get; set; }
    }
}
