using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTrackCheckings;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.ParameterChecks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace API.Controllers
{
    public class ParamChecksController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ParamChecksController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<DataReferenceDTO>>> GetParameters(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 999999,
    [FromQuery] string searchQuery = "",
    [FromQuery] string category = "All"
)
        {
            var query = new Application.ParameterChecks.List.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchQuery = searchQuery,
                Category = category
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<ParameterCheck>>GetPC(Guid Id)
        {
             try
            {
               var parameterCheck =await Mediator.Send(new Application.ParameterChecks.Details.Query{Id = Id});
               if (parameterCheck == null)
            {
                return NotFound();
            }

            return parameterCheck;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data parameter: " + ex.Message);
            }


            
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreatePC(Application.ParameterChecks.Create.Command command)
        {
            try
            {
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat Parameter: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpPut("{Id}")]
        public async Task<IActionResult> EditPC(Application.ParameterChecks.Edit.Command command)
        {
            try
            {
                return Ok(await _mediator.Send(command));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam Edit Parameter: " + ex.Message);
            }
        }
        [AllowAnonymous]

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePC(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.ParameterChecks.Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete Parameter: " + ex.Message);
            }
        }
    }
}