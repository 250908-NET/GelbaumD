using Barbershop.Models;

namespace Barbershop.Services
{
    public interface IBarberService
    {
        // Show all their appointments
        public Task<List<Appointment>> GetAllSync();
       
    }
}