using System;
using System.ComponentModel.DataAnnotations;

namespace TPMC_WebApp_Vesperr.Models.Online
{
    public class OrderItem
    {
        [Key]
        public Guid OrderId { get; set; }
        public string MemberId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    
}
