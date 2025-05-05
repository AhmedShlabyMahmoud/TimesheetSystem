namespace TimesheetSystem.UI.Models
{
    public class TimesheetPageViewModel
    {
        public CreateTimeLogViewModel NewLog { get; set; } = new CreateTimeLogViewModel();
        public List<TimeLogViewModel> TimeLogs { get; set; } = new List<TimeLogViewModel>();
    }

}
