using Barbershop.Models;
using Barbershop.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BarbershopDbContext _context;

        public CustomerRepository(BarbershopDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            //return await _context.Students.Where( student => student.id  == id);
            throw new NotImplementedException();
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
