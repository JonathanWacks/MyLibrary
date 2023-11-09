//using System;
//using System.Collections.Generic;

//namespace MyLibrary
//{
//    public class CheckedOutBooks
//    {
//        private string _renewAllLink;
//        private string _renewSelectionLink;

//        public IEnumerable<Book> Books { get; set; }

//        public string RenewAllLink
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(_renewAllLink))
//                {
//                    throw new Exception("Books cannot be renewed before checked out books are retrieved");
//                }
//                return _renewAllLink;
//            }
//            set { _renewAllLink = value; }
//        }

//        public string RenewSelectionLink {
//            get
//            {
//                if (string.IsNullOrEmpty(_renewSelectionLink))
//                {
//                    throw new Exception("Books cannot be renewed before checked out books are retrieved");
//                }
//                return _renewSelectionLink;
//            }
//            set { _renewSelectionLink = value; }
//        }
//    }
//}
