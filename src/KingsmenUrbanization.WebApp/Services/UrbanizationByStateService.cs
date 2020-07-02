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

        public IEnumerable<UrbanizationByStateModel> GetStateUrbanization()
        {
            return _UrbanizationByStateStore.GetUrbanizationByStates().Select(u => new UrbanizationByStateModel
            {   
                Id = u.Id,
                StateName = u.StateName,
                StateFips = u.StateFips,
                LatLong = $"({u.Latitude}, {u.Longditude})",
                GisJoin = u.GISJoin,
                Population = u.Population,
                UrbanIndex = u.UrbanIndex,
            });
        }

        public IEnumerable<UrbanizationByStateModel> GetStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order)
        {
            IEnumerable<UrbanizationByStateModel> data;
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(order))
            {
                string formatedOrderBy = char.ToUpper(orderBy[0]) + orderBy.Substring(1);
                data = _UrbanizationByStateStore.GetUrbanizationByStates().Select(u => new UrbanizationByStateModel
                {
                    Id = u.Id,
                    StateName = u.StateName,
                    StateFips = u.StateFips,
                    LatLong = $"({u.Latitude}, {u.Longditude})",
                    GisJoin = u.GISJoin,
                    Population = u.Population,
                    UrbanIndex = u.UrbanIndex,
                }).AsQueryable().Sort($"{formatedOrderBy} {order}").Skip(page * rowsPerPage).Take(rowsPerPage);
                var test = data.AsQueryable().Expression;
            }
            else
            {
                data = _UrbanizationByStateStore.GetUrbanizationByStates().Skip(page * rowsPerPage).Take(rowsPerPage).Select(u => new UrbanizationByStateModel
                {
                    Id = u.Id,
                    StateName = u.StateName,
                    StateFips = u.StateFips,
                    LatLong = $"({u.Latitude}, {u.Longditude})",
                    GisJoin = u.GISJoin,
                    Population = u.Population,
                    UrbanIndex = u.UrbanIndex,
                });
            }
            return data;
        }


        //public async Task<int> GetCountyUrbanizationCount()
        //{
        //    return (await _UrbanizationByStateStore.GetUrbanizationByStates()).Count();
        //}

        //public async Task<IEnumerable<UrbanizationByStateModel>> GetStateUrbanization()
        //{
        //    return (await _UrbanizationByStateStore.GetUrbanizationByStates()).Select(u => new UrbanizationByStateModel
        //    {
        //        Id = u.Id,
        //        StateName = u.StateName,
        //        StateFips = u.StateFips,
        //        LatLong = $"({u.Latitude}, {u.Longditude})",
        //        GISJoin = u.GISJoin,
        //        Population = u.Population,
        //        UrbanIndex = u.UrbanIndex,
        //    });
        //}

        //public async Task<IEnumerable<UrbanizationByStateModel>> GetStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order)
        //{
        //    var data = (await _UrbanizationByStateStore.GetUrbanizationByStates()).AsQueryable().Sort($"{orderBy} {order}").Skip(page * rowsPerPage).Take(rowsPerPage).Select(u => new UrbanizationByStateModel
        //    {
        //        Id = u.Id,
        //        StateName = u.StateName,
        //        StateFips = u.StateFips,
        //        LatLong = $"({u.Latitude}, {u.Longditude})",
        //        GISJoin = u.GISJoin,
        //        Population = u.Population,
        //        UrbanIndex = u.UrbanIndex,
        //    });
        //    return data;
        //}
    }
}
