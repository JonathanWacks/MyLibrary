using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace MyLibrary
{
    public interface ILibraryUser
    {        
        Task LoginAsync();
        Task GetAccountAsync();

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<string> GetImageTokenAsync(Book book);

        Task<Stream> GetImageStreamAsync(string imageToken);

        Task<Renewal> RenewBookAsync(Book book);
    }    
}