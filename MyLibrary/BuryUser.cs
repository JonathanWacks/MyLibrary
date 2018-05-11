//using mshtml;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyLibrary
//{
//    public class BuryUser : ILibraryUser
//    {
//        private string _login;
//        private CookieContainer _cookies = new CookieContainer();

//        public BuryUser(string firstName, string lastName, string code, string pin)
//        {
//            GetCookies();

//            var request = WebRequest.CreateHttp("https://library.bury.gov.uk/patroninfo");

//            request.Method = "POST";

//            request.CookieContainer = _cookies;

//            request.AllowAutoRedirect = false;

//            using (var stream = new StreamWriter(request.GetRequestStream()))
//            {
//                stream.Write($"name={firstName}+{lastName}&code={code}&pin={pin}");
//            }
        
//            var response = request.GetResponse();

//            _login = response.Headers[HttpResponseHeader.Location];
//        }

//        private void GetCookies()
//        {
//            var request = WebRequest.CreateHttp("https://library.bury.gov.uk/patroninfo");

//            request.Method = "GET";

//            var response = request.GetResponse();

//            var cookies = response.Headers[HttpResponseHeader.SetCookie].Split(',');

//            foreach(var cookie in cookies)
//            {
//                var nameValue = cookie.Split(';')[0].Split('=');
//                var newCookie = new Cookie(nameValue[0], nameValue[1], "/", ".bury.gov.uk");
//                _cookies.Add(newCookie);
//            }            
//        }

//        public CheckedOutBooks GetCheckedOutBooks()
//        {
//            var request = WebRequest.CreateHttp($"https://library.bury.gov.uk{_login}");

//            request.Method = "GET";

//            request.CookieContainer = _cookies;

//            var body = GetResponseBody(request.GetResponse());

//            var htmlDocument = (IHTMLDocument2)new HTMLDocument();

//            htmlDocument.write(body);

//            var table = htmlDocument.all.OfType<IHTMLTable>().SingleOrDefault();

//            if(table != null)
//            {
//                var bookRows = table.rows.Cast<IHTMLTableRow>().Skip(2);               

//                return new CheckedOutBooks
//                {
//                    Books = bookRows.Select(GetBook)
//                };
//            }

//            return new CheckedOutBooks { Books = new List<Book>() };
//        }

//        private Book GetBook(IHTMLTableRow row)
//        {
//            var cells = row.cells.Cast<IHTMLElement>();
//            var title = cells.ElementAt(1).innerText.Split('/')[0].Trim();
//            var dueDate = DateTime.Parse(cells.ElementAt(3).innerText.Remove(0, 4));
//            var renewCell = ((IHTMLElementCollection)cells.ElementAt(0).children).Cast<IHTMLElement>().First();
//            var id = renewCell.id;
//            var value = renewCell.getAttribute("value");
//            return new Book { Title = title, DueDate = dueDate, RenewId = $"&{id}={value}"};
//        }

//        private static string GetResponseBody(WebResponse response)
//        {
//            using (var stream = new StreamReader(response.GetResponseStream()))
//            {
//                return stream.ReadToEnd();
//            }
//        }

//        public void RenewAll()
//        {
//            throw new NotImplementedException();
//        }

//        public void RenewBooks(IEnumerable<Book> books)
//        {
//            var request = WebRequest.CreateHttp($"https://library.bury.gov.uk{_login}");

//            request.Method = "POST";

//            request.CookieContainer = _cookies;

//            var concatenatedIds = books.Select(book => book.RenewId).Aggregate((s1, s2) => s1 + s2);

//            using (var stream = new StreamWriter(request.GetRequestStream()))
//            {
//                stream.Write($"renewsome=renewsome&currentsortorder=current_checkout{concatenatedIds}");
//            }

//            request.GetResponse();
//        }
//    }
//}
