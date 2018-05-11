using MyLibrary;
using SQLite;

namespace MyLibraryApp
{
    internal class SqlUser
    {
        public SqlUser(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            if (user.Id != null)
            {
                Id = ((SqlId)user.Id).Id;
            }
        }

        public SqlUser(SqlId id)
        {
            Id = id.Id;
        }

        public SqlUser() { }

        public static implicit operator User(SqlUser sqlUser)
        {
            return new User
            {
                FirstName = sqlUser.FirstName,
                LastName = sqlUser.LastName,
                Id = new SqlId(sqlUser.Id)
            };
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}