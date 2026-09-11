using HMS.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<HospitalSettingsOptions>(
    builder.Configuration.GetSection(HospitalSettingsOptions.SectionName));

var app = builder.Build();

app.MapControllers();

app.Run();
