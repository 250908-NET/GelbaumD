using Microsoft.EntityFrameworkCore;
using Barbershop.Data;
using Barbershop.Models;
using Barbershop.Services;
using Barbershop.Repositories;
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
// ____________Root__________
app.MapGet("/", () => {
    return "Lets get you a crispy fade!";
});
// _________Customer__________
// Create a customer
app.MapPost("/customers", async (Customer customer, ICustomerService customerService) =>
{
    // Example: only validate Name since PhoneNumber doesn't exist
    if (string.IsNullOrWhiteSpace(customer.Name))
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

app.Run();

