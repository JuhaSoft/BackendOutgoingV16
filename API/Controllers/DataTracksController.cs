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

namespace API.Controllers
{
    public class DataTracksController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DataTracksController(IMediator mediator)
        {
            this._mediator = mediator;
            
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
            var query = new List.DtQuery { PageNumber = pageNumber, PageSize = pageSize,
                SearchQuery = SearchQuery,
                Category = Category
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{TrackPSN}")]
        public async Task<ActionResult<DetailDataTrackDto>> GetDataTrack(string TrackPSN)
        {
            try
            {
                var dataTrackDto = await Mediator.Send(new Details.Query { TrackPSN = TrackPSN });
                if (dataTrackDto == null)
                {
                    return NotFound();
                }

                // Buat JsonSerializerOptions dan atur ReferenceHandler.Preserve
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Serialize objek DataTrack ke JSON dengan menggunakan JsonSerializer
                var jsonData = JsonSerializer.Serialize(dataTrackDto, options);

                // Deserialisasi JSON kembali menjadi objek DataTrack
                var deserializedDataTrackDto = JsonSerializer.Deserialize<DetailDataTrackDto>(jsonData, options);

                // Kembalikan objek DataTrack yang telah diserialisasi ulang sebagai respons
                return deserializedDataTrackDto;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data Tracking: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateDataTrack(DataTrack dataTrack)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { DataTrack = dataTrack }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam menyimpan: " + ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditDataTrack(Guid id,DataTrack dataTrack)
        {
            try
            {
                dataTrack.Id=id;
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