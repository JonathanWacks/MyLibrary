using System.Collections.Generic;

namespace MyLibrary
{
    public interface IEntityManager<T> //where T : IIdable
    {
        void Add(T t);
        void Edit(T t);
        void Delete(int id);
        IEnumerable<T> GetAll();
        T Get(int id);
        int Count { get; }
    }
}