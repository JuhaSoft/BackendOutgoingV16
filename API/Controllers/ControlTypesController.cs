using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataContrplTypes;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ControlTypesController : BaseApiController

    {
        private readonly IMediator _mediator;
      
        public ControlTypesController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task <ActionResult<List<DataContrplType>>>GetCT()
        {
            return await Mediator.Send(new Application.DataContrplTypes.List.PCQuery());
        
        }
        [HttpGet("{id}")]
        public async Task <ActionResult<DataContrplType>>GetPC(Guid Id)
        {
             try
            {
               var dataControlType =await Mediator.Send(new Detail.Query{Id = Id});
               if (dataControlType == null)
            {
                return NotFound();
            }

            return dataControlType;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data control type: " + ex.Message);
            }


            
        }
        [HttpPost]
        public async Task<IActionResult> CreateCT(DataContrplType dataContrplType)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { DataContrplType = dataContrplType }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat control type: " + ex.Message);
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditCT(Guid id,DataContrplType dataContrplType)
        {
            try
            {
                dataContrplType.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Edit.Command { DataContrplType = dataContrplType }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit control type: " + ex.Message);
            }
        }
           [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCT(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete control type: " + ex.Message);
            }
        }
        
    }
}