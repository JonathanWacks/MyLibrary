//using MyLibrary;
//using SQLite;

//namespace MyLibraryApp
//{
//    internal class SqlUser
//    {
//        public SqlUser(User user)
//        {
//            Name = user.Name;
//            if (user.Id != null)
//            {
//                Id = ((SqlId)user.Id).Id;
//            }
//        }

//        public SqlUser(SqlId id)
//        {
//            Id = id.Id;
//        }

//        public SqlUser() { }

//        public static implicit operator User(SqlUser sqlUser)
//        {
//            return new User
//            {
//                Name = sqlUser.Name,
//                Id = new SqlId(sqlUser.Id)
//            };
//        }

//        [PrimaryKey, AutoIncrement]
//        public int Id { get; set; }

//        public string Name { get; set; }        
//    }
//}