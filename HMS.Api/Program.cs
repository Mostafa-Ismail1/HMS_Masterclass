using System.Diagnostics;
using HMS.Api;
using HMS.Api.Data;
using HMS.Api.Data.Repositories;
using HMS.Api.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.Configure<HospitalSettingsOptions>(
    builder.Configuration.GetSection(HospitalSettingsOptions.SectionName));

builder.Services.AddDbContext<HmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<LogActionActivityFilter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Branch: emergency requests
app.Map("/emergency", emergencyApp =>
{
    emergencyApp.Run(async context =>
    {
        await context.Response.WriteAsync("Emergency Department: Fast-track pipeline active!");
    });
});

app.MapControllers();

app.Run();