using BasePlate.Api.Services;
using BasePlate.Core.Interfaces;
using BasePlate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore; // <-- Nuova libreria UI
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Registriamo il Tenant Provider
builder.Services.AddHttpContextAccessor(); // 👈 AGGIUNGI QUESTA RIGA!
builder.Services.AddScoped<ITenantProvider, CurrentTenantProvider>();

// 2. Registriamo Entity Framework e PostgreSQL
builder.Services.AddDbContext<BasePlateDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Aggiungiamo i controller e Swagger (standard web api)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// 🟢 NATIVE .NET 9 OPENAPI (Sostituisce AddSwaggerGen)
builder.Services.AddOpenApi();
// ... altri servizi (es. AddControllers, AddDbContext)

// 🔓 Aggiungiamo la policy CORS per Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // L'indirizzo del server di sviluppo Angular
              .AllowAnyHeader()  // Permette qualsiasi intestazione (incluso il futuro token JWT)
              .AllowAnyMethod(); // Permette GET, POST, PUT, DELETE, ecc.
    });
});
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });
var app = builder.Build();

// Configurazione della pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // 🟢 NATIVE .NET 9 UI (Sostituisce UseSwagger e UseSwaggerUI)
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();