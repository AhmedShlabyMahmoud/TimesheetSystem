using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetSystem.Appliication.Dtos;

namespace TimesheetSystem.Appliication.IServices
{
    public interface ITimesheetService
    {
        Task<Guid> CreateTimeLogAsync(CreateTimeLogDto dto);
        Task<List<TimeLogDto>> GetAllTimeLogsAsync(Guid userId, int pageSize, int pageNumber);
    }
}
