using BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IImportDetailRepository
    {
        List<ImportDetailDto> GetImportDetailsByImportId(int id);
        void AddNewImportDetail(int importId, ImportDetailCreateDto importDetail);
    }
}
