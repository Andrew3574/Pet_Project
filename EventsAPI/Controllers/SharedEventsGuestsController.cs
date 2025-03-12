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
    public class SharedEventsGuestsController : ControllerBase
    {
        private readonly SharedEventsGuestsRepository _sharedEventsGuestsRepository;

        public SharedEventsGuestsController(SharedEventsGuestsRepository sharedEventsGuestsRepository)
        {
            _sharedEventsGuestsRepository = sharedEventsGuestsRepository;
        }

        // GET: api/SharedEventsGuests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SharedEventsGuest>>> GetSharedEventsGuests()
        {
            var objList = await _sharedEventsGuestsRepository.GetAll();
            return Ok(objList);
        }

        // GET: api/SharedEventsGuests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SharedEventsGuest>> GetSharedEventsGuest(int id)
        {
            var sharedEventsGuest = await _sharedEventsGuestsRepository.GetById(id);
            if(sharedEventsGuest == null)
            {
                return NotFound();
            }
            return Ok(sharedEventsGuest);           
        }

        // PUT: api/SharedEventsGuests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSharedEventsGuest(SharedEventsGuest sharedEventsGuest)
        {
            if(sharedEventsGuest == null)
            {
                return BadRequest();
            }
            await _sharedEventsGuestsRepository.Update(sharedEventsGuest);
            return Ok();
        }

        // POST: api/SharedEventsGuests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SharedEventsGuest>> PostSharedEventsGuest(SharedEventsGuest sharedEventsGuest)
        {
            await _sharedEventsGuestsRepository.Create(sharedEventsGuest);
            return Ok();
        }

        // DELETE: api/SharedEventsGuests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSharedEventsGuest(int id)
        {
            var sharedEventsGuest = await _sharedEventsGuestsRepository.GetById(id);
            await _sharedEventsGuestsRepository.Delete(sharedEventsGuest);
            return Ok();
        }

    }
}
