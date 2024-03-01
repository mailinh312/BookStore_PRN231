using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class BookDAO
    {
        public static List<Book> GetAllBooks()
        {
            try
            {
                using (var context = new BookStoreDbContext())
                {
                    var books = context.Books.ToList();
                    if (books.Count() <= 0)
                    {
                        throw new Exception("List is empty!");
                    }
                    return books;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
