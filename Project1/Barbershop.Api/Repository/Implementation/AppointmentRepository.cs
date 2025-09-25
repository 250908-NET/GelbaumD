using Barbershop.Models;
using Barbershop.Data;
using Microsoft.EntityFrameworkCore;

namespace Barbershop.Repositories
{   
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BarbershopDbContext _context;

        public AppointmentRepository(BarbershopDbContext context)
        {
            _context = context;
        }

        public Task<List<Appointment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Appointment?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void Remove(Appointment appointment)
        {
            throw new NotImplementedException();
        }

          public void Update(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Appointment>> GetAppointmentsByBarberIdAsync(int barberId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BarberHasAppointmentAtTimeAsync(int barberId, DateTime time, int? excludeAppointmentId = null)
        {
            // 
            return await _context.Appointments
                .Include(a => a.Barbers)
                .AnyAsync(a =>
                    a.AppointmentDateAndTime == time &&
                    a.Barbers.Any(b => b.Id == barberId) &&
                    (!excludeAppointmentId.HasValue || a.Id != excludeAppointmentId.Value));
        }
    }
 
}