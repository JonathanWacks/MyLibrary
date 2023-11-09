//using MyLibrary;
//using SQLite;

//namespace MyLibraryApp
//{
//    internal class SqlAccount
//    {     
//        public SqlAccount(Account account)
//        {
//            LibraryId = new SqlLibrary(account.Library).Id;
//            UserId = new SqlUser(account.User).Id;
//            if (account.Id != null)
//            {
//                Id = ((SqlId) account.Id).Id;
//            }
//            CardNo = account.Login.CardNo;
//            PIN = account.Login.PIN;
//        }

//        public SqlAccount(SqlId id)
//        {
//            Id = id.Id;
//        }

//        public SqlAccount() { }

//        public Account ToAccount(IEntityManager<Library> libraryManager, IEntityManager<User> userManager)
//        {
//            return new Account
//            {
//                Library = libraryManager.Get(LibraryId),
//                User = userManager.Get(UserId),
//                Id = Id,
//                Login = new Login { CardNo = CardNo, PIN = PIN }
//            };
//        }

//        public int LibraryId { get; set; }
//        public int UserId { get; set; }  
//        public string CardNo { get; set; }
//        public string PIN { get; set; }
        
//        [PrimaryKey, AutoIncrement]
//        public int Id { get; set; }   
//    }
//}