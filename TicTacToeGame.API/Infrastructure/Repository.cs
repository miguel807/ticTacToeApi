using Contracts.Domain;
using System.Collections.Concurrent;

namespace TicTacToeGame.API.Infrastructure
{
    public class Repository
    {
        private readonly ConcurrentDictionary<string, UserJoinRoomIntegrarionEvent> _connections = new();

        public  ConcurrentDictionary<string, UserJoinRoomIntegrarionEvent> connections => _connections;

    }
}
