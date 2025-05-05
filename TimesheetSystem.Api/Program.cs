using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimesheetSystem.Appliication.IServices;
using TimesheetSystem.Appliication.Services;
using TimesheetSystem.Appliication.UnitOfWork;
using TimesheetSystem.Domain.Entities;
using TimesheetSystem.Domain.IRepository;
using TimesheetSystem.Infrastructure.Context;
using TimesheetSystem.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITimesheetService, TimesheetService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseLazyLoadingProxies(false).UseSqlServer(builder.Configuration.GetConnectionString(name: "DefaultConnection")
               ));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})

 .AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuerSigningKey = true,
         ValidateIssuer = false,
         ValidateAudience = false,
         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"]))
     };

 });

try
{
    var log4netPath = Path.Combine(AppContext.BaseDirectory, "log4net.config");
    builder.Logging.ClearProviders();
    builder.Logging.AddLog4Net(log4netPath);
}
catch (Exception ex)
{
    Console.WriteLine("Error configuring log4net: " + ex.Message);
}


var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

var client = new HttpClient(handler);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
