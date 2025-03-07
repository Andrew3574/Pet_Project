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
    public class SharedEventsGuestsController : ControllerBase
    {
        private readonly EventsDbContext _context;

        public SharedEventsGuestsController(EventsDbContext context)
        {
            _context = context;
        }

        // GET: api/SharedEventsGuests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SharedEventsGuest>>> GetSharedEventsGuests()
        {
            return await _context.SharedEventsGuests.ToListAsync();
        }

        // GET: api/SharedEventsGuests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SharedEventsGuest>> GetSharedEventsGuest(int id)
        {
            var sharedEventsGuest = await _context.SharedEventsGuests.FindAsync(id);

            if (sharedEventsGuest == null)
            {
                return NotFound();
            }

            return sharedEventsGuest;
        }

        // PUT: api/SharedEventsGuests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSharedEventsGuest(int id, SharedEventsGuest sharedEventsGuest)
        {
            if (id != sharedEventsGuest.Id)
            {
                return BadRequest();
            }

            _context.Entry(sharedEventsGuest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SharedEventsGuestExists(id))
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

        // POST: api/SharedEventsGuests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SharedEventsGuest>> PostSharedEventsGuest(SharedEventsGuest sharedEventsGuest)
        {
            _context.SharedEventsGuests.Add(sharedEventsGuest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSharedEventsGuest", new { id = sharedEventsGuest.Id }, sharedEventsGuest);
        }

        // DELETE: api/SharedEventsGuests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSharedEventsGuest(int id)
        {
            var sharedEventsGuest = await _context.SharedEventsGuests.FindAsync(id);
            if (sharedEventsGuest == null)
            {
                return NotFound();
            }

            _context.SharedEventsGuests.Remove(sharedEventsGuest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SharedEventsGuestExists(int id)
        {
            return _context.SharedEventsGuests.Any(e => e.Id == id);
        }
    }
}
