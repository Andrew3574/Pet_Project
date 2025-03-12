using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly GuestsRepository _guestsRepository;

        public GuestsController(GuestsRepository guestsRepository)
        {
            _guestsRepository = guestsRepository;
        }

        // GET: api/Guests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
        {
            var events = await _guestsRepository.GetAll();
            return Ok(events);
        }

        // GET: api/Guests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(Guid id)
        {
            var guest = await _guestsRepository.GetById(id);

            if (guest == null)
            {
                return NotFound();
            }

            return Ok(guest);
        }

        // PUT: api/Guests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(Guest guest)
        {
            if(guest == null)
            {
                return BadRequest();
            }

            await _guestsRepository.Update(guest);
            return Ok();
            
        }

        // POST: api/Guests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuest(Guest guest)
        {
            await _guestsRepository.Create(guest);
            return Ok();
        }

        // DELETE: api/Guests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(Guid id)
        {
            var guest = await _guestsRepository.GetById(id);
            if (guest == null)
            {
                return NotFound();
            }

            await _guestsRepository.Delete(guest);
            return Ok();
        }
    }
}
