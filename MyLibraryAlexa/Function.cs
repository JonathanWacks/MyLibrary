using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using MyLibrary;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace MyLibraryAlexa
{
    public class Function
    {
        private Dictionary<string, ILibraryUser> _users;

        private Dictionary<string, ILibraryUser> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = ReadUsersAsync().GetAwaiter().GetResult();
                }

                return _users;
            }
        }

        private IEnumerable<Book> Books
        {
            get
            {
                return Users.SelectMany(user => user.Value.Books);
            }
        }

        private KeyValuePair<string, ILibraryUser> GetUserBySpokenName(string name)
        {  
            return Users.OrderBy(user => ComputeLevenshteinDistance(user.Key, name)).First();
        }

        private string GetUsers()
        {            
            var userNames = Users.Select(user => user.Key);

            return $"<speak>Users are <break time='200ms'/>" + userNames.Aggregate((s1, s2) => s1 + "<break time='200ms'/>" + s2) + "</speak>";

        }

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

        private async Task<Dictionary<string, ILibraryUser>> ReadUsersAsync()
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                // This client will access the US East 1 region.
                RegionEndpoint = RegionEndpoint.EUWest1
            };
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

            var users = Table.LoadTable(client, "Users");
            var config = new ScanOperationConfig
            {
                Select = SelectValues.AllAttributes                
            };

            var search = users.Scan(config);

            var set = await search.GetNextSetAsync();
            
            var salfordLogins = set.ToDictionary(match => match["Name"].AsString(), match => match["Logins"].AsListOfDocument().Single(login => login["Name"].AsString() == "Salford"));

            return salfordLogins.ToDictionary(
                salfordLogin => salfordLogin.Key, 
                salfordLogin => (ILibraryUser)new SpydusUser(
                    salfordLogin.Value["LibraryCardNo"].AsString(), 
                    salfordLogin.Value["Pin"].AsString(), 
                    salfordLogin.Value["Affiliate"].AsString(), null));
        }

        private string GetBooksBySpokenUserName(string name)
        {
            var user = GetUserBySpokenName(name);

            return GetBooks(user);
        }

        private string GetBooks(KeyValuePair<string, ILibraryUser> user)
        {
            var titles = user.Value.Books.Select(book => book.Title);

            if (titles.Count() == 0)
            {
                return $"{user.Key} does not currently have any books out";
            }

            return $"{user.Key}'s books are <break time='200ms'/>" + titles.Aggregate((s1, s2) => s1 + "<break time='200ms'/>" + s2);

        }

        private ILambdaLogger _log;

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {        
            SkillResponse response = new SkillResponse
            {
                Response = new ResponseBody()
            };
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            _log = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                _log.LogLine($"Default LaunchRequest made: 'Alexa, open My Library");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Ok, what do you want me to do?";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        _log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Goodbye";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        _log.LogLine($"AMAZON.StopIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Goodbye";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        _log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "You can say tell me my checked out books, or renew a book, or you can say exit... What can I help you with?";
                        break;
                    case "GetUsers":
                        _log.LogLine($"GetUsers sent: send users");
                        innerResponse = new SsmlOutputSpeech();
                        (innerResponse as SsmlOutputSpeech).Ssml = GetUsers();
                        break;
                    case "GetBooksByName":
                        _log.LogLine($"GetBooksByName sent: send checked out books");
                        var intent = input.Request as IntentRequest;
                        var name = intent.Intent.Slots["Name"].Value;
                        innerResponse = new SsmlOutputSpeech();
                        (innerResponse as SsmlOutputSpeech).Ssml = "<speak>" + GetBooksBySpokenUserName(name) + "</speak>";
                        break;
                    case "GetAllBooks":
                        _log.LogLine($"GetAllBooks sent: send all checked out books");
                        innerResponse = new SsmlOutputSpeech();
                        (innerResponse as SsmlOutputSpeech).Ssml = "<speak>" + GetAllBooks() + "</speak>";
                        break;
                    case "RenewAll":
                        _log.LogLine($"RenewAll sent: renew all books");
                        var intent3 = input.Request as IntentRequest;
                        innerResponse = new SsmlOutputSpeech();
                        (innerResponse as SsmlOutputSpeech).Ssml = RenewAll();
                        break;
                    case "RenewBooksDue":
                        _log.LogLine($"RenewBook sent: renew books due");
                        var intent2 = input.Request as IntentRequest;
                        var due = intent2.Intent.Slots["DueByDate"].Value;
                        innerResponse = new SsmlOutputSpeech();
                        (innerResponse as SsmlOutputSpeech).Ssml = RenewBooks(due);
                        break;
                    case "CheckDueByDate":
                        _log.LogLine($"CheckDueByDate sent: check when book is due back");
                        var intent4 = input.Request as IntentRequest;
                        var bookName = intent4.Intent.Slots["BookName"].Value;
                        innerResponse = new SsmlOutputSpeech();
                        var book = GetBookBySpokenName(bookName);
                        (innerResponse as SsmlOutputSpeech).Ssml = $"<speak>{book.Title} is due back by {book.DueDate.ToLongDateString()}</speak>"; 
                        break;
                    default:
                        _log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "What can I help you with?";
                        break;
                }                
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            return response;
        }

        private Book GetBookBySpokenName(string bookName)
        {
            return Books.OrderBy(book => ComputeLevenshteinDistance(book.Title, bookName)).First();
        }

        private string RenewBooks(string due)
        {
            var dueBy = DateTime.Parse(due);

            var any = false;

            foreach(var user in Users)
            {
                var booksToRenew = user.Value.Books.Where(book => book.DueDate <= dueBy);
                if (booksToRenew.Any())
                {
                    any = true;
                    user.Value.RenewBooks(booksToRenew.ToArray());
                }
            }

            if(!any)
            {
                return "<speak>No books are due by that date</speak>";
            }
            return GetBookRenewalStatuses();
        }

        private string GetAllBooks()
        {
            return Users.Select(GetBooks).Aggregate((s1, s2) => s1 + "<break time='200ms'/>" + s2);
        }

        private string RenewAll()
        {
            foreach (var user in Users)
            {
                user.Value.RenewAll();
            }

            return GetBookRenewalStatuses();
        }

        private string GetBookRenewalStatuses()
        {
            var renewedCount = Books.Count(book => book.Status is Renewed renewed && renewed.JustNow);

            var message = $"<speak>{renewedCount} book{(renewedCount == 1 ? string.Empty : "s")} have been renewed<break time='200ms'/>";

            var errors = Books.Where(book => book.Status is NotRenewed || book.Status is MyLibrary.Error);
            if (errors.Any())
            {
                message += "The following have not been renewed<break time='200ms'/>";

                message += errors.Select(error => error.Title + "<break time = '200ms' />" + GetStatus(error.Status)).Aggregate((s1, s2) => s1 + "<break time = '200ms' />" + s2);
            }

            message += "</speak>";

            return message;
        }

        private string GetStatus(RenewalStatus status)
        {
            switch (status)
            {
                case NotRenewed n:
                    switch (n.Reason)
                    {
                        case NotRenewedReason.AlreadyRenewedOrIssuedToday:
                            return "Already renewed or issued today";
                        default:
                            return "Unknown reason";
                    }

                case MyLibrary.Error e:
                    return e.Message;

                default:
                    return "Unknown status";
            }
        }
    }
}
