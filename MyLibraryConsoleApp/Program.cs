using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using MyLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MyLibraryConsoleApp
{
    class Program
    {
        //private static Dictionary<string, ILibraryUser> _users;

        //private static ILibraryUser GetUser(string name)
        //{
        //    if (_users == null)
        //    {
        //        _users = ReadUsers();
        //    }

        //    return _users.OrderBy(user => ComputeLevenshteinDistance(user.Key, name)).First().Value;
        //}

        private static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        //private static Dictionary<string, ILibraryUser> ReadUsers()
        //{
        //    AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
        //    // This client will access the US East 1 region.
        //    clientConfig.RegionEndpoint = RegionEndpoint.EUWest1;
        //    AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

        //    var users = Table.LoadTable(client, "Users");
        //    var config = new ScanOperationConfig
        //    {
        //        Select = SelectValues.AllAttributes,
        //    };

        //    var search = users.Scan(config);

        //    var set = search.GetNextSet();

        //    var salford_logins = set.ToDictionary(match => match["Name"].AsString(), match => match["Logins"].AsListOfDocument().Single(login => login["Name"].AsString() == "Salford"));

        //    return salford_logins.ToDictionary(
        //        salford_login => salford_login.Key,
        //        salford_login => (ILibraryUser)new SpydusUser(salford_login.Value["LibraryCardNo"].AsString(), salford_login.Value["Pin"].AsString(), salford_login.Value["Affiliate"].AsString()));
        //}

        static void Main(string[] args)
        {
            //var user = new SirsiDynixUser("5000720888", "TCoM1Tlf2", "bury");
            //user.Login();

            //var user = GetUser("Malka");
            //DisplayBooks(user.Books);
            //var book = user.Books.Single(b => b.Title.StartsWith("Thief"));
            //user.RenewAll();
            //DisplayBooks(user.Books);
            //var spydus = new SpydusUser("P03548473", "1981", "trafford");
            //var books = spydus.GetCheckedOutBooks();

            //var first = books.Books.First();
            //var imageBytes = first.GetCoverImageBytes();
            //File.WriteAllBytes("testbook.jpg", imageBytes);

            var tracingService = new TracingService();

            //var malcaAtSalford = new SpydusUser("0002529688", "1986", "salfordlibraries", tracingService);
            //var meAtSalford = new SpydusUser("000252967X", "5741", "salfordlibraries", tracingService);
            //var ezraAtSalford = new SpydusUser("0002554887", "2011", "salfordlibraries", tracingService);
            //var benjaminAtSalford = new SpydusUser("0002554631", "2013", "salfordlibraries", tracingService);
            //var asherAtSalford = new SpydusUser("000286388X", "2017", "salfordlibraries", tracingService);

            //asherAtSalford.LoginAsync().Wait();
            //asherAtSalford.GetAccountAsync().Wait();
            //var books = asherAtSalford.GetBooksAsync().Result;

            //foreach(var book in books)
            //{
            //    Console.WriteLine(book.Title + " - " + book.DueDate);
            //}

            ////var response = asherAtSalford.RenewBookAsync(books.Last()).Result;            

            //var imageToken = asherAtSalford.GetImageTokenAsync(books.ElementAt(1)).Result;

            //var stream = asherAtSalford.GetImageStreamAsync(imageToken).Result;
            //var image = Image.FromStream(stream);
            //image.Save("test.jpg");
            var users = new SpydusUser[6];
            users[0] = new SpydusUser("D9999010826037", "0205", "manchester", tracingService);
            users[1] = new SpydusUser("D9999010723465", "0604", "manchester", tracingService);
            users[2] = new SpydusUser("D9999010821966", "2017", "manchester", tracingService);
            users[3] = new SpydusUser("D9999010826041", "0605", "manchester", tracingService);
            users[4] = new SpydusUser("D9999010586089", "0304", "manchester", tracingService);
            users[5] = new SpydusUser("D9999010780037", "2104", "manchester", tracingService);

            //users[0] = new SpydusUser("000252967X", "5741", "salfordlibraries", tracingService);
            //users[1] = new SpydusUser("0002554887", "2011", "salfordlibraries", tracingService);
            //users[2] = new SpydusUser("0002554631", "2013", "salfordlibraries", tracingService);
            //users[3] = new SpydusUser("000286388X", "2017", "salfordlibraries", tracingService);
            //users[4] = new SpydusUser("0003252205", "2020", "salfordlibraries", tracingService);
            //users[5] = new SpydusUser("0003341127", "1986", "salfordlibraries", tracingService);

            //using (var f = File.Create("books.txt"))
            //{
            //    using (var bs = new StreamWriter(f))
            //    {
            foreach (var user in users)
                    {
                        user.LoginAsync().Wait();
                        user.GetAccountAsync().Wait();
                var books = user.GetBooksAsync().Result;

                if(!books.Any())
                {
                    Console.WriteLine("no books");
                }

                foreach (var book in books)
                {
                    Console.WriteLine(book.DueDate + ": " + book.Title);
                }

                
                //RenewLateBooks(user);
            }
            //    }
            //}
            ////var salford = new BuryUser("Jonathan", "Wacks", "5000720888", "TCoM1Tlf2");
            //var books = meAtSalford.Books;

            //DisplayBooks(books);

            //meAtSalford.RenewBooks(books.Books.Skip(8).Take(2).ToArray());
            //meAtSalford.RenewAll();

            //books = meAtSalford.GetCheckedOutBooks();
            //var malcaBooks = malcaAtSalford.Books;
            //DisplayBooks(malcaBooks);

            //RenewLateBooks(asherAtSalford);
        }

        //private static void RenewLateBooks(params SpydusUser[] users)
        //{
        //    //using (var f = File.AppendText("books.txt"))
        //    //{
        //        foreach (var user in users)
        //        {
        //            //WriteUser(user, f);
        //            RenewLateBooks(user);
        //        }
        //    //}
        //}

        private static void RenewLateBooks(SpydusUser user)
        {
            foreach (var book in user.GetBooksAsync().Result)
            {
                //Console.WriteLine(book.Title + " - " + book.DueDate.ToShortDateString() + ": " + book.Status);
                //if (book.DueDate <= DateTime.Today)
                //{
                //    Console.WriteLine("Adding to renew list");
                //}
                //Console.WriteLine("Renewing " + book.Title);

                var renewal = user.RenewBookAsync(book).Result;

                Console.WriteLine(renewal);
            }
            //user.RenewBooks(user.Books.Where(book => book.DueDate <= DateTime.Today).ToArray());
        }

        //private static void WriteUser(SpydusUser user, StreamWriter s)
        //{
        //    foreach(var book in user.Books)
        //    {
        //        s.WriteLine($"{book.Title} - {book.DueDate.ToShortDateString()} - {book.Status}");
        //    }
        //}

        //private static void DisplayBooks(IEnumerable<Book> books)
        //{
        //    foreach (var book in books)
        //    {
        //        Console.WriteLine(book.Title + " - " + book.DueDate.ToShortDateString() + ": " + book.Status);
        //    }
        //}
    }

    internal class TracingService : ITracingService
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
