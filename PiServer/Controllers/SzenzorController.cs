using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using PiServer.DataManagers;
using PiServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PiServer.Controllers
{
    [Route("api/szenzor")]
    [ApiController]
    public class SzenzorController : ControllerBase
    {
        private readonly ISzenzorManager _szenzorManager;

        public SzenzorController(ISzenzorManager szenzorManager)
        {
            _szenzorManager = szenzorManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody] SzenzorPostLoginDTO szenzorDTO)
        {
            if (szenzorDTO == null)
            {
                return BadRequest("Szenzor is null.");
            }

            if (_szenzorManager.Login(szenzorDTO.Id, szenzorDTO.Tipus, Request.HttpContext.Connection.RemoteIpAddress))
            {
                return Ok();
            }
            else
            {
                return StatusCode(503);
            }
        }

        [HttpPost("meres")]
        public async Task<IActionResult> PostMeresData([FromBody] SzenzorPostMertAdatDTO meresDTO)
        {
            if (meresDTO == null)
            {
                return BadRequest("Meres is null.");
            }

            if (_szenzorManager.PostMeresData(meresDTO.Id, meresDTO.MertAdat))
            {
                return Ok();
            }
            else
            {
                return StatusCode(503);
            }
        }
    }
}
