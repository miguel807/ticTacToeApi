namespace TicTacToeGame.API.Application
{
    public class TableInitializerUseCase
    {
        private readonly GameDataInitializer _gameDatainitializer;

        public TableInitializerUseCase(GameDataInitializer gameDatainitializer)
        {
            _gameDatainitializer = gameDatainitializer;
        }

        public bool Run(string gameRoom,string connectionID)
        {
            if (!_gameDatainitializer.getGameStatus().ContainsKey(gameRoom))
            {
               _gameDatainitializer.getGameStatus()[gameRoom] = new char[3, 3]; // Por ejemplo, un tablero de 3x3 para Tic Tac Toe
               _gameDatainitializer.getPlayerInRoom()[gameRoom] = 1;
               _gameDatainitializer.getCurrentPlayer()[gameRoom] = connectionID; // Establecer al primer jugador como jugador activo
                _gameDatainitializer.getSymbolP()[connectionID] = "X";
                return true;
            }
            else
            {
                _gameDatainitializer.getPlayerInRoom()[gameRoom] += 1;
                _gameDatainitializer.getSymbolP()[connectionID] = "O";
                return false;
            }
        }
    }
}
