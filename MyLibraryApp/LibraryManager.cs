using System.Collections.Generic;
using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class LibraryManager : ILibraryManager
    {
        public Library Get(IId id)
        {
            using (var db = new SQLiteConnection(_path))
            {
                return db.Get<SqlLibrary>(((SqlId)id).Id);
            }
        }

        public void Add(Library library)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.InsertAsync(new SqlLibrary(library));
        }

        public void Edit(Library library)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.UpdateAsync(new SqlLibrary(library));
        }

        public void Delete(IId id)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.DeleteAsync(new SqlLibrary((SqlId)id));
        }

        public IEnumerable<Library> GetAll()
        {
            using (var db = new SQLiteConnection(_path))
            {
                foreach(var library in db.Table<SqlLibrary>())
                {
                    yield return library;
                }
            }
        }

        private readonly string _path;

        public LibraryManager(string path)
        {
            _path = path;
            using (var db = new SQLiteConnection(path))
            {
                db.CreateTable<SqlLibrary>();
            }
        }   
    }
}