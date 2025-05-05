using log4net;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.Appliication.IServices;
using TimesheetSystem.Appliication.UnitOfWork;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.IRepository;

namespace TimesheetSystem.Appliication.Services
{
    public class TimesheetService: ITimesheetService
    {
        private readonly IRepository<Timesheet> _timesheetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(TimesheetService));
        private readonly UserManager<ApplicationUser> _userManager;


        public TimesheetService(IRepository<Timesheet> timesheetRepository, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _timesheetRepository = timesheetRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Guid> CreateTimeLogAsync(CreateTimeLogDto dto)
        {
            _logger.Info("CreateTimeLogAsync called");
            _logger.Debug($"Register parameters: Date={dto.Date}, login={dto.LoginTime}, logout={dto.LogoutTime}");
            try
            {
                var timeLog = new Timesheet(dto.UserId, dto.Date, dto.LoginTime, dto.LogoutTime);
                await _timesheetRepository.AddAsync(timeLog);
                await _unitOfWork.SaveChangesAsync();
                return timeLog.Id;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in CreateTimeLogAsync", ex);
                throw;
            }
        }


        public async Task<List<TimeLogDto>> GetAllTimeLogsAsync(Guid userId, int pageSize, int pageNumber)
        {
            _logger.Info("GetAllTimeLogsAsync called");
            _logger.Debug($"Register parameters: pageSize={pageSize}, pageNumber={pageNumber}");
            var user = await _userManager.FindByIdAsync(userId.ToString());

            try
            {
                var logs =await _timesheetRepository.filter(e => e.UserId == userId);

                logs = logs.Skip(pageSize * pageNumber).Take(pageSize).ToList();
       
                return logs.Select(l => new TimeLogDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    LogoutTime = l.LogoutTime,
                    LoginTime = l.LoginTime,
                    Date = l.Date,
                    UserName=  user?.UserName ?? "Unknown"

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Error in GetAllTimeLogsAsync", ex);
                throw;
            }
        }


    }
}
