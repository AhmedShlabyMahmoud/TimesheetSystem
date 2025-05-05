using Xunit;
using Moq;
using TimesheetSystem.Appliication.Services;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.IRepository;
using TimesheetSystem.Appliication.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimesheetSystem.Appliication.Dtos;

public class TimesheetServiceTests
{
    private readonly Mock<IRepository<Timesheet>> _timesheetRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly TimesheetService _timesheetService;

    public TimesheetServiceTests()
    {
        _timesheetRepoMock = new Mock<IRepository<Timesheet>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        _timesheetService = new TimesheetService(_timesheetRepoMock.Object, _unitOfWorkMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateTimeLogAsync_ReturnsGuid()
    {
        // Arrange
        var dto = new CreateTimeLogDto
        {
            UserId = Guid.NewGuid(),
            Date = DateTime.Today,
            LoginTime = DateTime.Today.AddHours(9),
            LogoutTime = DateTime.Today.AddHours(17)
        };

        _timesheetRepoMock.Setup(r => r.AddAsync(It.IsAny<Timesheet>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _timesheetService.CreateTimeLogAsync(dto);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
}
