//using System.Collections.Generic;
//using MyLibrary;
//using SQLite;

//namespace MyLibraryApp
//{
//    internal class AccountManager : IEntityManager<Account>
//    {
//        public Account Get(IId id)
//        {
//            using var db = new SQLiteConnection(_path);
//            return db.Get<SqlAccount>(((SqlId)id).Id).ToAccount(_libraryManager, _userManager);
//        }

//        private readonly string _path;
//        private readonly IEntityManager<User> _userManager;
//        private readonly IEntityManager<Library> _libraryManager;

//        public AccountManager(string path, IEntityManager<Library> libraryManager, IEntityManager<User> userManager)
//        {
//            _path = path;
//            _libraryManager = libraryManager;
//            _userManager = userManager;
//            using var db = new SQLiteConnection(_path);
//            db.CreateTable<SqlAccount>();
//        }

//        public async void Add(Account account)
//        {
//            var db = new SQLiteAsyncConnection(_path);
//            await db.InsertAsync(new SqlAccount(account));
//        }

//        public async void Delete(IId id)
//        {
//            var db = new SQLiteAsyncConnection(_path);
//            await db.DeleteAsync((SqlId) id);
//        }

//        public async void Edit(Account account)
//        {
//            var db = new SQLiteAsyncConnection(_path);
//            await db.UpdateAsync(new SqlAccount(account));
//        }

//        public IEnumerable<Account> GetAll()
//        {
//            using var db = new SQLiteConnection(_path);
//            foreach (var sqlAccount in db.Table<SqlAccount>())
//            {
//                yield return sqlAccount.ToAccount(_libraryManager, _userManager);
//            }
//        }       
//    }
//}