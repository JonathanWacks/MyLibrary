using System.Collections.Generic;
using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class AccountManager : IAccountManager
    {
        public Account Get(IId id)
        {
            using (var db = new SQLiteConnection(_path))
            {
                return db.Get<SqlAccount>(((SqlId)id).Id).ToAccount(_libraryManager, _userManager);
            }
        }

        private readonly string _path;
        private readonly IUserManager _userManager;
        private readonly ILibraryManager _libraryManager;

        public AccountManager(string path, ILibraryManager libraryManager, IUserManager userManager)
        {
            _path = path;
            _libraryManager = libraryManager;
            _userManager = userManager;
            using (var db = new SQLiteConnection(_path))
            {
                db.CreateTable<SqlAccount>();
            }
        }

        public async void Add(Account account)
        {
            using (var db = new SQLiteAsyncConnection(_path))
            {
                await db.InsertAsync(new SqlAccount(account));
            }
        }

        public async void Delete(IId id)
        {
            var db = new SQLiteAsyncConnection(_path);
            await db.DeleteAsync((SqlId) id);
        }

        public async void Edit(Account account)
        {
            var db = new SQLiteAsyncConnection(_path);
            await db.UpdateAsync(new SqlAccount(account));
        }

        public IEnumerable<Account> GetAll()
        {
            using (var db = new SQLiteConnection(_path))
            {
                return db.Table<SqlAccount>().Select(sqlAccount => sqlAccount.ToAccount(_libraryManager, _userManager));
            }
        }       
    }
}