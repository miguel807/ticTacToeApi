namespace Users.API.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string GameRoom { get; set; } = string.Empty ;

        public User(string name, string gameRoom)
        {
            Id = Guid.NewGuid();
            Name = name;
            GameRoom = gameRoom;
        }
    }
}
