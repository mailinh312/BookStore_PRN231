using BusinessObjects.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public static class Paging
    {
        public static PagingResponseInfo GetPagingResponse(PagingRequestBase request, int totalRecord)
        {
            return new PagingResponseInfo
            {
                CurrentPage = request.Page,
                ItemsPerPage = request.ItemsPerPage,
                ToTalPage = (totalRecord / request.ItemsPerPage) + ((totalRecord % request.ItemsPerPage) == 0 ? 0 : 1),
                ToTalRecord = totalRecord
            };
        }
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> pagingList, PagingRequestBase paging) => pagingList.Skip(paging.Skip).Take(paging.Take);
    }
}
