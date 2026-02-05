using Microsoft.AspNetCore.Mvc;
using Stats.Api.Services;
using Stats.Models;
using System.ComponentModel.DataAnnotations;

namespace Stats.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PullerController : ControllerBase
    {
        private readonly IDataService _dataService;
        public PullerController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("meetings", Name = "GetMeetings")]
        public async Task<IActionResult> GetMeetings([FromHeader(Name = "appuser")][Required(ErrorMessage = "appuser is required")][MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")] string appUser, [FromBody] MeetingParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _dataService.PullMeetings(parameters, appUser))
            {
                return StatusCode(500, "Internal Error");
            }

            return Ok();

        }

        [HttpGet("participants", Name = "GetMeetingParticipants")]
        public async Task<IActionResult> GetMeetingParticipants([FromHeader(Name = "appuser")][Required(ErrorMessage = "appuser is required")][MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")] string appUser)
        {
            if (!await _dataService.PullMeetingParticipants(appUser))
            {
                return StatusCode(500, "Internal Error");
            }

            return Ok();

        }

        [HttpGet("datapull/{pulltype}", Name = "GetLastDataPull")]
        public async Task<IActionResult> GetLastDataPull([FromHeader(Name = "appuser")][Required(ErrorMessage = "appuser is required")][MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")] string appUser, [FromRoute] LastDataPullParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetLastDataPullByType(parameters, appUser);
            return data == null ? StatusCode(500, "Internal Error") : Ok(data);
        }

       



    }
}
