using WebApplicationPostMeeting.Controllers;
using WebApplicationPostMeeting.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registramos la implementaci�n de IOrderLogger<OrdersController>
builder.Services.AddSingleton<IOrderLogger<OrdersController>, FileLogger>();

var app = builder.Build();

// Registro el middleware de normalizaci�n de pedidos
app.UseMiddleware<OrderDeduplicationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
