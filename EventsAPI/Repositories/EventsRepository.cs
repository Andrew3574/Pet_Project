using EventsAPI.Models;
using EventsAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventsAPI.Repositories
{
    public class EventsRepository : IRepository<Event>
    {
        private readonly EventsDbContext _dbContext;
        public EventsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Event entity)
        {
            try
            {
                await _dbContext.Events.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Event entity)
        {
            try
            {
                _dbContext.Events.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            try
            {
                var events = await _dbContext.Events.ToListAsync();
                return events;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Event entity)
        {
            try
            {
                _dbContext.Events.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Event> GetById(Guid id)
        {
            try
            {
                var @event = await _dbContext.Events.FindAsync(id);
                return @event;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetByName(string name)
        {
            try
            {
                var events = await _dbContext.Events.Where(e => e.Name.Contains(name)).ToListAsync();
                return events;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Event>> GetEventByCriteria(DateTime date, string location, string category)
        {
            try
            {
                var queryEvents = _dbContext.Events.AsQueryable();

                if (date > DateTime.MinValue && date < DateTime.MaxValue)
                {
                    queryEvents = queryEvents.Where(e => e.EventDate.ToUniversalTime().Date == date.ToUniversalTime().Date);
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

                return events;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
