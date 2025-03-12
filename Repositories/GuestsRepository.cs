using Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Repositories
{
    public class GuestsRepository : IRepository<Guest>
    {
        private readonly EventsDbContext _dbContext;
        public GuestsRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Guest entity)
        {
            try
            {
                entity.Role_id = 1;
                await _dbContext.Guests.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Guest entity)
        {
            try
            {
                _dbContext.Guests.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Guest>> GetAll()
        {
            try
            {
                var guests = await _dbContext.Guests.ToListAsync();
                return guests;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Update(Guest entity)
        {
            try
            {
                _dbContext.Guests.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Guest> GetById(Guid id)
        {
            try
            {
                var guest = await _dbContext.Guests.FindAsync(id);
                return guest;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Guest> GetByEmail(string email)
        {
            try
            {
                var guest = _dbContext.Guests.First(g=>g.Email == email);
                return guest;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
