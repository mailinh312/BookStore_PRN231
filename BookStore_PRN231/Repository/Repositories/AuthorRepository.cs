using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        public AuthorRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddNewAuthor(AuthorDto authorDto)
        {
            try
            {
                if (_context.Authors.FirstOrDefault(x => x.AuthorName.ToUpper().Equals(authorDto.AuthorName)) != null)
                {
                    throw new Exception("Author existed!");
                }
                Author author = _mapper.Map<Author>(authorDto);
                author.AuthorId = 0;
                author.Active = true;
                _context.Authors.Add(author);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AuthorDto> GetAllAuthors()
        {
            try
            {
                List<Author> authors = _context.Authors.ToList();
                if (authors.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }
                var authorDtos = _mapper.Map<List<AuthorDto>>(authors);
                return authorDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public AuthorDto GetAuthorById(int id)
        {
            try
            {
                Author author = _context.Authors.FirstOrDefault(x => x.AuthorId == id);
                if (author == null)
                {
                    throw new Exception("Not found author!");
                }
                var authorDto = _mapper.Map<AuthorDto>(author);
                return authorDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AuthorDto> GetAuthorsWithActiveIsTrue()
        {
            try
            {
                List<Author> authors = _context.Authors.Where(x => x.Active == true).ToList();
                if (authors.Count <= 0)
                {
                    throw new Exception("List is empty!");
                }
                var authorDtos = _mapper.Map<List<AuthorDto>>(authors);
                return authorDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAuthor(AuthorDto authorDto)
        {
            try
            {
                if (!_context.Authors.Any(x => x.AuthorId == authorDto.AuthorId))
                {
                    throw new Exception("Author does not existed!");
                }
                Author author = _context.Authors.FirstOrDefault(x => x.AuthorId == authorDto.AuthorId);
                _mapper.Map(authorDto, author);

                _context.Authors.Update(author);

                if (!author.Active)
                {
                    List<Book> books = _context.Books.Where(x => x.AuthorId == author.AuthorId).ToList();
                    foreach (Book book in books)
                    {
                        book.Active = false;
                        _context.Books.Update(book);
                    }
                }
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
