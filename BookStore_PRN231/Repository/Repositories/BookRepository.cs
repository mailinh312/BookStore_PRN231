using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public BookRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddNewBook(BookCreateDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    throw new Exception("Book must be not null!");
                }

                Book book = _context.Books.FirstOrDefault(x => x.Title.Equals(bookDto.Title));
                if (book != null)
                {
                    throw new Exception("Book existed!");
                }

                book = _mapper.Map<Book>(bookDto);
                book.Active = true;
                book.Author = _context.Authors.FirstOrDefault(x => x.AuthorId == book.AuthorId);
                book.Category = _context.Categories.FirstOrDefault(x => x.CategoryId == book.CategoryId);

                _context.Books.Add(book);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<BookDto> GetAllBooks()
        {
            try
            {
                List<Book> books = _context.Books.Include(x => x.Author).Include(x => x.Category).ToList();

                if (!books.Any())
                {
                    throw new Exception("List book is empty!");
                }
                List<BookDto> bookDTOs = _mapper.Map<List<BookDto>>(books);

                return bookDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<BookDto> GetBooksByAuthor(int id)
        {
            try
            {
                List<Book> books = _context.Books.Where(x => x.AuthorId == id).Include(x => x.Author).Include(x => x.Category).ToList();
                if (!books.Any())
                {
                    throw new Exception("List book is empty!");
                }
                List<BookDto> bookDTOs = _mapper.Map<List<BookDto>>(books);
                return bookDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<BookDto> GetBooksByCategory(int id)
        {
            try
            {
                List<Book> books = _context.Books.Where(x => x.CategoryId == id).Include(x => x.Author).Include(x => x.Category).ToList();
                if (!books.Any())
                {
                    throw new Exception("List book is empty!");
                }
                List<BookDto> bookDTOs = _mapper.Map<List<BookDto>>(books);
                return bookDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<BookDto> GetBooksByTitle(string title)
        {
            try
            {
                List<Book> books = _context.Books.Where(x => x.Title.ToUpper().Contains(title.ToUpper())).Include(x => x.Author).Include(x => x.Category).ToList();
                if (!books.Any())
                {
                    throw new Exception("There is no book found!");
                }
                List<BookDto> bookDTOs = _mapper.Map<List<BookDto>>(books);
                return bookDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public BookDto GetByBookById(int id)
        {
            try
            {
                Book book = _context.Books.Include(x => x.Category).Include(x => x.Author).FirstOrDefault(x => x.BookId == id);
                if (book == null)
                {
                    throw new Exception("There is no book found!");
                }
                BookDto bookDto = _mapper.Map<BookDto>(book);
                return bookDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateBook(BookDto bookDto)
        {

            try
            {
                if (!_context.Books.Any(x => x.BookId == bookDto.BookId))
                {
                    throw new Exception("Book does not exist!");
                }

                Book book = _context.Books.FirstOrDefault(b => b.BookId == bookDto.BookId);
                _mapper.Map(bookDto, book);
                book.Author = _context.Authors.FirstOrDefault(x => x.AuthorId == bookDto.AuthorId);
                book.Category = _context.Categories.FirstOrDefault(x => x.CategoryId == bookDto.CategoryId);

                _context.Books.Update(book);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
