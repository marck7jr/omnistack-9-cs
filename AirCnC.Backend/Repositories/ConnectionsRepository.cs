using AirCnC.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirCnC.Backend.Repositories
{
    public class ConnectionsRepository
    {
        private readonly Dictionary<string, User> _connections = new Dictionary<string, User>();

        public void Add(string uniqueId, User user)
        {
            if (!_connections.ContainsKey(uniqueId))
            {
                _connections.Add(uniqueId, user);
            }
        }

        public string GetUserId(Guid guid)
        {
            return _connections
                .FirstOrDefault(connection => connection.Value.Guid == guid).Key;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _connections.Values;
        }
    }
}
