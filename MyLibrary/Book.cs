using System;
using System.Net;
using System.Text.RegularExpressions;

namespace MyLibrary
{
    public abstract class RenewalStatus
    {        
    }

    public class None : RenewalStatus
    {
        public override string ToString()
        {
            return "Empty status";
        }
    }

    public class Renewed : RenewalStatus
    {
        public int Times { get; set; }
        public bool JustNow { get; set; }

        public override string ToString()
        {
            if(JustNow)
            {
                return "Renewed";
            }
            return "Renewed " + Times + " times";
        }
    }

    public enum NotRenewedReason
    {
        AlreadyRenewedOrIssuedToday
    }

    public class NotRenewed : RenewalStatus
    {
        public NotRenewedReason Reason { get; set; }

        public override string ToString()
        {
            return Reason.ToString();
        }
    }

    public class Error : RenewalStatus
    {
        public string Message { get; set; }

        public override string ToString()
        {
            return "Error: " + Message;
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }

        public string IRN { get; set; }
        //public string RenewId { get; set; }
        //public string Link { get; set; }
        public RenewalStatus Status { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public string SmallImage { get; set; }

        //public byte[] GetCoverImageBytes(string affiliate)
        //{
        //    var isbn = GetIsbn(affiliate);

        //    var path = GetImagePath(isbn);

        //    return new WebClient().DownloadData(path);
        //}

        //private static string GetImagePath(string isbn)
        //{
        //    var request = WebRequest.CreateHttp($"https://www.ehaus-ssl5.co.uk/liblookup/liblookup-display-query.asp?xc=salfordlibraries.spydus.co.uk&keyword={isbn}&apikey=WMwE5VQDdxvuxkMDzpczQCay2_FEYMHn7dHZgYrcJGQ");

        //    var response = request.GetResponse();

        //    var body = response.GetResponseBody();

        //    return Regex.Match(body, "img%20class%3D%22jacket%22%20src%3D%22(https.+?JPG)").Groups[1].Value.Replace("%3A", ":");
        //}

        //private string GetIsbn(string affiliate)
        //{
        //    var request = WebRequest.CreateHttp($"https://{affiliate}.spydus.co.uk/{Link}");

        //    //request.CookieContainer = Cookies;

        //    var response = request.GetResponse();

        //    var body = response.GetResponseBody();

        //    return Regex.Match(body, "ISBN=(.{13})").Groups[1].Value;
        //}
    }
}