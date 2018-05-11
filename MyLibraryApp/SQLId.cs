using MyLibrary;

namespace MyLibraryApp
{
    public class SqlId : IId
    {
        public SqlId(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}