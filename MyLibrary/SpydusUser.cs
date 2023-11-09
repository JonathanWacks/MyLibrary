using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyLibrary
{
    public interface ITracingService
    {
        void WriteLine(string line);
    }

    public partial class SpydusUser : ILibraryUser
    {
        private readonly HttpClient _client;               

        private readonly string _affiliate;
        private readonly string _username;
        private readonly string _password;
        private readonly ITracingService _trace;
        private bool _loggedIn = false;
        private string _booksLink;

        private IDictionary<string, object> Header { get; set; }

        public SpydusUser(string username, string password, string affiliate, ITracingService trace)
        {
            _affiliate = affiliate;
            _username = username;
            _password = password;
            _trace = trace;

             _client = new HttpClient
             {
                 BaseAddress = new Uri($"https://{_affiliate}.spydus.co.uk/SpydusMobileAPI/ISorcer/")
             };
        }

        private async Task<dynamic> CallAPI(string apiUrl, Dictionary<string, object> dataIn)
        {
            var plistString = NSObject.Wrap(dataIn).ToXmlPropertyList();

            var response = await _client.PostAsync(apiUrl, new StringContent(plistString));

            var responseXML = await (response.Content as StreamContent).ReadAsStringAsync();

            // remove keys that aren't followed by a string, array or dict
            // as the parser can't handle keys without a corresponding value
            var fixedResponseXML = Regex.Replace(responseXML, @"<key>[^<]*<\/key>(?!<string|<array|<dict)", string.Empty);

            return PropertyListParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(fixedResponseXML)));
        }
        
        public async Task LoginAsync()
        {
            Header = new Dictionary<string, object> { 
                { "UserIrn", "-1"},
                { "Version", "0.0.0" }
            };

            var dataIn = new Dictionary<string, object> {
                { "Header", Header },
                { "Data", new Dictionary<string, object> {
                        { "Barcode", _username },
                        { "DeviceID", "" },
                        { "LibID", "1239" },
                        { "Password", _password }
                    }
                }
            };
            
            _trace?.WriteLine($"Logging in {_username}");
            
            var responseObject = await CallAPI("MILAuth", dataIn);

            Header = new Dictionary<string, object> {
                { "Version", responseObject["Header"]["Version"] },
                { "UserIrn", responseObject["Header"]["UserIrn"] },
                { "BrwdIrn", responseObject["Data"]["BrwdIrn"] },
                { "NameIrn", responseObject["Data"]["NameIrn"] },
                { "LocIrn", "34198968" }
            };

            _loggedIn = true;
        }

        public async Task GetAccountAsync()
        {
            if(!_loggedIn)
            {
                throw new Exception("Not logged in");
            }
            
            var dataIn = new Dictionary<string, object> {
                { "Header", Header },
                { "Data", new Dictionary<string, object> { } }
            };

            _trace?.WriteLine($"Getting account for {_username}");

            var responseObject = await CallAPI("GetAccountSummary", dataIn);

            var loans = ((IEnumerable<dynamic>)responseObject["Data"]["Groups"])
                .Single(g => g["RowID"].Content == "LOANS");

            var line = ((IEnumerable<dynamic>)loans["Lines"])
                .Single(l => l["LineId"].Content == "CURRENT");

            _booksLink = line["Link"].Content;             
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            if (_booksLink == null)
            {
                throw new Exception("Not fetched account");
            }

            if(_booksLink == String.Empty)
            {
                return new List<Book>();
            }

            var dataIn = new Dictionary<string, object> {
                { "Header", Header },
                { "Data", new Dictionary<string, object> {
                        { "QueryString", _booksLink },
                        { "LineId", "CURRENT" }
                    }
                }
            };

            _trace?.WriteLine($"Getting books for {_username}");

            var responseObject = await CallAPI("GetSummaryItemList", dataIn);

            var records = responseObject["Data"]["Records"] as IEnumerable<dynamic>;

            return records.Select(record => new Book { 
                Title = record["Lines"]["TITLE"]["Text"].Content, 
                DueDate = DateTime.Parse(record["Lines"]["DUE"]["Text"].Content),
                IRN = record["IRN"].Content,
                SmallImage = record["ImageData"].Content
            });
        }

        public async Task<string> GetImageTokenAsync(Book book)
        {
            var dataIn = new Dictionary<string, object> {
                { "Header", Header },
                { "Data", new Dictionary<string, object> {
                        { "LoanIrn", book.IRN },
                        { "Provider", string.Empty }
                    }
                }
            };

            _trace?.WriteLine($"Getting image token for {book}");

            var responseObject = await CallAPI("MILFindItemDetails", dataIn);

            return responseObject["Data"]["Info"]["ImgToken"].ToString();
        }

        public async Task<Stream> GetImageStreamAsync(string imageToken)
        {
            _trace?.WriteLine($"Getting image for {imageToken}");

            return await _client.GetStreamAsync($"MILGetImageWithToken?version=2.0.4&token={imageToken}");            
        }        

        public async Task<Renewal> RenewBookAsync(Book book)
        {
            var dataIn = new Dictionary<string, object> {
                { "Header", Header },
                { "Data", new Dictionary<string, object> {
                        { "LoanIRN", book.IRN },
                        { "IsCommit", "0" }
                    }
                }
            };

            _trace?.WriteLine($"Renewing {book}");

            var responseObject = await CallAPI("RenewLoan", dataIn);

            var status = int.Parse(responseObject["Data"]["RenewStatus"].Content);

            return status switch {
                0 => new FailedRenewal(responseObject["Data"]["Message"].Content),
                1 => new SuccessfulRenewal(responseObject["Data"]["RnwTxt"].Content,DateTime.Parse(responseObject["Data"]["DUE"].Content)),
                _ => throw new Exception($"Unknown renewal status {status}")
            };
        }        
    }
}
