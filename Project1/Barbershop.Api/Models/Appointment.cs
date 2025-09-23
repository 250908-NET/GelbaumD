using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Barbershop.Models;

public class Appointment
{

    public int Id { get; set; }

    [Required]
    public DateTime AppointmentDateAndTime { get; set; }

    [Required, MaxLength(20)]
    public string HaircutType { get; set; }

    public Barber Barber { get; set; }

    public Customer Customer { get; set; }
}