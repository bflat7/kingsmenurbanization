using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Urbanization.Data.Models;

namespace Urbanization.Data.Abstractions
{
    public interface IUrbanizationByStateStore
    {
        public Task<IEnumerable<UrbanizationByState>> GetUrbanizationByStates();
    }
}
