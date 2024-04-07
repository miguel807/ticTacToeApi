namespace TicTacToeGame.API.Application
{
    public   class LimitPlayerByRoomUseCase
    {
        private  readonly GameDataInitializer _gameDatainitializer;

        public LimitPlayerByRoomUseCase(GameDataInitializer gameDatainitializer)
        {
            _gameDatainitializer = gameDatainitializer;
        }

        public  bool Run(string gameRoom)
        {
            if (_gameDatainitializer.getPlayerInRoom().ContainsKey(gameRoom) && _gameDatainitializer.getPlayerInRoom()[gameRoom] >= 2)
            {
                return true;
            }
            return false;
    }
    }
}
