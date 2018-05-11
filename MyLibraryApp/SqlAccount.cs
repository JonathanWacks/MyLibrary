using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class SqlAccount
    {     
        public SqlAccount(Account account)
        {
            LibraryId = new SqlLibrary(account.Library).Id;
            UserId = new SqlUser(account.User).Id;
            if (account.Id != null)
            {
                Id = ((SqlId) account.Id).Id;
            }
        }

        public SqlAccount(SqlId id)
        {
            Id = id.Id;
        }

        public SqlAccount() { }

        public Account ToAccount(ILibraryManager libraryManager, IUserManager userManager)
        {
            return new Account
            {
                Library = libraryManager.Get(new SqlId(LibraryId)),
                User = userManager.Get(new SqlId(UserId)),
                Id = new SqlId(Id)
            };
        }

        public int LibraryId { get; set; }

        public int UserId { get; set; }     
        
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }   
    }
}