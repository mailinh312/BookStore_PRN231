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
        public void AddNewImportDetail(ImportDetailCreateDto importDetailDto)
        {
            try
            {
                ImportDetail imporDetail = _mapper.Map<ImportDetail>(importDetailDto);

                _context.ImportDetails.Add(imporDetail);
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
