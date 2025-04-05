using EventsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Net;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private HttpClient _httpClient;

        public AccountController(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _httpClient = _httpClientService.GetClient();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
           
            
            if(String.IsNullOrEmpty(model.Code))
            {
                await _httpClient.PostAsJsonAsync($"https://localhost:7034/api/account/request_code?email={model.Email}",model.Email);
                return View(model);
            }

            var tokenResponse = await _httpClient.GetAsync($"https://localhost:7034/api/account/login?email={model.Email}&code={model.Code}");
            if (tokenResponse.IsSuccessStatusCode)
            {
                string token = await tokenResponse.Content.ReadAsStringAsync();
                HttpContext.Response.Cookies.Append("jwt", token,new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(7),
                    Path = "/"
                });
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("","Wrong confirmation code");
            return View(model);
        }

        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7034/api/account/register", model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Account");
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(content);

            var errors = jsonObject["errors"]?.ToObject<Dictionary<string, List<string>>>();


            if (errors != null)
            {
                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Value)
                    {
                        ModelState.AddModelError(error.Key, errorMessage);
                    }
                }
            }
            return View(model);
        }

    }
}
