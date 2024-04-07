namespace TicTacToeGame.API.Application
{
    public class PlayerTurnControlUseCase
    {
        private readonly GameDataInitializer _gameDataInitializer;

        public PlayerTurnControlUseCase(GameDataInitializer gameDataInitializer)
        {
            _gameDataInitializer = gameDataInitializer;
        }

        public bool Run(string roomId,string connectionId)
        {
            if ((_gameDataInitializer.getMoveCounter()[roomId] % 2 != 0 && _gameDataInitializer.getSymbolP()[connectionId] == "X") ||
               (_gameDataInitializer.getMoveCounter()[roomId] % 2 == 0 && _gameDataInitializer.getSymbolP()[connectionId] == "O"))
            {
                return true;
            }
            else
            {
                return false;
            }
    }
    }

}
