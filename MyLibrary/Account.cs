namespace MyLibrary
{
    public class Account : IIdable
    {        
        public Library Library { get; set; }
        public User User { get; set; }
        public IId Id { get; set; }

        public override string ToString()
        {
            return $"{Library}: {User}";
        }
    }
}
