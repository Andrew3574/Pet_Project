using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventsAPI.Models;
using EventsAPI.Utility;
using EventsAPI.Repositories;
using EventsAPI.Services;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsRepository _eventsRepository;
        private readonly SharedEventsGuestsRepository _sharedEventsGuest;
        private readonly EmailService _emailService;

        public EventsController(EventsRepository eventsRepository,SharedEventsGuestsRepository sharedEventsGuestRepository, EmailService emailService)
        {
            _eventsRepository = eventsRepository;
            _sharedEventsGuest = sharedEventsGuestRepository;
            _emailService = emailService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventsRepository.GetAll();
            return Ok(events);
        }

        // GET: api/Events/3073c69d-4977-4b5a-a8b4-bc8c9e47a7bd
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Event>> GetEventById(Guid id)
        {
            var @event = await _eventsRepository.GetById(id);
            if (@event == null)
            {
                return NotFound();
            }
            return Ok(@event);
        }

        // GET: api/Events/EventName
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByName(string name)
        {
            var @event = await _eventsRepository.GetByName(name);
            if (@event == null)
            {
                return NotFound();
            }
            return Ok(@event);
        }

        // GET: api/Events/EventName
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByCriteria(string date = "", string location = "", string category = "")
        {
           DateTime dateTime;
           DateTime.TryParse(date, out dateTime);

           var events = await _eventsRepository.GetEventByCriteria(dateTime, location, category);
           if (!events.Any())
           {
                return NotFound();
           }
           return Ok(events);
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Event @event)
        {
            await _eventsRepository.Update(@event);

            var sharedEventsGuests = _sharedEventsGuest.GetAll().Result.Where(sh => sh.EventId == @event.EventId);
            foreach(var sh in sharedEventsGuests)
            {
                await _emailService.SendAsync($"{sh.Guest.Email}","Check new updates!", $"Event - {sh.Event.Name} - has been changed\nEvent starts at: ${sh.Event.EventDate}.");
            }
            return Ok();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            await _eventsRepository.Create(@event);
            return Ok();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _eventsRepository.GetById(id);
            if (@event == null)
            {
                return NotFound();
            }
            await _eventsRepository.Delete(@event);
            return Ok();
        }

    }
}
