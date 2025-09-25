using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Barbershop.Models;

public class Appointment
{

    public int Id { get; set; }

    [Required]
    // [Appointment(9,17)]
    public DateTime AppointmentDateAndTime { get; set; }

    [Required, MaxLength(20)]
    public string HaircutType { get; set; }

    public List<Barber> Barbers { get; set; } = new();

    public Customer Customer { get; set; }
}