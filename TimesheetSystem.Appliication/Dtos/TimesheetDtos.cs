using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetSystem.Appliication.Dtos
{
    public class TimeLogDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public string UserName { get; set; }

    }

    public class CreateTimeLogDto
    {

        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }
    public class GetTimeLogFilter
    {
        public Guid UserId { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }
}
