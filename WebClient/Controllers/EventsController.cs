using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class EventsController : Controller
    {
        private HttpClient _httpClient;
        private readonly IHttpClientService _httpClientService;

        public EventsController(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _httpClient = _httpClientService.GetClient();
        }
        // GET: Event

        public async Task<IActionResult> Index(string name="",string location="",string date="")
        {
            List<Event> events = new List<Event>();

            
            if (!string.IsNullOrEmpty(location) || !string.IsNullOrEmpty(date))
            {
                events = await _httpClient.GetFromJsonAsync<List<Event>>($"https://localhost:7034/api/Events/search?date={date}&location={location}");
            }
            else
            {
                events = await _httpClient.GetFromJsonAsync<List<Event>>("https://localhost:7034/api/Events");
                                
            }
            if (!string.IsNullOrEmpty(name))
            {
                events = await _httpClient.GetFromJsonAsync<List<Event>>($"https://localhost:7034/api/Events/{name}");
            }
            return View(events);
        }



        public async Task<IActionResult> EventsAdminOnly()
        {

            var events = await _httpClient.GetFromJsonAsync<List<Event>>("https://localhost:7034/api/Events/eventsAdminOnly");
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _httpClient.GetFromJsonAsync<Event>($"https://localhost:7034/api/Events/{id}");
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (ModelState.IsValid)
            {

                var response = await _httpClient.DeleteAsync($"https://localhost:7034/api/Events/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("EventsAdminOnly", "Events");
                }
            }
            
            return View();

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            { 
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:7034/api/Events", @event);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("EventsAdminOnly", "Events");
                }
            }            

            return View(@event);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var @event = await _httpClient.GetFromJsonAsync<Event>($"https://localhost:7034/api/Events/{id}");
            return View(@event);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Event @event)
        {
            if (ModelState.IsValid)
            {
                await _httpClient.PutAsJsonAsync($"https://localhost:7034/api/Events", @event);

                return RedirectToAction("EventsAdminOnly", "Events");
            }

            return View(@event);
        }

    }
}
