using Barbershop.Models;

namespace Barbershop.Services
{
    public interface IAppointmentService
    {
        // Get all appointments by Customer Id
        // Get all appointments by Barber Id or Name 
        // List all appointments by day? 
        public Task<List<Appointment>> GetAllAsync();
        public Task<Appointment?> GetByIdAsync(int id);
        public Task CreateAsync(Appointment appointment);
    }
}