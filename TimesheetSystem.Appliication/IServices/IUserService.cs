using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetSystem.Appliication.Dtos;

namespace TimesheetSystem.Appliication.IServices
{
    public interface IUserService
    {

        Task<RegisterationDto> Register(CreateUserDto dto);
        Task SignOut();
        Task<RegisterationDto> LoginAsync(LoginDto model);

    }
}
