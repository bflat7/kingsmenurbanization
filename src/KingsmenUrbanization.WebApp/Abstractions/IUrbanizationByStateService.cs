using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Urbanization.Data.Models;

namespace Urbanization.WebApp.Abstractions
{
    public interface IUrbanizationByStateService
    {
        public Task<IEnumerable<UrbanizationByStateModel>> GetStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order);
        public Task<int> GetCountyUrbanizationCount();
    }
}
