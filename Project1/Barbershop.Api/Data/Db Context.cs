using Microsoft.EntityFrameworkCore;
using barbershop.Models;

namespace Barbershop.Data;

public class BarbershopDbContext : Db BarbershopDbContext
{

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Apointment> Appointments { get; set; }
    public DbSet<Barber> Barbers { get; set; }

    public BarbershopDbContext( DbContextOptions<BarbershopDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}