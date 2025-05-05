using System.ComponentModel.DataAnnotations;

namespace TimesheetSystem.UI.Models
{
    public class CreateTimeLogViewModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        [Required]
        public DateTime LogoutTime { get; set; }

        public List<TimeLogViewModel>? TimeLogs { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalPages { get; set; }
    }

    public class TimeLogViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; } 
        public DateTime Date { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
    }


}
