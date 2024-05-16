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
using static Application.ParameterChecks.List;

namespace API.Controllers
{
    public class ParamChecksController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ParamChecksController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ParameterCheckDTO>>> GetDataPC(
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 999999,
           [FromQuery] string SearchQuery = "",
            [FromQuery] string Category = "All"
           )
        {
            var query = new List2.Query
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
        [HttpGet("ByReference/{id}")]
        public async Task<ActionResult<ParameterCheck>> GetPCByRef(Guid id) // Mengubah tipe balikan dari ActionResult<ParameterCheck> menjadi ActionResult<List<ParameterCheck>>
        {
            try
            {
                try
                {
                    var parameterCheck = await Mediator.Send(new Application.ParameterChecks.DetailByRefrence.Query { Id = id });
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

        //[HttpPut("{Id}")]
        //public async Task<IActionResult> EditPC(Application.ParameterChecks.Edit.Command command)
        //{
        //    try
        //    {
        //        return Ok(await _mediator.Send(command));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Tangkap kesalahan dan kirim respons error ke client
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam Edit Parameter: " + ex.Message);
        //    }
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPC(Guid id, ParameterCheck parameterChecks)
        {
            try
            {
                parameterChecks.Id = id;
        
                return Ok(await _mediator.Send(new Application.ParameterChecks.Edit.Command { ParamID = id, ParameterChecks = parameterChecks }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit Data Track: " + ex.Message);
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