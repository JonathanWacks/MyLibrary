namespace MyLibrary
{
    public class User : IIdable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IId Id { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}