using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataReferences;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DataReferenceController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataReferenceController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<DataReferenceDTO>>> GetDataRef(
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
        [AllowAnonymous]

        [HttpGet("{id}")]
        public async Task <ActionResult<DataReference>>GetReference(Guid Id)
        {
             try
            {
               var dataLine =await Mediator.Send(new Application.DataReferences.Detail.Query{Id = Id});
               if (dataLine == null)
            {
                return NotFound();
            }

            return dataLine;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil Detail Data Ref: " + ex.Message);
            }


            
        }
        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> CreateRef(DataReference dataReference)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { DataReference = dataReference }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat Ref: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpPut("{Id}")]
        public async Task<IActionResult> EditLine(Guid id,DataReference dataReference)
        {
            try
            {
                dataReference.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.DataReferences.Edit.Command { dataReference = dataReference }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit user: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePC(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.DataReferences.Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete Parameter: " + ex.Message);
            }
        }
    }
}