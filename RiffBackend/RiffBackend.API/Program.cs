using Amazon.S3;
using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using RiffBackend.API.Extensions;
using RiffBackend.API.Middleware;
using RiffBackend.Application.Common;
using RiffBackend.Application.Services;
using RiffBackend.Application.Validators;
using RiffBackend.Core.Abstraction.Repository;
using RiffBackend.Core.Abstraction.Service;
using RiffBackend.Infrastructure;
using RiffBackend.Infrastructure.Data;
using RiffBackend.Infrastructure.MappingProfiles;
using RiffBackend.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

//db and auto mapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(TrackMappingProfile));
builder.Services.AddDbContext<ApplicationDbContext>();

//repos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IFileStorageRepository, FileStorageRepository>();

//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IFileProcessor, FileProcessor>();

//auth
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddApiAuthentication(builder.Configuration);

//s3
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings"));
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

//validators
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TrackRequestValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseCors(x => x
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.UseAuthentication();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();