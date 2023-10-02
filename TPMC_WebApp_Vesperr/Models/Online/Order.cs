using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TPMC_WebApp_Vesperr.Models.Online
{
    public class Order
    {
        [Key]
        public Guid OrdersId { get; set; }
        public string MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        //public ICollection<OrderItem> OrderItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
