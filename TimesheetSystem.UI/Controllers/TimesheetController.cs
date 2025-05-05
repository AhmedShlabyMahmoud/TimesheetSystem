using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.UI.Models;

namespace TimesheetSystem.UI.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TimesheetController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            
            var viewModel = new TimesheetPageViewModel();

            var response = await _httpClient.GetAsync($"/api/Timesheet/all?userId={userId}&pageSize=10&pageNumber=0");
            if (response.IsSuccessStatusCode)
            {
                var dtoList = await response.Content.ReadFromJsonAsync<List<TimeLogDto>>();
                viewModel.TimeLogs = dtoList.Select(dto => new TimeLogViewModel
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    Date = dto.Date,
                    LoginTime = dto.LoginTime,
                    LogoutTime = dto.LogoutTime,
                    UserName = dto.UserName
                }).ToList();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int pageSize = 10, int pageNumber = 0)
        {
            var userId = GetCurrentUserId();
            if (userId==default)
            {
                return RedirectToAction("Login","User");

            }
            var viewModel = new CreateTimeLogViewModel();
            if (pageNumber < 0) { pageNumber = 0; }
            if (pageSize < 0) { pageNumber = 10; }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request?.Cookies["AccessToken"]);
            var response = await _httpClient.GetAsync($"/api/Timesheet/all?UserId={userId}&pageSize={pageSize}&pageNumber={pageNumber}");

            if (response.IsSuccessStatusCode)
            {
                var dtoList = await response.Content.ReadFromJsonAsync<List<TimeLogDto>>();
                viewModel.TimeLogs = dtoList.Select(dto => new TimeLogViewModel
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    Date = dto.Date,
                    LoginTime = dto.LoginTime,
                    LogoutTime = dto.LogoutTime,
                    UserName = dto.UserName
                }).ToList();
            }

            viewModel.PageNumber = pageNumber;
            viewModel.PageSize = pageSize;
            var timeLogs = viewModel.TimeLogs ?? new List<TimeLogViewModel>();
            viewModel.TotalPages = (int)Math.Ceiling((double)timeLogs.Count / pageSize);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["AccessToken"]);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTimeLogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TimeLogs = await LoadTimeLogs(); 
                return View(model);
            }

            var userId = GetCurrentUserId();

            var dto = new CreateTimeLogDto
            {
                UserId = userId,
                Date = model.Date,
                LoginTime = model.LoginTime,
                LogoutTime = model.LogoutTime
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request?.Cookies["AccessToken"]);
            var response = await _httpClient.PostAsJsonAsync("/api/Timesheet/Create", dto);
             var newModel = new CreateTimeLogViewModel();
             newModel.TimeLogs = await LoadTimeLogs();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorMessage = await response.Content.ReadAsStringAsync(); 
                    ViewData["Error"] = $"Error: {errorMessage}";
                }
                catch (JsonException jsonEx)
                {
                    ViewData["Error"] = "An error occurred while processing the request.";
                }
            }
            return View(newModel);
        }

        private async Task<List<TimeLogViewModel>> LoadTimeLogs()
        {
            var userId = GetCurrentUserId();
            var response = await _httpClient.GetAsync($"/api/Timesheet/all?userId={userId}&pageSize=10&pageNumber=0");
            var result = new List<TimeLogViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var dtoList = await response.Content.ReadFromJsonAsync<List<TimeLogDto>>();
                result = dtoList.Select(dto => new TimeLogViewModel
                {
                    Id = dto.Id,
                    UserId = dto.UserId,
                    Date = dto.Date,
                    LoginTime = dto.LoginTime,
                    LogoutTime = dto.LogoutTime,
                    UserName = dto.UserName
                }).ToList();
            }

            return result;
        }

        private Guid GetCurrentUserId()
        {
 
                return UserController.UserId;
            

            throw new Exception("Invalid or missing user ID in UserId.");
        }

    }

}
