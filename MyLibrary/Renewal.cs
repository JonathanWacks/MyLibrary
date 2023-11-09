using System;

namespace MyLibrary
{
    public abstract class Renewal
    {

    }

    class SuccessfulRenewal : Renewal
    {
        private readonly string _renewalText;
        private readonly DateTime _dueDate;

        public SuccessfulRenewal(string renewalText, DateTime dueDate)
        {
            _renewalText = renewalText;
            _dueDate = dueDate;
        }

        public override string ToString()
        {
            return this._renewalText + " due on " + _dueDate;
        }
    }

    class FailedRenewal : Renewal
    {
        private readonly string _message;

        public FailedRenewal(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return "Failed renewal: " + _message;
        }
    }
}
