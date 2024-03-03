using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IBookRepository
    {
        List<BookDto> GetAllBooks();
        List<BookDto> GetBooksByTitle(string title);
        BookDto GetByBookById(int id);
        List<BookDto> GetBooksByCategory(int id);
        List<BookDto> GetBooksByAuthor(int id);

        void AddNewBook(BookCreateDto bookDto);
        void UpdateBook(BookDto bookDto);
    }
}
