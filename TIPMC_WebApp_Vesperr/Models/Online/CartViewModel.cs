﻿using System.Collections.Generic;

namespace TIPMC_WebApp_Vesperr.Models.Online
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
