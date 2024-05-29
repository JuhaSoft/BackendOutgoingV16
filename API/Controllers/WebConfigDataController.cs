using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WebConfigDataController  :BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebConfigDataController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
         [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task <ActionResult<WebConfigData>>GetWebConfigData(Guid Id)
        {
             try
            {
               var webconfigData =await Mediator.Send(new Application.WebConfigDatas.Detail.Query{Id = Id});
               if (webconfigData == null)
            {
                return NotFound();
            }

            return webconfigData;
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Mengambil data Config: " + ex.Message);
            }


            
        }
        [AllowAnonymous]
        [HttpPut("{Id}")]
        public async Task<IActionResult> EditWebConfig(Guid id,WebConfigData webConfigData)
        {
            try
            {
                webConfigData.Id=id;
                // Mengirim permintaan ke mediator untuk membuat user
                return Ok(await _mediator.Send(new  Application.WebConfigDatas.Edit.Command { WebConfigData = webConfigData }));
            }
            catch (Exception ex)
            {
                // Tangkap kesalahan dan kirim respons error ke client
                return StatusCode(StatusCodes.Status500InternalServerError, "Terjadi kesalahan dalam edit Config: " + ex.Message);
            }
        }
    }
}