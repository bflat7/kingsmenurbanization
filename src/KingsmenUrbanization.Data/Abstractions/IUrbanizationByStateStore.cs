using System;
using System.Collections.Generic;
using System.Text;
using Urbanization.Data.Models;

namespace Urbanization.Data.Abstractions
{
    public interface IUrbanizationByStateStore
    {
        public IEnumerable<UrbanizationByState> GetUrbanizationByStates();
    }
}
