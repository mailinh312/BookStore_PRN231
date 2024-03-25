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
    public class ImportRepository : IImportRepository
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public ImportRepository(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int AddNewImport(ImportCreateDto dto)
        {
            try
            {
                Import import = _mapper.Map<Import>(dto);
                import.ImportDate = DateTime.Now;
                _context.Imports.Add(import);
                _context.SaveChanges();
                return import.ImportId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ImportDto> GetAllImports()
        {
            try
            {
                var imports = _context.Imports.Include(x => x.User).OrderByDescending(x => x.ImportDate).ToList();
                if (!imports.Any())
                {
                    imports = new List<Import>();
                }
                var importsDto = _mapper.Map<List<ImportDto>>(imports);
                return importsDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ImportDto GetImportsById(int id)
        {
            try
            {
                var import = _context.Imports.Include(x => x.User).FirstOrDefault(x => x.ImportId == id);
                if (import == null)
                {
                    throw new Exception("Not found");
                }
                var importDto = _mapper.Map<ImportDto>(import);
                return importDto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
