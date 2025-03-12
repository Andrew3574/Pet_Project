
using System.Net.Http;
using System.Net;

namespace WebClient.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly CookieContainer _cookieContainer;
        public HttpClientService(IHttpContextAccessor httpContextAccessor)
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true
            };

            _httpClient = new HttpClient(handler);

            var request = httpContextAccessor.HttpContext?.Request;
            if (request != null && request.Cookies.TryGetValue("jwt", out var token))
            {
                var cookie = new Cookie("jwt", token)
                {
                    Path = "/",
                    HttpOnly = true
                };
                _cookieContainer.Add(new Uri("https://localhost:7034"), cookie);
            }
        }
        public HttpClient GetClient()
        {
           return _httpClient;
        }
    }
}
