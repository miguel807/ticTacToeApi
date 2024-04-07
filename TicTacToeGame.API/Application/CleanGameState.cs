namespace TicTacToeGame.API.Application
{
    public class CleanGameState
    {
        private GameDataInitializer _gameDataInitializer;

        public CleanGameState(GameDataInitializer gameDataInitializer)
        {
            _gameDataInitializer = gameDataInitializer;
        }

        public void Run(string roomId)
        {
            _gameDataInitializer.getGameStatus().Remove(roomId);
            _gameDataInitializer.getPlayerInRoom().Remove(roomId);
            _gameDataInitializer.getCurrentPlayer().Remove(roomId);
            _gameDataInitializer.getSymbolP().Remove(roomId);
            _gameDataInitializer.getMoveCounter().Remove(roomId);
        }
    }
}
