using System;

namespace MyLibrary
{
    public class RenewalResponse
    {
        public string ErrorMessage { get; set; }
        public RenewStatus Status { get; set; }
        public string RenewalText { get; set; }
        public DateTime Due { get; set; }
    }

    public enum RenewStatus
    {

    }
}