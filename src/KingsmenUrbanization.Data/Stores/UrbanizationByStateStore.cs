using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Urbanization.Data.Abstractions;
using Urbanization.Data.Models;

namespace Urbanization.Data.Stores
{
    public class UrbanizationByStateStore : IUrbanizationByStateStore
    {
        private IUrbanizationDbContext _DbContext;
        //public UrbanizationByStateStore

        public UrbanizationByStateStore(IUrbanizationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        //public IEnumerable<UrbanizationByState> GetUrbanizationByStates()
        //{
        //    return _DbContext.UrbanizationByState;
        //}


        public Task<IEnumerable<UrbanizationByState>> GetUrbanizationByStates()
        {
            return Task.FromResult(_DbContext.UrbanizationByState as IEnumerable<UrbanizationByState>);
        }
    }
}
