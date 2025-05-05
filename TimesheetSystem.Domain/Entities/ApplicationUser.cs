using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; private set; }
        public string Password { get; private set; }


        public ICollection<Timesheet> Timesheets { get; private set; } = new List<Timesheet>();

        private ApplicationUser() { }

        public ApplicationUser(string name, string email, string password, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password is required");
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentException("Phone number is required");
            UserName = email;
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }
    }
}
