using Contracts.Domain;
using Microsoft.AspNetCore.SignalR;

namespace TicTacToeGame.API.Infrastructure
{
    public class TicTacToeGameHub: Hub
    {
        private readonly Repository _repository;
        
        private Dictionary<string, char[,]> _gameStatus = new Dictionary<string, char[,]>();
        private Dictionary<string, int> _playersInRoom = new Dictionary<string, int>();
        private Dictionary<string, string> _currentPlayer = new Dictionary<string, string>();
        private Dictionary<string, string> _symbolP = new Dictionary<string, string>();
        private Dictionary<string, int> _moveCounter = new Dictionary<string, int>();
        public TicTacToeGameHub(Repository repository, Dictionary<string, char[,]> gameStatus, Dictionary<string, int> playersInRoom, Dictionary<string, string> currentPlayer, Dictionary<string, int> moveCounter, Dictionary<string, string> symbolP)
        {
            _repository = repository;
            _gameStatus = gameStatus;
            _playersInRoom = playersInRoom;
            _currentPlayer = currentPlayer;
            _moveCounter = moveCounter;
            _symbolP = symbolP;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceivedMessage", $"este viene del oncconect {Context.ConnectionId }");
        }

        public async Task JoinSpecificGameRoom( string name, string gameRoom)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameRoom);
                await Clients.Group(gameRoom).SendAsync("JoinSpecificGameRoom", $"{name} has joined {gameRoom}");

                // Verificar si ya hay dos jugadores en la sala
                if (_playersInRoom.ContainsKey(gameRoom) && _playersInRoom[gameRoom] >= 2)
                {
                    // Si ya hay dos jugadores, enviar un mensaje de sala llena al cliente
                    await Clients.Caller.SendAsync("RoomFull", "The room is already full. Try joining another room.");
                    return;
                }

               

