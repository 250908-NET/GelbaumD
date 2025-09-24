using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
    public class BarberService : IBarberService
    {
        private readonly IBarberRepository _repo;
        private readonly IAppointmentRepository _appointmentRepo;
        
        public BarberService(IBarberRepository repo, IAppointmentRepository _appointmentRepo){
            _repo = repo;
            _appointmentRepo = _appointmentRepo;
        }
        // Get all appointments by barber
    //      public async Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId)
    // {
        
    //         var barber = await _barberRepo.GetByIdAsync(barberId);
    //         if (barber == null) {
    //             throw new KeyNotFoundException($"Barber with ID {barberId} not found.");
    //         }

    //         return await _appointmentRepo.GetAppointmentsByBarberIdAsync(barberId);
    // }
        // public async Task<List<Appointment>> GetAllAsync() => _repo.GetAllAsync();

        // public async Task<Barber?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Barber barber)
        {
           await _repo.AddAsync(barber);
           await _repo.SaveChangesAsync();

        }
    }
}