using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace Repositories
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
                if(entity == null)
                {
                    return;
                }                
                await _dbContext.Events.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var innerex = ex.InnerException;
                string msg = ex.Message;
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
            catch (Exception ex)
            {
                var inex = ex.InnerException;
                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            try
            {
                var events = await _dbContext.Events.Include(e=>e.Category).ToListAsync();
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
                /*
                var events = await _dbContext.Events.Where(e => e.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
                 */
                var events = await _dbContext.Events.Where(e => e.Name.ToUpper().Contains(name.ToUpper())).ToListAsync();
                return events;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Event>> GetEventByCriteria(string inputDate, string location, string category)
        {
            try
            {
                var queryEvents = _dbContext.Events.AsQueryable();

                if (DateOnly.TryParse(inputDate, out var date))
                {
                    queryEvents = queryEvents.Where(e => DateOnly.FromDateTime(e.EventDate) == date);
                }
                if (!string.IsNullOrEmpty(location))
                {
                    queryEvents = queryEvents.Where(e => e.Location.ToUpper().Contains(location.ToUpper()));
                }
                if (!string.IsNullOrEmpty(category))
                {
                    queryEvents = queryEvents.Where(e => e.Category.Name.Contains(category));
                }


                return await queryEvents.ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
