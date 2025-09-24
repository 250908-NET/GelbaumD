using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IAppointmentRepository _appointmentRepo;

        public CustomerService(ICustomerRepository repo, IAppointmentRepository _appointmentRepo)
        {
            _repo = repo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<Customer?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Customer customer)
        {
            await _repo.AddAsync(customer);
            await _repo.SaveChangesAsync();
        }

          // Get all appointments by Customer
        //   public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId)
        // {
        //     var customer = await _customerRepo.GetByIdAsync(customerId);
        //     if (customer == null)
        //         throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

        //     return await _appointmentRepo.GetAppointmentsByCustomerIdAsync(customerId);
        // }


        

        // public async Task<List<Appointment>> GetAllAsync() => await _repo. ???

    }
}