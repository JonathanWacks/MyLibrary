using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class EntityManager<T> : IEntityManager<T> where T : new()
    {
        public T Get(int id)
        {
            using var db = new SQLiteConnection(_path);
            return db.Get<T>(id);
        }

        public void Add(T t)
        {
            using var db = new SQLiteConnection(_path);
            db.Insert(t);
        }

        public void Edit(T t)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.UpdateAsync(t);
        }

        public void Delete(int id)
        {
            var db = new SQLiteAsyncConnection(_path);
            db.DeleteAsync(id);
        }

        public IEnumerable<T> GetAll()
        {
            using var db = new SQLiteConnection(_path);
            return db.Table<T>().AsEnumerable();
        }

        private readonly string _path;

        public int Count
        {
            get
            {
                // TODO redo this using SQLiteOpenHelper?
                using var db = new SQLiteConnection(_path);
                var query = $"select count(*) from {typeof(T).Name}";
                return db.ExecuteScalar<int>(query);
            }
        }

        public EntityManager(string path)
        {
            _path = path;

            using var db = new SQLiteConnection(path);
            db.CreateTable<T>();
        }
    }
}