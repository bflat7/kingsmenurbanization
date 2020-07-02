using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult<IEnumerable<UrbanizationByStateModel>> GetWithPagination(int page, int rowsPerPage, string orderBy, string order)
        {
            try
            {
                //var data = _UrbanizationByStateService.GetStateUrbanization().Skip(rowsPerPage * page).Take(rowsPerPage);
                var data = _UrbanizationByStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, order);
                return new JsonResult(data);
            } catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("Count")]
        public ActionResult<int> GetTotalRows()
        {
            try
            {
                var count = _UrbanizationByStateService.GetStateUrbanization().Count();
                return new JsonResult(count);
            } catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return new JsonResult(ex.Message);
            }
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UrbanizationByStateModel>>> GetWithPagination(int page, int rowsPerPage, string orderBy, string order)
        //{
        //    try
        //    {
        //        var data = (await _UrbanizationByStateService.GetStateUrbanization()).Skip(rowsPerPage * page).Take(rowsPerPage);
        //        var test = data.AsQueryable().OrderBy("Id");
        //        //var data = await _UrbanizationByStateService.GetStateUrbanizationSortedPaged(page, rowsPerPage, orderBy, order);
        //        //return new JsonResult(data);
        //        return Ok(new JsonResult(data));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        return new JsonResult(ex.Message);
        //    }
        //}

        //[HttpGet("Count")]
        //public async Task<ActionResult<int>> GetTotalRows()
        //{
        //    try
        //    {
        //        //var count = _UrbanizationByStateService.GetStateUrbanization().Count();
        //        var count = await _UrbanizationByStateService.GetCountyUrbanizationCount();
        //        return Ok(new JsonResult(count));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.Message);
        //        return new JsonResult(ex.Message);
        //    }
        //}
    }
}
