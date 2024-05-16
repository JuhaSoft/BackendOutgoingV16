using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.DataTracks;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Common.DTOs;
using Common.DTOs.DataTrack;
using System.Security.Claims;

namespace API.Controllers
{
    public class DataTracksController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataTracksController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<DataTrackDTO>>> GetDataTracks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 999999,
            [FromQuery] string SearchQuery = "",
            [FromQuery] string Category = "All"
            )
        {
            var query = new List.DtQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = SearchQuery,
                Category = Category
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("order/{order}")]
        public async Task<ActionResult<List<DataTrackDTO>>> GetDataTracks(
                string order, // Parameter order yang ditentukan dalam jalur
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 999999,
                [FromQuery] string SearchQuery = "",
                [FromQuery] string Category = "All"
            )
        {
            var query = new ListByOrder.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = SearchQuery,
                Category = Category,
                Order = order // Set nilai order ke dalam objek query
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult<DetailDataTrackDto>> GetDataTrack(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                {
                    return BadRequest("Invalid ID format");
                }

                var dataTrackDto = await Mediator.Send(new Details.Query { Id = guidId });
                if (dataTrackDto == null)
                {
                    return NotFound();
                }

                return dataTrackDto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data Tracking: " + ex.Message);
            }
        }
        //[Authorize]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateDataTrack(DataTrack dataTrack)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                dataTrack.TrackingDateCreate = DateTime.Now;
                dataTrack.TrackingUserIdChecked = userId;
                
                string guidStringWithoutBraces = dataTrack.TrackingLastStationId.ToString().Replace("{", "").Replace("}", "");


                return Ok(await _mediator.Send(new Create.Command { Request = dataTrack }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam menyimpan: " + ex.Message);
            }
        }
        [AuthorizeRoles("Admin,Staf")]

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDataTrack(Guid id, DataTrack dataTrack)
        {
            try
            {
                dataTrack.Id = id;
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                dataTrack.TrackingUserIdChecked = userId;
                return Ok(await _mediator.Send(new Application.DataTracks.Edit.Command { DataTrack = dataTrack }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit Data Track: " + ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteDataTrack(Guid Id)
        {
            try
            {

                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Application.DataTracks.Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete user: " + ex.Message);
            }
        }
    }
}