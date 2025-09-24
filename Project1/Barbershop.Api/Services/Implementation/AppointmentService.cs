using Barbershop.Models;
using Barbershop.Repositories;

namespace Barbershop.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentService _repo;

        public AppointmentService(IAppointmentService repo)
        {
            _repo = repo;
        
        }

        public async Task<List<Appointment>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Appointment?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CreateAsync(Appointment appointment)
        {
            await _repo.AddAsync(appointments);
            await _repor.SaveChangesAsync();
        }

        // Delete an appointment??
        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _repo.GetIdAsync(id);
            if(appointment == null)
                return false;
            
            _repo.Remove(appointment);
            await _repo.SaveChangesAsync();
            return true;
        }
        // Upate an exiting appointment 
        public async Task<bool> UpdateAsync(int appointmentId, int newBarberId, DateTime newAppointmentTime)
        {
            var existingAppointment = await _repo.GetByIdAsync(appointmentId);
            if (existingAppointment == null)
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

            // Find the barber in the list and replace/update
            var barber = existingAppointment.Barbers.FirstOrDefault(b => b.Id == newBarberId);
            if (barber == null)
            {
                // Optionally, add the new barber to the list
                existingAppointment.Barbers.Add(new Barber { Id = newBarberId });
            }

            existingAppointment.AppointmentTime = newAppointmentTime;

            await _repo.SaveChangesAsync();
            return true;
        }

        // New method: Get all appointments containing a specific barber
        public async Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId)
        {
            var allAppointments = await _repo.GetAllAsync();
            return allAppointments
                .Where(a => a.Barbers.Any(b => b.Id == barberId))
                .ToList();
        }


    }
}