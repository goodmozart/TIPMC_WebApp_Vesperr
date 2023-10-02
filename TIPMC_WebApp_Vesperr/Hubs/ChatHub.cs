using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TIPMC_WebApp_Vesperr.Data;

namespace TIPMC_WebApp_Vesperr.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public static Dictionary<string, string> UsernameConnectionId = new();

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> GetConnectionId(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.FullName == username);

            if (UsernameConnectionId.ContainsKey(username))
            {
                UsernameConnectionId[username] = Context.ConnectionId;
            }
            else
            {
                UsernameConnectionId.Add(username, Context.ConnectionId);
            }

            return Context.ConnectionId;
        }
    }
}
