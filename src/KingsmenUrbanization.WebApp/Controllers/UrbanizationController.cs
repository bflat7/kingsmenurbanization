using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrbanizationByStateModel>>> GetWithPagination(int page, int rowsPerPage, string orderBy, string order)
        {
            try
            {
                var data = (await _UrbanizationByStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, order));
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            } 
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("Count")]
        public async Task<ActionResult<int>> GetTotalRows()
        {
            try
            {
                var count = await _UrbanizationByStateService.GetCountyUrbanizationCount();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
