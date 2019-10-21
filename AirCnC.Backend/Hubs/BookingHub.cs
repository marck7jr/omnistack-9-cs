using AirCnC.Backend.Data;
using AirCnC.Backend.Repositories;
using AirCnC.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AirCnC.Backend.Hubs
{
    public class BookingHub : Hub
    {
        private readonly static ConnectionsRepository _connections = new ConnectionsRepository();
        private ApplicationDbContext _context;

        public BookingHub(ApplicationDbContext context) => _context = context;

        public override Task OnConnectedAsync()
        {
            var userGuid = Guid.Parse(Context.GetHttpContext().Request.Query["user"]);

            if (_context.Users.Find(userGuid) is User user)
            {
                _connections.Add(Context.ConnectionId, user);
                Console.WriteLine($"Usuário conectado: {{ id: {_connections.GetUserId(user.Guid)}, email: {user.Email} }}");
            }

            return base.OnConnectedAsync();
        }

        public async Task SendBooking(string to, string message)
        {
            Console.WriteLine($"to: {to}");
            Console.WriteLine($"message: {message}");
            var userToReceive = _connections.GetUserId(Guid.Parse(to));
            Console.WriteLine($"userToReceive: {userToReceive}");
            await Clients.Client(userToReceive).SendAsync("ReceiveBooking", to, message);
        }
    }
}
