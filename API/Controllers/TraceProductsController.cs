using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class TraceProductsController : ControllerBase
    {
        private readonly DataContext _dbContext;

        public TraceProductsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(string paramSn, string paramStationNumber)
        {
            if (string.IsNullOrEmpty(paramSn) || string.IsNullOrEmpty(paramStationNumber))
            {
                return BadRequest("Both paramSn and paramStationNumber are required.");
            }

            // Query untuk mencari semua data berdasarkan SerialNumber
            var traceProducts = await _dbContext.TraceProducts
                .Where(p => p.SerialNumber == paramSn)
                .ToListAsync();

            if (traceProducts.Count == 0)
            {
                return Ok(new { Status = "Fail", Description = "Data not found." });
            }

            // Memeriksa apakah ada StationName yang cocok dengan paramStationNumber
            var stationMatch = traceProducts.FirstOrDefault(p => p.StationName == paramStationNumber);

            if (stationMatch == null)
            {
                return Ok(new { Status = "Fail", Description = "Station not found for given SerialNumber." });
            }

            // Mengembalikan status dan deskripsi
            return Ok(new { Status = stationMatch.Status, Description = "" });
        }
    }
    }