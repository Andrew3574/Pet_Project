namespace Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task Delete(T entity);
        public Task Update(T entity);
        public Task Create(T entity);
    }
}
