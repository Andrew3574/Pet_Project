using EventsAPI.Models;
using EventsAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Repositories
{
    public class SharedEventsGuestsRepository : IRepository<SharedEventsGuest>
    {
        private readonly EventsDbContext _dbContext;
        public SharedEventsGuestsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(SharedEventsGuest entity)
        {
            try
            {
                await _dbContext.SharedEventsGuests.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(SharedEventsGuest entity)
        {
            try
            {
                _dbContext.SharedEventsGuests.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<SharedEventsGuest>> GetAll()
        {
            try
            {
                var objList = await _dbContext.SharedEventsGuests.ToListAsync();
                return objList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(SharedEventsGuest entity)
        {
            try
            {
                _dbContext.SharedEventsGuests.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SharedEventsGuest> GetById(int id)
        {
            try
            {
                var obj = await _dbContext.SharedEventsGuests.FindAsync(id);
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
