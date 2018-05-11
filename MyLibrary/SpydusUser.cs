using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MyLibrary
{
    public class SpydusUser : ILibraryUser
    {
        CookieContainer _cookies = new CookieContainer();
        private string _brwlscn_443;
        private string _enquiryId;
        private CheckedOutBooks _books;

        public CheckedOutBooks Books => _books;
        private string _brwl_443;
        private string _affiliate;
        private string _renewAllLink;
        private string _renewSelectionLink;

        public SpydusUser(string username, string password, string affiliate)
        {
            _affiliate = affiliate;
            var request = WebRequest.CreateHttp($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/PGM/OPAC/CCOPT/LB/2");

            request.Method = "POST";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.WriteLine($"BRWLID={username}&BRWLPWD={password}&ISGLB=1&RDT=%2Fcgi-bin%2Fspydus.exe%2FMSGTRN%2FOPAC%2FBSEARCH%3FHOMEPRMS%3DGENPARAMS");
            }

            request.CookieContainer = _cookies;

            var response = request.GetResponse();

            var cookies = ((HttpWebResponse)response).Cookies;
            _brwlscn_443 = cookies["BRWLSCN_443"].Value;
            _brwl_443 = cookies["BRWL_443"].Value;

            _cookies.Add(cookies);

            _enquiryId = GetEnquiryId();

            _books = GetCheckedOutBooks();
        }

        private string GetEnquiryId()
        {
            var request = WebRequest.CreateHttp($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/ENQ/OPAC/BRWENQ/{_brwlscn_443}?QRY=%23{_brwlscn_443}&QRYTEXT=My%20details&ISGLB=0&NRECS=999");

            request.CookieContainer = _cookies;

            var response = request.GetResponse();

            var body = response.GetResponseBody();

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(body);

            var anchor = htmlDocument.DocumentNode.SelectSingleNode("//a[@id='urltabMYDETAILS']");

            return Regex.Match(anchor.Attributes["href"].Value, "/cgi-bin/spydus.exe/FULL/OPAC/BRWENQ/(.+)/").Groups[1].Value;
        }

        private CheckedOutBooks GetCheckedOutBooks()
        {
            var request = WebRequest.CreateHttp($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/ENQ/OPAC/LOANRENQ/{_enquiryId}?QRY=FULL(OND01%5C{_brwl_443}%20%2B%20ONC01%5C0)%20-%20(ONS01%5CCR%20/%20ONS01%5CCRNB%20/%20ONS01%5CLR%20/%20ONS01%5CL)%20-%20OND26%5C1&SQL=LEFT%20OUTER%20JOIN%20XAT%20ON%20XAT.IRN%20%3D%20%23TBLID.IRN%20WHERE%20%23TBLID.IRN%20NOT%20IN%20(SELECT%20XAT2.IRN%20FROM%20XAT%20AS%20XAT2%20WHERE%20XAT2.IRN%20%3D%20XAT.IRN%20AND%20XAT2.NAM%20%3D%20N%27OND.EBK%27%20AND%20XAT2.VAL%20%3D%20N%271%27)&QRYTEXT=Current%20Loans&FMT=CL&SETLVL=SET&NRECS=30&SEARCH_FORM=/cgi-bin/spydus.exe/FULL/OPAC/BRWENQ/{_enquiryId}/{_brwl_443},1");

            request.CookieContainer = _cookies;

            var response = request.GetResponse();

            var body = response.GetResponseBody();

            //var htmlDocument = new HtmlWeb().Load($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/ENQ/OPAC/LOANRENQ/{_enquiryId}?QRY=FULL(OND01%5C{_brwl_443}%20%2B%20ONC01%5C0)%20-%20(ONS01%5CCR%20/%20ONS01%5CCRNB%20/%20ONS01%5CLR%20/%20ONS01%5CL)%20-%20OND26%5C1&SQL=LEFT%20OUTER%20JOIN%20XAT%20ON%20XAT.IRN%20%3D%20%23TBLID.IRN%20WHERE%20%23TBLID.IRN%20NOT%20IN%20(SELECT%20XAT2.IRN%20FROM%20XAT%20AS%20XAT2%20WHERE%20XAT2.IRN%20%3D%20XAT.IRN%20AND%20XAT2.NAM%20%3D%20N%27OND.EBK%27%20AND%20XAT2.VAL%20%3D%20N%271%27)&QRYTEXT=Current%20Loans&FMT=CL&SETLVL=SET&NRECS=30&SEARCH_FORM=/cgi-bin/spydus.exe/FULL/OPAC/BRWENQ/{_enquiryId}/{_brwl_443},1");

            return GetBooksFromHtml(body);
        }

        private CheckedOutBooks GetBooksFromHtml(string body)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(body);

            var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

            if (table != null)
            {
                var bookRows = table.Descendants("tr").Skip(1);

                _renewAllLink = GetRenewAllLink(htmlDocument);

                _renewSelectionLink = GetRenewSelectionLink(htmlDocument);

                return new CheckedOutBooks
                {
                    Books = bookRows.Select(GetBook),
                    RenewAllLink = _renewAllLink,
                    RenewSelectionLink = _renewSelectionLink
                };
            }

            else
            {
                return new CheckedOutBooks
                {
                    Books = new List<Book>()
                };
            }
        }

        private string GetRenewSelectionLink(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("//input[@name='RNWSEL']").Attributes["value"].Value;
        }

        private string GetRenewAllLink(HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("//input[@name='RNWALL']").Attributes["value"].Value;
        }

        private Book GetBook(HtmlNode row)
        {
            var cells = row.SelectNodes("td");
            var svl = cells[0].SelectSingleNode("input").Attributes["value"].Value;
            var title = cells[1].SelectSingleNode("a").InnerText;
            var dueDate = DateTime.Parse(cells[2].InnerText);
            var link = cells[1].SelectSingleNode("a").Attributes["href"].Value;
            return new Book {
                Title = title,
                DueDate = dueDate, 
                RenewId = $"SVL={svl}",
                Link = link, //.Replace("about:", string.Empty), 
            };
        }

        public void RenewAll()
        {
            if(_renewAllLink == null || _renewSelectionLink == null)
            {
                var books = GetCheckedOutBooks();
                _renewAllLink = books.RenewAllLink;
                _renewSelectionLink = books.RenewSelectionLink;
            }

            var request = WebRequest.CreateHttp($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/PGM/OPAC/RFN");

            request.Method = "POST";

            request.CookieContainer = _cookies;

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.WriteLine($"RFN%5DRNWALL=&RNWSEL={WebUtility.HtmlEncode(_renewSelectionLink)}&RNWALL={WebUtility.HtmlEncode(_renewAllLink)}&NREC=0");
            }

            var response = request.GetResponse();

            var body = response.GetResponseBody();

            _books = GetBooksFromHtml(body);
        }

        public void RenewBooks(params Book[] books)
        {
            var request = WebRequest.CreateHttp($"https://{_affiliate}.spydus.co.uk/cgi-bin/spydus.exe/PGM/OPAC/RFN");

            request.Method = "POST";

            request.CookieContainer = _cookies;

            var concatenatedIds = books.Select(book => "&" + book.RenewId).Aggregate((s1, s2) => s1 + s2);

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                stream.WriteLine($"RFN%5DRNWSEL=&RNWSEL={WebUtility.HtmlEncode(_renewSelectionLink)}&RNWALL={WebUtility.HtmlEncode(_renewAllLink)}{concatenatedIds}");
            }
            
            var response = request.GetResponse();

            var body = response.GetResponseBody();

            _books = GetBooksFromHtml(body);       
        }
    }
}
