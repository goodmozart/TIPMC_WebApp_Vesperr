using System;

namespace TIPMC_WebApp_Vesperr.Models.Chat
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public string FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }

        public string ToUserId { get; set; }
        public ApplicationUser ToUser { get; set; }

        public string Image { get; set; }
        public string Sender { get; set; }
    }
}
