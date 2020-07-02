﻿using System;
using System.Collections.Generic;
using System.Text;
using Urbanization.Data.Models;

namespace Urbanization.WebApp.Abstractions
{
    public interface IUrbanizationByStateService
    {
        public IEnumerable<UrbanizationByStateModel> GetStateUrbanization();
        public IEnumerable<UrbanizationByStateModel> GetStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order);
        //etStateUrbanizationSortedPaged(int page, int rowsPerPage, string orderBy, string order);
        //public Task<IEnumerable<UrbanizationByStateModel>> GetStateUrbanization();
        //public Task<int> GetCountyUrbanizationCount();
    }
}
