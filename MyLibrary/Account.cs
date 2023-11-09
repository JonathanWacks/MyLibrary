namespace MyLibrary
{
    public class Account
    {        
        public Library Library { get; set; }
        public User User { get; set; }
        public Login Login { get; set; }

        public ILibraryUser LibraryUser
        {
            get => new SpydusUser(Login.CardNo, Login.PIN, Library.Host, null);
        }

        public override string ToString() => $"{Library}: {User}";
    }
}
