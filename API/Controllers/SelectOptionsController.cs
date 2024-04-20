using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CSelectOptions;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SelectOptionsController :BaseApiController
    {
        private readonly IMediator _mediator;
      
        public SelectOptionsController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task <ActionResult<List<SelectOption>>>GetSOp()
        {
            return await Mediator.Send(new Application.CSelectOptions.List.Query());
        
        }
        [HttpGet("{id}")]
        public async Task <ActionResult<SelectOption>>GetSOp(Guid Id)
        {
             try
            {
               var selectOption =await Mediator.Send(new Detail.Query{Id = Id});
               if (selectOption == null)
            {
                return NotFound();
            }

            return selectOption;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data Select Option: " + ex.Message);
            }


            
        }
        [HttpPost]
        public async Task<IActionResult> CreateSOp(SelectOption selectOption)
        {
            try
            {
                return Ok(await _mediator.Send(new Create.Command { SelectOption = selectOption }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam membuat Select Option: " + ex.Message);
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditSOp(Guid id,SelectOption selectOption)
        {
            try
            {
                selectOption.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Edit.Command { SelectOption = selectOption }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit select option: " + ex.Message);
            }
        }
           [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSOp(Guid Id)
        {
            try
            {
              
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new Delete.Command { Id = Id }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam delete Select Option: " + ex.Message);
            }
        }
 
    }
}