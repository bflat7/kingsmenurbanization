using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Urbanization.Data.Models;
using Urbanization.WebApp.Abstractions;

namespace KingsmenUrbanization.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrbanizationController : ControllerBase
    {
        private readonly ILogger<UrbanizationController> _logger;
        private readonly IUrbanizationByStateService _UrbanizationByStateService;

        public UrbanizationController(IUrbanizationByStateService urbanizationByStateService, ILogger<UrbanizationController> logger)
        {
            _UrbanizationByStateService = urbanizationByStateService;
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<UrbanizationByStateModel> Get()
        //{
        //    return _UrbanizationByStateService.GetStateUrbanization();
        //}

        [HttpGet]
        public IEnumerable<UrbanizationByStateModel> GetWithPagination(int page, int rowsPerPage, int orderBy, int order)
        {
            return _UrbanizationByStateService.GetStateUrbanization().Skip(rowsPerPage*page).Take(rowsPerPage);
        }

        [HttpGet("Count")]
        public int GetTotalRows()
        {
            return _UrbanizationByStateService.GetStateUrbanization().Count();
        }
    }
}
