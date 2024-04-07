namespace TicTacToeGame.API.Application
{
    public class GameDataInitializer
    {

        private Dictionary<string, char[,]> _gameStatus = new Dictionary<string, char[,]>();
        private Dictionary<string, int> _playersInRoom = new Dictionary<string, int>();
        private Dictionary<string, string> _currentPlayer = new Dictionary<string, string>();
        private Dictionary<string, string> _symbolP = new Dictionary<string, string>();
        private Dictionary<string, int> _moveCounter = new Dictionary<string, int>();

        public GameDataInitializer(Dictionary<string, char[,]> gameStatus, Dictionary<string, int> playersInRoom, Dictionary<string, string> currentPlayer, Dictionary<string, string> symbolP, Dictionary<string, int> moveCounter)
        {
            _gameStatus = gameStatus;
            _playersInRoom = playersInRoom;
            _currentPlayer = currentPlayer;
            _symbolP = symbolP;
            _moveCounter = moveCounter;
        }

        public Dictionary<string,char[,]> getGameStatus()
        {
            return _gameStatus;
        }
        public Dictionary<string,int> getPlayerInRoom()
        {
            return _playersInRoom;
        }
        public Dictionary<string,string> getCurrentPlayer()
        {
            return _currentPlayer;
        }
        public Dictionary<string,string> getSymbolP()
        {
            return _symbolP;
        }
        public Dictionary<string,int> getMoveCounter()
        {
            return _moveCounter;
        }
    }
}
