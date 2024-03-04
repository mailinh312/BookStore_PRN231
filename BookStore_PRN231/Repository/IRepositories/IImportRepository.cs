using BusinessObjects.DTO;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IImportRepository
    {
        List<ImportDto> GetAllImports();
        int AddNewImport(ImportCreateDto dto);

        ImportDto GetImportsById(int id);
    }
}
