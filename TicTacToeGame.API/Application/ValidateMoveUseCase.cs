namespace TicTacToeGame.API.Application
{
    public class ValidateMoveUseCase
    {
        private readonly GameDataInitializer _gameDataInitializer;
 

        public ValidateMoveUseCase(GameDataInitializer gameDataInitializer)
        {
            _gameDataInitializer = gameDataInitializer;
        }

        public ValidateAndMoveResponse Run(string roomId, int row, int column)
        {
            // Obtener el estado actual del juego
            char[,]? gameState = _gameDataInitializer.getGameStatus().ContainsKey(roomId) ? _gameDataInitializer.getGameStatus()[roomId] : null;

            if (gameState == null || row < 0 || row >= 3 || column < 0 || column >= 3 || gameState[row, column] != 0)
            {
                // Movimiento no válido si el estado del juego es nulo, la fila/columna está fuera del rango o la casilla ya está ocupada
                ValidateAndMoveResponse res = new ValidateAndMoveResponse(false,'-');
                return  res;
            }
            else
            {
                //movimiento valido
                char playerSymbol = '-';
                char[,] state = _gameDataInitializer.getGameStatus()[roomId];
                if (_gameDataInitializer.getMoveCounter()[roomId] % 2 == 0)
                {
                    playerSymbol = 'X';
                }
                else
                {
                    playerSymbol = '0';
                }

                // Realizar el movimiento
                state[row, column] = playerSymbol;

                // Incrementar el contador de movimientos para esta sala
                _gameDataInitializer.getMoveCounter()[roomId]++;
                ValidateAndMoveResponse res = new ValidateAndMoveResponse(true, playerSymbol);
                return res;
            }

           
        }
    }

    public class ValidateAndMoveResponse
    {
        public bool valid { get; set; }
        public char symbol { get; set; }

        public ValidateAndMoveResponse(bool valid, char symbol)
        {
            this.valid = valid;
            this.symbol = symbol;
        }
    }
}
