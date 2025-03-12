using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class SharedEventsGuestsRepository : IRepository<SharedEventsGuest>
    {
        private readonly EventsDbContext _dbContext;
        public SharedEventsGuestsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Регистрация участника на мероприятие
        public async Task Create(SharedEventsGuest entity)
        {
            try
            {
                var @event = await _dbContext.Events.FindAsync(entity.Event.EventId);

                if (@event.GuestLimit != 0)
                {
                    await _dbContext.SharedEventsGuests.AddAsync(entity);
                    @event.GuestLimit -= 1;
                    await _dbContext.SaveChangesAsync();
                   
                }
                else
                    throw new Exception("Отсутствуют свободные места на мероприятие");

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
                var @event = await _dbContext.Events.FindAsync(entity.EventId);
                _dbContext.SharedEventsGuests.Remove(entity);
                await _dbContext.SaveChangesAsync();
                @event.GuestLimit += 1;
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

        public async Task<List<SharedEventsGuest>> GetByGuest(Guest entity)
        {
            try
            {
                var obj = _dbContext.SharedEventsGuests.Where(eg=>eg.Guest.GuestId == entity.GuestId);
                return await obj.ToListAsync();
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
