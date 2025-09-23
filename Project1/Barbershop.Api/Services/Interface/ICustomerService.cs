using Barbershop.Models;

namespace Barbershop.Services
{
    public interface ICustomerService
    {
        public Task<Customer?> GetByIdAsync(int id);
        public Task<List<Appointment>> GetAllSync();
        public Task CreateAsync(Customer customer);

    }
}