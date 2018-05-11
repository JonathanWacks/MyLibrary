using System;
using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class SqlLibrary
    {               
        public SqlLibrary(Library library)
        {
            Name = library.Name;
            Uri = library.Uri.AbsoluteUri;
            Type = (int)library.Type;
            if (library.Id != null)
            {
                Id = ((SqlId)library.Id).Id;
            }
        }

        public SqlLibrary(SqlId id)
        {
            Id = id.Id;
        }

        public SqlLibrary() { }

        public static implicit operator Library(SqlLibrary sqlLibrary)
        {
            return new Library
            {
                Name = sqlLibrary.Name,
                Uri = new Uri(sqlLibrary.Uri),
                Type = (LibraryType)sqlLibrary.Type,
                Id = new SqlId(sqlLibrary.Id)
            };
        }

        public string Name { get; set; }
        public string Uri { get; set; }
        public int Type { get; set; }
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } 
    }
}