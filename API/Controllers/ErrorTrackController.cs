using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataLines;
using Application.ErrorTracks;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorTrackController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorTrackController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this._mediator = mediator;
            this._httpContextAccessor = httpContextAccessor;
        }
       [AllowAnonymous]
        [HttpGet("Chart")]
        public async Task<ActionResult<List<ErrorTrackChartDTO>>> GetErrorTrack(
   
            [FromQuery] string Start = "",
            [FromQuery] string EndDate = ""
            )
        {
            var query = new ListChart.DtQuery
            {
              
                Start = Start,
                EndDate = EndDate
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        } 
    }
}