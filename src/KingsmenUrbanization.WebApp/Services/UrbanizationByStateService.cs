using KingsmenUrbanization.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;
using Urbanization.WebApp.Abstractions;

namespace Urbanization.WebApp.Services
{
    public class UrbanizationByStateService : IUrbanizationByStateService
    {
        private IUrbanizationByStateStore _UrbanizationByStateStore;
        public UrbanizationByStateService(IUrbanizationByStateStore urbanizationByStateStore)
        {
            _UrbanizationByStateStore = urbanizationByStateStore;
        }

        public async Task<int> GetCountyUrbanizationCount()
        {
            return (await _UrbanizationByStateStore.GetUrbanizationByStates()).Count();
        }

        public async Task<IEnumerable<UrbanizationByStateModel>> GetStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order)
        {
            if (!string.IsNullOrEmpty(orderBy) && order.ToLowerInvariant() != "desc" && order.ToLowerInvariant() != "asc")
                throw new ArgumentException();

            if (page < 0 || rowsPerPage < 0)
            {
                throw new ArgumentException();
            }

            IEnumerable<UrbanizationByStateModel> data;
            try
            {
                data = (await _UrbanizationByStateStore.GetUrbanizationByStates()).Select(u => new UrbanizationByStateModel
                {
                    Id = u.Id,
                    StateName = u.StateName,
                    StateFips = u.StateFips,
                    LatLong = $"({u.Latitude}, {u.Longditude})",
                    GisJoin = u.GISJoin,
                    Population = u.Population,
                    UrbanIndex = u.UrbanIndex,
                });

                if (!string.IsNullOrEmpty(orderBy))
                {
                    string formatedOrderBy = char.ToUpper(orderBy[0]) + orderBy.Substring(1);
                    data = data.AsQueryable().Sort($"{formatedOrderBy} {order}");
                }
            }
            catch (System.Linq.Dynamic.Core.Exceptions.ParseException ex)
            {
                if (ex.Message.ToLowerInvariant().Contains("no property or field"))
                    throw new ArgumentException(ex.Message);
                throw ex;
            }

            return data.Skip(page * rowsPerPage).Take(rowsPerPage);
        }
    }
}
