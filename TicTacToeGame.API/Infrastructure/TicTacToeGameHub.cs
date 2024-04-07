using Contracts.Domain;
using Microsoft.AspNetCore.SignalR;
using TicTacToeGame.API.Application;

namespace TicTacToeGame.API.Infrastructure
{
    public class TicTacToeGameHub: Hub
    {
        private readonly LimitPlayerByRoomUseCase _limitPlayerByRoomUseCase;
        private readonly TableInitializerUseCase _tableInitializerUseCase;
        private readonly DetermineWinner _determineWinner;
        private readonly CleanGameState _cleanGameState;
        private readonly ValidateMoveUseCase _validateMoveUseCase;
        private readonly PlayerTurnControlUseCase _playerTurnControlUseCase;
        private readonly GameDataInitializer _gameDataInitializer;

        public TicTacToeGameHub(LimitPlayerByRoomUseCase limitPlayerByRoomUseCase, TableInitializerUseCase tableInitializerUseCase, DetermineWinner determineWinner, CleanGameState cleanGameState, ValidateMoveUseCase validateMoveUseCase, PlayerTurnControlUseCase playerTurnControlUseCase, GameDataInitializer gameDataInitializer)
        {
            _limitPlayerByRoomUseCase = limitPlayerByRoomUseCase;
            _tableInitializerUseCase = tableInitializerUseCase;
            _determineWinner = determineWinner;
            _cleanGameState = cleanGameState;
            _validateMoveUseCase = validateMoveUseCase;
            _playerTurnControlUseCase = playerTurnControlUseCase;
            _gameDataInitializer = gameDataInitializer;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceivedMessage", $"User connected with id: {Context.ConnectionId }");
        }

        public async Task JoinSpecificGameRoom( string name, string gameRoom)
        {
            try
            {
                if (_limitPlayerByRoomUseCase.Run(gameRoom))
                {
                    await Clients.Caller.SendAsync("RoomFull", "The room is already full. Try joining another room.");
                    return;
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, gameRoom);
                await Clients.Group(gameRoom).SendAsync("JoinSpecificGameRoom", $"{name} has joined to room: {gameRoom}");

                if (_tableInitializerUseCase.Run(gameRoom,Context.ConnectionId))
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("symbol", "X");
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("symbol", "O");
                    await Clients.Group(gameRoom).SendAsync("start", "started game");
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        public async Task ExecuteMotion(string roomId, int row, int column)
        {
            if ( _playerTurnControlUseCase.Run(roomId,Context.ConnectionId)) 
            {
                await Clients.Caller.SendAsync("ErrorMov", "It's not your turn.");
                return;
            }

            var res = _validateMoveUseCase.Run(roomId, row, column);
            
            if (res.valid)
            {
                await Clients.Group(roomId).SendAsync("MoveDone", row, column, res.symbol);

                _gameDataInitializer.getCurrentPlayer()[roomId] = (_gameDataInitializer.getCurrentPlayer()[roomId] == "X") ? "O" : "X";

                char winner = _determineWinner.Run(roomId);
                if (winner != '\0')
                {  
                    await Clients.Group(roomId).SendAsync("Winner", winner);
                    _cleanGameState.Run(roomId);
                    await Clients.Group(roomId).SendAsync("clean");
                }
                await Clients.Group(roomId).SendAsync("TurnMessage", _gameDataInitializer.getSymbolP()[Context.ConnectionId]);
            }
            else
            {
                await Clients.Caller.SendAsync("ErrorMov", "Invalid move.");
            }
        }


    }
}
