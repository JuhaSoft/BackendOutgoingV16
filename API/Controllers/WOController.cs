using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.WorkOrders;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.DTOs;
using static Application.LastStationIDs.List;
using System.Security.Claims;

namespace API.Controllers
{
    public class WOController :BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WOController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]

        [HttpGet]
      
        public async Task<ActionResult<List<WorkOrderDto>>> GetDataWo(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 999999,
            [FromQuery] string SearchQuery = "",
    [FromQuery] string Category = "All"
            )
        {
            var query = new List.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = SearchQuery,
                Category = Category
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
         // Endpoint untuk pencarian data Work Order
         [AllowAnonymous]

        [HttpGet("search")]
        public async Task<ActionResult<ListResult<WorkOrderDto>>> SearchWorkOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 999999, [FromQuery] string searchTerm = "")
        {
            var query = new Search.Query { PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
// [Authorize]
        [HttpGet("{id}")]
        public async Task <ActionResult<WorkOrderDto>>GetWO(Guid Id)
        {
             try
            {
               var workOrderDto  =await Mediator.Send(new Details.Query{Id = Id});
               if (workOrderDto  == null)
            {
                return NotFound();
            }

            return workOrderDto ;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data WO Detail: " + ex.Message);
            }


            
        }
        [Authorize]


        [HttpPost]
        public async Task<IActionResult> CreateWO(WorkOrder workorder)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                workorder.WoCreate = DateTime.Now;
                workorder.UserIdCreate = userId;
                return Ok(await _mediator.Send(new Create.Command { WorkOrders = workorder }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat work order: " + ex.Message);
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditCT(Guid id,WorkOrder workOrder)
        {
            try
            {
                workOrder.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Edit.Command { WorkOrder = workOrder }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit WO: " + ex.Message);
            }
        }
           [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteWO(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete WO: " + ex.Message);
            }
        }
    }
}