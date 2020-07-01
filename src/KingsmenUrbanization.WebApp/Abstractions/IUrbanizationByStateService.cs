using System;
using System.Collections.Generic;
using System.Text;
using Urbanization.Data.Models;

namespace Urbanization.WebApp.Abstractions
{
    public interface IUrbanizationByStateService
    {
        public IEnumerable<UrbanizationByStateModel> GetStateUrbanization();
    }
}
