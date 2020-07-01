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
                GISJoin = u.GISJoin,
                Population = u.Population,
                UrbanIndex = u.UrbanIndex,
            });
        }
    }
}
