namespace TicTacToeGame.API.Application
{
    public class DetermineWinner
    {
        private readonly GameDataInitializer _gameDataInitializer;

        public DetermineWinner(GameDataInitializer gameDataInitializer)
        {
            _gameDataInitializer = gameDataInitializer;
        }

        public char Run(string roomId)
        {
            // Obtener el estado actual del juego
            char[,]? gameState = _gameDataInitializer.getGameStatus().ContainsKey(roomId) ? _gameDataInitializer.getGameStatus()[roomId] : null;

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
    }
}
