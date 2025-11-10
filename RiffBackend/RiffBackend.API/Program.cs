using RiffBackend.Application.Services;
using RiffBackend.Core.Abstraction;
using RiffBackend.Infrastructure;
using RiffBackend.Infrastructure.MappingProfiles;
using RiffBackend.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(TrackMappingProfile));
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();