                if (!_gameStatus.ContainsKey(gameRoom))
                {
                    // Si no existe, inicializar el estado del juego para esta sala
                    _gameStatus[gameRoom] = new char[3, 3]; // Por ejemplo, un tablero de 3x3 para Tic Tac Toe
                    _playersInRoom[gameRoom] = 1;
                    _currentPlayer[gameRoom] = Context.ConnectionId; // Establecer al primer jugador como jugador activo
                    _symbolP[Context.ConnectionId] = "X";
                    await Clients.Client(Context.ConnectionId).SendAsync("symbol", "X"); // Enviar el símbolo correspondiente al primer jugador
                }
                else
                {
                    _playersInRoom[gameRoom] += 1;
                    _symbolP[Context.ConnectionId] = "O";
                    await Clients.Client(Context.ConnectionId).SendAsync("symbol", "O"); // Enviar el símbolo correspondiente al segundo jugador
                    await Clients.Group(gameRoom).SendAsync("start", "started game");
                }
               
            }
            catch (Exception ex)
            {
                // Envía el mensaje de error de vuelta al cliente
                Console.WriteLine(ex);
            }

        }


        public async Task ExecuteMotion(string roomId, int row, int column)
        {
            Console.WriteLine($"{_moveCounter[roomId]}  {_symbolP[Context.ConnectionId]}");
            if ((_moveCounter[roomId] % 2 != 0 && _symbolP[Context.ConnectionId] == "X") ||
                (_moveCounter[roomId] % 2 == 0 && _symbolP[Context.ConnectionId] == "O")) 
            {
                // Si no es el turno del jugador actual, enviar un mensaje de error al cliente
                await Clients.Caller.SendAsync("ErrorMov", "It's not your turn.");
                return;
            }

            if (ValidateMove(roomId, row, column))
            {
                char playerSymbol =  '-';
                char[,] gameState = _gameStatus[roomId];
                if (_moveCounter[roomId] %2 ==0) {
                    playerSymbol = 'X' ;
                }
                else
                {
                    playerSymbol = '0';
                }

                // Realizar el movimiento
                gameState[row, column] = playerSymbol;

                // Incrementar el contador de movimientos para esta sala
                _moveCounter[roomId]++;

                // Notificar a todos los jugadores en la sala sobre el movimiento realizado y el nuevo estado del juego
                await Clients.Group(roomId).SendAsync("MoveDone", row, column, playerSymbol);

                // Cambiar el jugador activo al otro jugador
                _currentPlayer[roomId] = (_currentPlayer[roomId] == "X") ? "O" : "X";

                // Determinar si hay un ganador después del movimiento
                char winner = DetermineWinner(roomId);
                if (winner != '\0')
                {
                    // Notificar a todos los jugadores sobre el ganador
                    await Clients.Group(roomId).SendAsync("Winner", winner);

                    // Limpiar el estado del juego
                    cleanGameState(roomId);
                }
                await Clients.Group(roomId).SendAsync("TurnMessage", _symbolP[Context.ConnectionId]);
            }
            else
            {
                // Si el movimiento no es válido, enviar un mensaje de error al cliente
                await Clients.Caller.SendAsync("ErrorMov", "Invalid move.");
            }
        }

        private bool ValidateMove(string roomId,int row, int column)
        {
            // Obtener el estado actual del juego
            char[,]? gameState = _gameStatus.ContainsKey(roomId) ? _gameStatus[roomId] : null;

            if (gameState == null || row < 0 || row >= 3 || column < 0 || column >= 3 || gameState[row, column] != 0) 
            {
                // Movimiento no válido si el estado del juego es nulo, la fila/columna está fuera del rango o la casilla ya está ocupada
                return false;
            }

            // Movimiento válido
            return true;
        }

        private char GetPlayerSymbol(string roomId)
        {
            char[,]? gameState = _gameStatus.ContainsKey(roomId) ? _gameStatus[roomId] : null;

            if (gameState == null)
            {
                // Si el estado del juego es nulo, asignar 'X' al primer jugador
                return 'X';
            }
            else
            {
                // Contar el número de movimientos realizados en el estado actual del juego
                int count = gameState.Cast<char>().Count(s => s != 0);

                // Alternar entre 'X' y 'O' dependiendo del número de movimientos realizados
                return count % 2 == 0 ? 'X' : 'O';
            }
        }

        private char DetermineWinner(string roomID)
        {
            // Obtener el estado actual del juego
            char[,]? gameState = _gameStatus.ContainsKey(roomID) ? _gameStatus[roomID] : null;

            if (gameState == null)
            {
                // Si el estado del juego es nulo, no hay ganador
                return '\0'; // Carácter nulo, indica que no hay ganador
            }

            // Comprobar filas
            for (int row = 0; row < 3; row++)
            {
                if (gameState[row, 0] != '\0' && gameState[row, 0] == gameState[row, 1] && gameState[row, 0] == gameState[row, 2])
                {
                    return gameState[row, 0]; // Retorna el símbolo del ganador
                }
            }

            // Comprobar columnas
            for (int column = 0; column < 3; column++)
            {
                if (gameState[0, column] != '\0' && gameState[0, column] == gameState[1, column] && gameState[0, column] == gameState[2, column])
                {
                    return gameState[0, column]; // Retorna el símbolo del ganador
                }
            }

            // Comprobar gameState
            if (gameState[0, 0] != '\0' && gameState[0, 0] == gameState[1, 1] && gameState[0, 0] == gameState[2, 2])
            {
                return gameState[0, 0]; // Retorna el símbolo del ganador
            }

            if (gameState[0, 2] != '\0' && gameState[0, 2] == gameState[1, 1] && gameState[0, 2] == gameState[2, 0])
            {
                return gameState[0, 2]; // Retorna el símbolo del ganador
            }

            // Si no hay ganador, retorna el carácter nulo
            return '\0';
        }
        public void cleanGameState(string roomId)
        {
            // Eliminar el estado del juego de la sala
            _gameStatus.Remove(roomId);
        }

    }
}
