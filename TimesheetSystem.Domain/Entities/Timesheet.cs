using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetSystem.Domain.Entities
{
    public class Timesheet
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime LoginTime { get; private set; }
        public DateTime LogoutTime { get; private set; }

        public ApplicationUser User { get; private set; } = default!;

        private Timesheet() { }

        public Timesheet(Guid userId, DateTime date, DateTime loginTime, DateTime logoutTime)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required");

            if (date == default)
                throw new ArgumentException("Date is required");

            if (loginTime == default)
                throw new ArgumentException("Login time is required");

            if (logoutTime == default)
                throw new ArgumentException("Logout time is required");

            //if (loginTime >= logoutTime)
            //    throw new ArgumentException("Login time must be before logout time.");

            UserId = userId;
            Date = date;
            LoginTime = loginTime;
            LogoutTime = logoutTime;
        }
    }
}
