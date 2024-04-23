using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTrackCheckings;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    public class DataTrackChecksController :BaseApiController
    {
       private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataTrackChecksController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<List<DataTrackChecking>>> GetDataTracksChecking()
        {
            return await Mediator.Send(new  Application.DataTrackCheckings.Linst.DtQuery());

        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<DataTrackChecking>> GetDataTrackChecking(Guid id)
        {

            try
            {
                var dataTrackChecking = await Mediator.Send(new Application.DataTrackCheckings.Details.Query { Id = id });
                if (dataTrackChecking == null)
                {
                    return NotFound();
                }

                return dataTrackChecking;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data Tracking: " + ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateDataTrackChecking(DataTrackChecking datTrackChecking)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { DataTrackChecking = datTrackChecking }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam menyimpan: " + ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDataTrackChecking(Guid id, DataTrackChecking dataTrackChecking)
        {
            try
            {
                dataTrackChecking.Id = id;
                return Ok(await _mediator.Send(new Application.DataTrackCheckings.Edit.Command{DataTrackChecking = dataTrackChecking }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit Data Track: " + ex.Message);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteDataTrackChecking(Guid Id)
        {
            try
            {

                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Application.DataTrackCheckings.Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete user: " + ex.Message);
            }
        } 
    }
}