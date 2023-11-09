namespace MyLibrary
{
    public class Library
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public override string ToString() => Name;
    }
}
