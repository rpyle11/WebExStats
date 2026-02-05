using Microsoft.AspNetCore.Mvc;
using Stats.Api.Services;
using Stats.Models;
using System.ComponentModel.DataAnnotations;

namespace Stats.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataUiController : ControllerBase
    {
        private readonly IDataService _dataService;
        public DataUiController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("startenddates", Name = "GetStartEndDates")]
        public async Task<IActionResult> GetStartEndDates([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser)
        {
            var data = await _dataService.GetStartEndDates(appUser);

            return data == null ? StatusCode(500, "Internal Error") : Ok(data);
        }

       

        [HttpPost("EmployeesInAllMeetingsByDate", Name = "GetAllEmployeesInAllMeetingsByDate")]
        public async Task<IActionResult> GetAllEmployeesInAllMeetingsByDate([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var data = await _dataService.GetAllEmployeesInAllMeetings(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }


        [HttpPost("TotalEmployeeTimeAllMeetingsByDate", Name = "GetTotalEmployeesTimeAllMeetingsByDate")]
        public async Task<IActionResult> GetTotalEmployeesTimeAllMeetingsByDate([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingParameters parameters)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetEmployeeTotalTimeInAllMeetings(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }


       

        [HttpPost("ByEmployeeTotalTimeByValues", Name = "GetByEmployeeTotalTimeInMeetingsByValues")]
        public async Task<IActionResult> GetByEmployeeTotalTimeInMeetingsByDate([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetByEmployeeMeetingsTotalTime(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("TopUsers", Name = "GetTopUsersData")]
        public async Task<IActionResult> GetTopUsersData([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetTopUsersData(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("TopFiServUsers", Name = "GetTopFiServUsersData")]
        public async Task<IActionResult> GetTopFiServUsersData([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingParametersFilter parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetTopUsersDataFiltered(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("MonthlyTotals", Name = "GetMonthlyTotals")]
        public async Task<IActionResult> GetMonthlyTotals([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] MonthlyTotalsParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetMonthlyTotals(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpGet("meetingpaticipants/{meetingid}")]
        public async Task<IActionResult> GetMeetingParticipants([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromRoute] MeetingEmployeesParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var data = await _dataService.GetMeetingParticipants(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("EmployeeMeetings", Name = "GetEmployeeMeetings")]
        public async Task<IActionResult> GetEmployeeMeetings([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingsListParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var data = await _dataService.GetEmployeeMeetings(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("EmployeeMeetingsCompare", Name = "GetEmployeeTimeComparison")]
        public async Task<IActionResult> GetEmployeeTimeComparison([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeMeetingsListParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var data = await _dataService.EmployeeMeetingTimeCompare(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

        [HttpPost("EmployeeTotalTime", Name = "GetEmployeeTotalTime")]
        public async Task<IActionResult> GetEmployeeTotalTime([FromHeader(Name = "appuser")] [Required(ErrorMessage = "appuser is required")] [MaxLength(20, ErrorMessage = "appuser maximum length is 20 characters")]
            string appUser, [FromBody] EmployeeTotalTimeParameters parameters)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var data = await _dataService.GetEmployeeMeetingTotalTime(parameters, appUser);

            return data != null ? Ok(data) : StatusCode(500, "Internal Error");
        }

       
    }
}
