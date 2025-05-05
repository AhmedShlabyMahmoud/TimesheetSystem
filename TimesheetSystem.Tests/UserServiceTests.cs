using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TimesheetSystem.Appliication.Services;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Appliication.Dtos;

public class UserServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            contextAccessor.Object,
            userPrincipalFactory.Object,
            null, null, null, null);

        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["JWT:Secret"]).Returns("YourSuperSecretKeyForTesting123456789");
        _configurationMock.Setup(c => c["JWT:ValidIssuer"]).Returns("TestIssuer");
        _configurationMock.Setup(c => c["JWT:ValidAudience"]).Returns("TestAudience");

        _userService = new UserService(_userManagerMock.Object, _configurationMock.Object, _signInManagerMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnError_WhenEmailAlreadyExists()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Ahmed",
            Email = "ahmed@example.com",
            Password = "Password123",
            PhoneNumber = "1234567890"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(userDto.Email))
                  .ReturnsAsync(new ApplicationUser(userDto.Name, userDto.Email, userDto.Password, userDto.PhoneNumber));


        // Act
        var result = await _userService.Register(userDto);

        // Assert
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Email is already registered!", result.Message);
    }

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var userDto = new CreateUserDto
        {
            Name = "Ahmed",
            Email = "ahmed@example.com",
            Password = "Password123",
            PhoneNumber = "1234567890"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(userDto.Email))
                        .ReturnsAsync((ApplicationUser)null);

        _userManagerMock.Setup(x => x.FindByNameAsync(userDto.Name))
                        .ReturnsAsync((ApplicationUser)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), userDto.Password))
                        .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.Register(userDto);

        // Assert
        Assert.True(result.IsAuthenticated);
        Assert.Equal("Registerd Succesfully", result.Message);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal(userDto.Name, result.Name);
    }
}
