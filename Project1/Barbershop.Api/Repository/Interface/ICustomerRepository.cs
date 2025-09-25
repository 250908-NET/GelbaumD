using Barbershop.Models;

namespace Barbershop.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Customer?> GetByIdAsync(int id);
        public Task AddAsync(Customer customer);
        public Task SaveChangesAsync();
    }
}