using SQLite;

namespace MyLibrary
{
    public partial class SqlLibrary : Library, IIdable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}

//using MyLibrary;
//using SQLite;

//namespace MyLibraryApp
//{
//    internal class SqlLibrary
//    {               
//        public SqlLibrary(Library library)
//        {
//            Name = library.Name;
//            Host = library.Host;
//            //Type = (int)library.Type;
//            if (library.Id != null)
//            {
//                Id = ((SqlId)library.Id).Id;
//            }
//        }

//        public SqlLibrary(SqlId id)
//        {
//            Id = id.Id;
//        }

//        public SqlLibrary() { }

//        public static implicit operator Library(SqlLibrary sqlLibrary)
//        {
//            return new Library
//            {
//                Name = sqlLibrary.Name,
//                Host = sqlLibrary.Host,
//                //Type = (LibraryType)sqlLibrary.Type,
//                Id = new SqlId(sqlLibrary.Id)
//            };
//        }

//        public string Name { get; set; }
//        public string Host { get; set; }
//        //public int Type { get; set; }
//        [PrimaryKey, AutoIncrement]
//        public int Id { get; set; } 
//    }
//}