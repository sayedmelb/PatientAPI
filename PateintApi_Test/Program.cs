using PatientAPI.Application.Interfaces;
using PatientAPI.Application.Services;
using PatientAPI.Domain.Repositories;
using PatientAPI.Infrastructure.Configuration;
using PatientAPI.Infrastructure.Extensions;
using PatientAPI.Infrastructure.Persistence.Repositories;
using PatientAPI.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(PatientAPI.Application.Mapping.MappingProfile));

// Register repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();

// Register services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddDataSeeding();
// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var seedingService = scope.ServiceProvider.GetRequiredService<IDataSeedingService>();
    await seedingService.SeedDatabaseAsync();
}
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
