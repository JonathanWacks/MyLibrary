using MyLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MyLibraryConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var spydus = new SpydusUser("P03548473", "1981", "trafford");
            //var books = spydus.GetCheckedOutBooks();

            //var first = books.Books.First();
            //var imageBytes = first.GetCoverImageBytes();
            //File.WriteAllBytes("testbook.jpg", imageBytes);

            var meAtSalford = new SpydusUser("000252967X", "5741", "salfordlibraries");
            //var salford = new BuryUser("Jonathan", "Wacks", "5000720888", "TCoM1Tlf2");
            var books = meAtSalford.Books;

            var count = books.Books.Count();

            DisplayBooks(books.Books);

            meAtSalford.RenewBooks(books.Books.Skip(8).Take(2).ToArray());
            //meAtSalford.RenewAll();

            //books = meAtSalford.GetCheckedOutBooks();

            DisplayBooks(books.Books);
        }

        private static void DisplayBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                Console.WriteLine(book.Title + " - " + book.DueDate.ToShortDateString());
            }
        }
    }
}
