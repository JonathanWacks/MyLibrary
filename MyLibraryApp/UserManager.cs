using System.Collections.Generic;
using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class UserManager : IUserManager
    {
        private readonly string _path;

        public UserManager(string path)
        {
            _path = path;
            using (var db = new SQLiteConnection(_path))
            {
                db.CreateTable<SqlUser>();
            }
        }

        public void Add(User user)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.InsertAsync(new SqlUser(user));
        }

        public void Delete(IId id)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.DeleteAsync(new SqlUser((SqlId)id));
        }

        public void Edit(User user)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.UpdateAsync(new SqlUser(user));
        }

        public User Get(IId id)
        {
            using (var db = new SQLiteConnection(_path))
            {
                return db.Get<SqlUser>(((SqlId)id).Id);
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = new SQLiteConnection(_path))
            {
                foreach(var user in db.Table<SqlUser>())
                {
                    yield return user;
                }
            }
        }
    }
}