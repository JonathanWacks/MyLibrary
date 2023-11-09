//using HtmlAgilityPack;
//using MyLibrary;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
////using System.Web;

//public class SirsiDynixUser : ILibraryUser
//{
//    private string _libraryCardNumber;
//    private string _password;
//    private readonly string _affiliate;

//    private CookieContainer _cookies = new CookieContainer();

//    public IEnumerable<Book> Books => throw new System.NotImplementedException();

//    public void RenewAll()
//    {
//        throw new System.NotImplementedException();
//    }

//    public void RenewBooks(params Book[] book)
//    {
//        throw new System.NotImplementedException();
//    }

//    public struct PostData
//    {
//        public string j_username { get; set; }
//        public string j_password { get; set; }
//        [JsonProperty("t:formdata")]
//        public string tformdata { get; set; }
//        [JsonProperty("t:ac")]
//        public string tac => "$N";
//        [JsonProperty("t:submit")]
//        public string tsubmit => "[\"submit_0\",\"submit_0\"]";
//        public string hidden => "SYMWS";        
//        public string submit_0 => "Log+In";
//    }

//    public void Login()
//    {
//        var request = WebRequest.CreateHttp($"https://{_affiliate}.ent.sirsidynix.net.uk//client/en_GB/default/search/patronlogin/$N");

//        var response = request.GetResponse();

//        var body = response.GetResponseBody();   

//        var htmlDocument = new HtmlDocument();
//        htmlDocument.LoadHtml(body);

//        var formData = htmlDocument.DocumentNode.SelectSingleNode("//input[@name=\"t:formdata\"]").GetAttributeValue("value", string.Empty);

//        var postData = new PostData { j_username = _libraryCardNumber, j_password = _password, tformdata = formData };

//        _cookies = GetSessionCookie(response);

//        response = MakeRequest(postData, "$N");

//        body = response.GetResponseBody();

//        _cookies = GetSessionCookie(response);         
//    }

//    private CookieContainer GetSessionCookie(WebResponse response)
//    {
//        var cookie = response.Headers[HttpResponseHeader.SetCookie];

//        var parts = cookie.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

//        var parts1 = parts[0].Split('=');

//        var container = new CookieContainer();

//        container.Add(new Cookie(parts1[0], parts1[1], parts[1].Split('=')[1], $".{_affiliate}.ent.sirsidynix.net.uk"));

//        return container;
//    }

//    private WebResponse MakeRequest(PostData postData, string tac)
//    {
//        var request = WebRequest.CreateHttp($"https://{_affiliate}.ent.sirsidynix.net.uk/client/en_GB/default/search/patronlogin.loginpageform/PRODUCTION?&t:ac={tac}");
//        request.Method = "POST";
//        request.ContentType = "application/x-www-form-urlencoded";
//        request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                
//        //request.ServicePoint.Expect100Continue = false;

//        request.CookieContainer = _cookies;

//        //var s = $"t%3Aac=%24N&t%3Asubmit=%5B%22submit_0%22%2C%22submit_0%22%5D&{HttpUtility.UrlEncode("t:formdata")}={HttpUtility.UrlEncode(postData.tformdata)}&j_username={HttpUtility.UrlEncode(postData.j_username)}&j_password={HttpUtility.UrlEncode(postData.j_password)}&hidden=SYMWS&submit_0=Log+In";
//        string s = string.Empty;
//        using (var stream = new StreamWriter(request.GetRequestStream()))
//        {
//            stream.WriteLine(s);
//        }

//        return request.GetResponse();
//    }

//    public SirsiDynixUser(string libraryCardNumber, string password, string affiliate)
//    {
//        _libraryCardNumber = libraryCardNumber;
//        _password = password;
//        _affiliate = affiliate;
//    }    
//}