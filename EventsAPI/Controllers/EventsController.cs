using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventsAPI.Models;
using EventsAPI.Utility;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public EventsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/3073c69d-4977-4b5a-a8b4-bc8c9e47a7bd
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Event>> GetEventById(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // GET: api/Events/EventName
        [HttpGet("{name:alpha}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByName(string name)
        {
            var @event = await _context.Events.Where(e=>e.Name == name).ToListAsync();
            
            if (@event == null)
            {
                return NotFound();
            }
            return @event;
        }
        // GET: api/Events/EventName
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventByCriteria(DateTime date, string location, string category)
        {
            try
            {
                var queryEvents = _context.Events.AsQueryable();

                if (date > DateTime.MinValue || date < DateTime.MaxValue)
                {
                    queryEvents = queryEvents.Where(e => e.EventDate == date);
                }
                if (location != String.Empty)
                {
                    queryEvents = queryEvents.Where(e => e.Location.Contains(location));
                }
                if (category != String.Empty)
                {
                    queryEvents = queryEvents.Where(e => e.Category.Name.Contains(category));
                }

                var events = await queryEvents.ToListAsync();
                /*if (events.Count == 0) 
                { 
                    return NotFound("no events with such criteria");
                }*/

                return events;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(Guid id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
