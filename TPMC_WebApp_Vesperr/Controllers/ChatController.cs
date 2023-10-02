using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using TIPMC_WebApp_Vesperr.Hubs;
using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models.Chat;
using Microsoft.EntityFrameworkCore;
using TIPMC_WebApp_Vesperr.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ErrorViewModel = TIPMC_WebApp_Vesperr.Models.Chat.ErrorViewModel;
using System.Text;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var user1 = email;

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var allMessages = await _context.Messages.Where(x =>
                x.FromUserId == user.Id ||
                x.ToUserId == user.Id)
                .ToListAsync();

            var chats = new List<ChatViewModel>();

            // Assuming _context is your DbContext instance
            var usersWithNonBlankFullName = await _context.Users
                .Where(user => !string.IsNullOrWhiteSpace(user.FullName))
                .ToListAsync();

            foreach (var i in usersWithNonBlankFullName)
            {
                if (i == user) continue;

                var chat = new ChatViewModel()
                {
                    MyMessages = allMessages.Where(x => x.FromUserId == user.Id && x.ToUserId == i.Id).ToList(),
                    OtherMessages = allMessages.Where(x => x.FromUserId == i.Id && x.ToUserId == user.Id).ToList(),
                    RecipientName = i.FullName
                };

                var chatMessages = new List<Message>();
                chatMessages.AddRange(chat.MyMessages);
                chatMessages.AddRange(chat.OtherMessages);

                chat.LastMessage = chatMessages.OrderByDescending(x => x.Timestamp).FirstOrDefault();

                chats.Add(chat);
            }

            return View(chats);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SendMessage(string to, string text)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return StatusCode(500);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var recipient = await _context.Users.SingleOrDefaultAsync(x => x.FullName == to);
            var image = user.AvatarURL;

            if (user.Id != recipient.Id)
            {

                Message message = new Message()
                {
                    FromUserId = user.Id,
                    ToUserId = recipient.Id,
                    Text = text,
                    Timestamp = DateTime.Now,
                    Image = image,
                    Sender = user.FullName
                };

                await _context.AddAsync(message);
                await _context.SaveChangesAsync();

                string connectionId = ChatHub.UsernameConnectionId[recipient.FullName];

                await _hubContext.Clients.Client(connectionId).SendAsync("RecieveMessage", message.Text, message.Timestamp.ToShortTimeString(), message.Image, message.Sender);

                return Ok();

            }
            else
            {
                return Error();
            }
        }

        public void ToBytes(string text)
        {
            string inputString = text;
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
        }

        public void ToString(byte[] byteText)
        {
            byte[] bytes = byteText; // Your byte array here
            string resultString = Encoding.UTF8.GetString(bytes);

        }
    }
}
