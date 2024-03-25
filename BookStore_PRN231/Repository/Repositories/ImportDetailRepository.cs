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
    public class ImportDetailRepository : IImportDetailRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public ImportDetailRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddNewImportDetail(int importId, ImportDetailCreateDto importDetailDto)
        {
            try
            {
                Import import = _context.Imports.Find(importId);
                if (import == null)
                {
                    throw new Exception("Import does not exist!");
                }
                
                ImportDetail imporDetail = _mapper.Map<ImportDetail>(importDetailDto);
                imporDetail.ImportId = importId;
                imporDetail.TotalPrice = importDetailDto.Quantity * importDetailDto.InputPrice;

                _context.ImportDetails.Add(imporDetail);

                Book book = _context.Books.Find(importDetailDto.BookId);
                book.OriginPrice = importDetailDto.InputPrice;
                book.Price = importDetailDto.OutputPrice;
                book.StockQuantity += importDetailDto.Quantity;
                _context.Books.Update(book);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ImportDetailDto> GetImportDetailsByImportId(int id)
        {
            try
            {
                var importDetails = _context.ImportDetails.Include(x => x.Book).Where(x => x.ImportId == id).ToList();
                if (!importDetails.Any())
                {
                    throw new Exception("List is empty");
                }
                var importDetailsDto = _mapper.Map<List<ImportDetailDto>>(importDetails);
                return importDetailsDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
