using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.Appliication.IServices;

namespace TimesheetSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTimeLogsByUserId([FromQuery] GetTimeLogFilter getTimeLogFilter)
        {
            try
            {
                var logs = await _timesheetService.GetAllTimeLogsAsync(getTimeLogFilter.UserId, getTimeLogFilter.pageSize, getTimeLogFilter.pageNumber);

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateTimeLogDto dto)
        {
            try
            {
                var id = await _timesheetService.CreateTimeLogAsync(dto);

                return Ok(new { TimeLogId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

