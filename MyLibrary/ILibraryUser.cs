namespace MyLibrary
{
    public interface ILibraryUser
    {
        CheckedOutBooks Books { get; }
        void RenewBooks(params Book[] book);
        void RenewAll();
    }
}