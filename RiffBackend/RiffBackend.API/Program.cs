using Amazon.S3;
using RiffBackend.Application.Services;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Infrastructure;
using RiffBackend.Infrastructure.Data;
using RiffBackend.Infrastructure.MappingProfiles;
using RiffBackend.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(TrackMappingProfile));
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IFileStorageRepository, FileStorageRepository>();

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings"));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();


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