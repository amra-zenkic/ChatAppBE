using ChatAppBE.Models;
using System.Collections.Concurrent;

namespace ChatAppBE.DataService
{
    public class SharedDB
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new ();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;
    }
}
