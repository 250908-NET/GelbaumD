using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Barbershop.Data;
using Barbershop.Models;
using Barbershop.Services;
using Barbershop.Repositories;
using Barbershop.DTOs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
string CS = File.ReadAllText("./ConnectionString.env");

Console.WriteLine(CS);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();   // ✅ Needed for Swagger/OpenAPI
builder.Services.AddSwaggerGen();            // ✅ Needed for Swagger/OpenAPI
builder.Services.AddOpenApi();

// ✅ Register DbContext BEFORE builder.Build()
builder.Services.AddDbContext<BarbershopDbContext>(options =>
    options.UseSqlServer(CS));

// ✅ You can also register repositories/services here
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBarberRepository, BarberRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBarberService, BarberService>();

var app = builder.Build(); // ✅ Build AFTER services registered

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// _____________________________Root_____________________________
app.MapGet("/", () => {
    return "Lets get you a crispy fade!";
});
// _____________________________Customer_____________________________
// Create a customer
app.MapPost("/customers", async (Customer customer, ICustomerService customerService) =>
{
    // Example: only validate Name since PhoneNumber doesn't exist
    if (string.IsNullOrWhiteSpace(customer.FirstName))
    {
        return Results.BadRequest(new { message = "Customer name is required." });
    }

    try
    {
        await customerService.CreateAsync(customer);
        return Results.Created($"/customers/{customer.Id}", customer);
    }
    catch (Exception ex)
    {
        // Optionally log the exception (Serilog, etc.)
        // Log.Error(ex, "Error creating customer");

        return Results.Problem("An error occurred while creating the customer.");
    }
});
// Get Customer by Id
app.MapGet("/customers/{id:int}", async (int id, ICustomerService customerService) =>
{
    try
    {
        var customer = await customerService.GetByIdAsync(id);

        if (customer == null)
        {
            return Results.NotFound(new { message = $"Customer with ID {id} not found." });
        }

        return Results.Ok(customer);
    }
    catch (Exception ex)
    {
        // Log.Error(ex, "Error retrieving customer");
        return Results.Problem("An error occurred while retrieving the customer.");
    }
});

// _____________________________Appointments_____________________________
// Helps with Haircut types, now a selection of strings in an Enum...
string GetDisplayName(Enum enumValue) =>
    enumValue.GetType()
             .GetMember(enumValue.ToString())[0]
             .GetCustomAttribute<DisplayAttribute>()?
             .Name ?? enumValue.ToString();

// Create an appointment
app.MapPost("/appointments", async (AppointmentCreateDto dto, IAppointmentService appointmentService) =>
{
    // Validate basic request
    if (dto.CustomerId <= 0)
        return Results.BadRequest(new { message = "CustomerId is required." });
    if (!dto.BarberIds.Any())
        return Results.BadRequest(new { message = "At least one BarberId is required." });

    try
    {
        var appointment = new Appointment
        {
            AppointmentDateAndTime = dto.AppointmentDateAndTime,
            HaircutType = dto.HaircutType,
            CustomerId = dto.CustomerId,
            Barbers = dto.BarberIds.Select(id => new Barber { Id = id }).ToList()
        };

        await appointmentService.CreateAsync(appointment);

        return Results.Created($"/appointments/{appointment.Id}", new
        {
            appointment.Id,
            appointment.AppointmentDateAndTime,
            HaircutType = appointment.HaircutType.ToString(),
            appointment.CustomerId,
            Barbers = dto.BarberIds
        });
    }
    catch (Exception ex)
    {
        // TODO: log exception with Serilog
        return Results.Problem("Failed to create appointment: " + ex.Message);
    }
});
// get an appointment by its Id
app.MapGet("/appointments/{id:int}", async (int id, IAppointmentService appointmentService) =>
{
    var appointment = await appointmentService.GetByIdAsync(id);

    if (appointment == null)
        return Results.NotFound(new { message = $"Appointment with ID {id} not found." });

    var dto = new AppointmentDto
    {
        Id = appointment.Id,
        AppointmentDateAndTime = appointment.AppointmentDateAndTime,
        HaircutType = GetDisplayName(appointment.HaircutType) // "Haircut and Beard"
    };

    return Results.Ok(dto);
});
// Update a current appointment
app.MapPut("/appointments/{id:int}", async (int id, AppointmentUpdateDto dto, IAppointmentService appointmentService) =>
{
    try
    {
        bool updated = await appointmentService.UpdateAsync(id, dto.BarberId, dto.AppointmentDateAndTime);

        if (!updated)
        {
            return Results.NotFound(new { message = $"Appointment with ID {id} not found." });
        }

        return Results.Ok(new
        {
            message = "Appointment updated successfully.",
            appointmentId = id,
            newBarberId = dto.BarberId,
            newTime = dto.AppointmentDateAndTime
        });
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        // If you add the double-booking check inside UpdateAsync
        return Results.Conflict(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to update appointment: " + ex.Message);
    }
});
// Get all appointments by barberId
app.MapGet("/appointments/barber/{barberId:int}", async (int barberId, IAppointmentService appointmentService) =>
{
    try
    {
        // Grabs all appointments associated with that barberId
        var appointments = await appointmentService.GetAppointmentsByBarberIdAsync(barberId);
       
        if (appointments == null || !appointments.Any())
        {
            // Handle no appointments found
            return Results.NotFound(new { message = $"No appointments found for barber with ID {barberId}." });
        }

        var response = appointments.Select(a => new
        {
            a.Id,
            a.AppointmentDateAndTime,
            HaircutType = a.HaircutType.ToString(),
            a.CustomerId
        });

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to retrieve appointments: " + ex.Message);
    }
});
// Delete an existing Appointment
app.MapDelete("/appointments/{id:int}", async (int id, IAppointmentService appointmentService) =>
{
    try
    {
        bool deleted = await appointmentService.DeleteAsync(id);

        if (!deleted)
        {
            return Results.NotFound(new { message = $"Appointment with ID {id} not found." });
        }

        return Results.Ok(new { message = $"Appointment with ID {id} was deleted successfully." });
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to delete appointment: " + ex.Message);
    }
});

// _____________________________Barber_____________________________
// Create a barber
app.MapPost("/barbers", async (BarberCreateDto dto, IBarberService barberService) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name))
    {
        return Results.BadRequest(new { message = "Barber name is required." });
    }

    try
    {
        var barber = new Barber
        {
            Name = dto.Name
        };

        await barberService.CreateAsync(barber);

        return Results.Created($"/barbers/{barber.Id}", new
        {
            barber.Id,
            barber.Name
        });
    }
    catch (Exception ex)
    {
        return Results.Problem("Failed to create barber: " + ex.Message);
    }
});



app.Run();

