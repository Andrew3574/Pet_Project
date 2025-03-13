using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoriesRepository : IRepository<Category>
    {
        private readonly EventsDbContext _dbContext;
        public CategoriesRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Create(Category entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                var categories = await _dbContext.Categories.ToListAsync();
                return categories;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public Task Update(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
