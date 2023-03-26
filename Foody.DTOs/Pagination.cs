using Foody.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foody.DTOs
{
    public static class Pagination
    {
        public static PagedResponse<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            //Create and Initialize new Paged result
            var pagedResult = new PagedResponse<T>();
            pagedResult.CurrentPage = pageNumber;
            pagedResult.PageSize = pageSize;
            pagedResult.TotalRecords = query.Count();

            //this is how many records each page will contain
            var pageCount = (double)pagedResult.TotalRecords / pageSize;
            pagedResult.TotalPages = (int)Math.Ceiling(pageCount);

            //Caltulate number of items to skip
            var pagesToSkip = (pageNumber - 1) * pageSize;

            //Get and return the paged result from the records
            pagedResult.Result = query.Skip(pagesToSkip).Take(pageSize).ToList();
            return pagedResult;
        }
    }
}
