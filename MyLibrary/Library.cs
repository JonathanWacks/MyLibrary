using System;

namespace MyLibrary
{
    public class Library : IIdable
    {
        public string Name { get; set; }
        public LibraryType Type { get; set; }
        public Uri Uri { get; set; }
        public IId Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
