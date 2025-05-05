using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.Appliication.IServices;
using TimesheetSystem.Domain.Entities;
namespace TimesheetSystem.Appliication.Services
{
    public class UserService : IUserService
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        private static readonly ILog _logger = LogManager.GetLogger(typeof(UserService));
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration configuration;


        public UserService(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
        }



        public async Task<RegisterationDto> Register(CreateUserDto model)
        {
            _logger.Info("Register called");
            _logger.Debug($"Register parameters: Name={model.Name}, Email={model.Email}, Phone={model.PhoneNumber}");


            try
            {
                RegisterationDto registerationDto = new RegisterationDto();

                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    return new RegisterationDto { Message = "Email is already registered!" };

                if (await _userManager.FindByNameAsync(model.Name) is not null)
                    return new RegisterationDto { Message = "Name is already registered!" };
                var user = new ApplicationUser(model.Name, model.Email, model.Password, model.PhoneNumber);

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return new RegisterationDto { Message = errors };
                }

                var jwtSecurityToken = GenerateToken(user);

                return new RegisterationDto
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.Name,
                    Name = user.Name,
                    Message = "Registerd Succesfully"

                };

            }
            catch (Exception ex)
            {
                _logger.Error("Error in Register", ex);
                throw;
            }
        }


        private JwtSecurityToken GenerateToken(ApplicationUser User)
        {
            try
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, User.Name),
                    new Claim(ClaimTypes.Email, User.Email),
                    new Claim("uid", User.Id.ToString())
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return token;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<RegisterationDto> LoginAsync(LoginDto model)
        {
            var authModel = new RegisterationDto();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var jwtSecurityToken = GenerateToken(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Name = user.Name;
            authModel.Username = user.Name;
            authModel.Message = "Login Succesfully";
            authModel.UserId = user.Id;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;


            return authModel;
        }



        public async Task SignOut()
        {
            try
            {
                _logger.Info("SignOut called");
                await _signInManager.SignOutAsync();

            }
            catch (Exception ex)
            {
                _logger.Error("Error in SignOut", ex);
                throw;
            }
        }





    }
}

