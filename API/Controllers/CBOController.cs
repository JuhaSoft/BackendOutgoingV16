using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.SComboBoxOptions;

using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CBOController :BaseApiController
    {
        private readonly IMediator _mediator;
      
        public CBOController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task <ActionResult<List<SComboBoxOption>>>GetCT()
        {
            return await Mediator.Send(new Application.SComboBoxOptions.List.CBOQuery());
        
        }
        [HttpGet("{id}")]
        public async Task <ActionResult<SComboBoxOption>>GetCBO(Guid Id)
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data combo box: " + ex.Message);
            }


            
        }
        [HttpPost]
        public async Task<IActionResult> CreateCBO(SComboBoxOption comboBoxOption)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { sComboBoxOption = comboBoxOption }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat combo box: " + ex.Message);
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditCT(Guid id,SComboBoxOption comboBoxOption)
        {
            try
            {
                comboBoxOption.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Edit.Command { SComboBoxOption = comboBoxOption }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit combo box: " + ex.Message);
            }
        }
           [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCBO(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete combo box: " + ex.Message);
            }
        }
        
    }
}