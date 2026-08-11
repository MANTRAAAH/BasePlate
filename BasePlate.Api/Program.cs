using BasePlate.Api.Services;
using BasePlate.Core.Interfaces;
using BasePlate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registriamo il Tenant Provider
builder.Services.AddScoped<ITenantProvider, CurrentTenantProvider>();

// 2. Registriamo Entity Framework e PostgreSQL
builder.Services.AddDbContext<BasePlateDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Aggiungiamo i controller e Swagger (standard web api)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurazione della pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();