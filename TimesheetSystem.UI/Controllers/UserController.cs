using Microsoft.AspNetCore.Mvc;
using TimesheetSystem.Appliication.Dtos;
using TimesheetSystem.UI.Models;

namespace TimesheetSystem.UI.Controllers
{

    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public static Guid UserId { get; set; }
       
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var dto = new CreateUserDto
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                
            };
            var jsonInput = JsonContent.Create(dto);
            var response = await _httpClient.PostAsJsonAsync("/api/User/Register", dto);
   
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            var resp = await response.Content.ReadFromJsonAsync<string>();
            ViewData["Error"] = resp;
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpGet]
        public IActionResult Logout() {


            HttpContext.Response.Cookies.Append("AccessToken", "null", new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("/api/User/login", model);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Login failed");
                var error = await response.Content.ReadFromJsonAsync<string>();
                ViewData["Error"] = error;
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<RegisterationDto>();
            
            HttpContext.Session.SetString("UserId", result.Token);

            if (response.IsSuccessStatusCode)
            {

                HttpContext.Response.Cookies.Append("AccessToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true
                });
                UserId= result.UserId;    
                return RedirectToAction("Create", "Timesheet");

            }
          

                return RedirectToAction("Login");
        }

  
    }

}
