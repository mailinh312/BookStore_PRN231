using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IAuthorRepository
    {
        List<AuthorDto> GetAllAuthors();
        List<AuthorDto> GetAuthorsWithActiveIsTrue();
        AuthorDto GetAuthorById(int id);
        void AddNewAuthor(AuthorDto author);
        void UpdateAuthor(AuthorDto author);

    }
}
