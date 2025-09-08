using csharp_api.Application.Interfaces;
using csharp_api.Application.Services;
using csharp_api.Application.Validation;
using csharp_api.Domain.AutoMapper;
using csharp_api.Domain.Interfaces;
using csharp_api.Infrastructure.Database;
using csharp_api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// configuração do banco de dados

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new() { Title = "Api in C# .Net", Version = "v1" }));

// configuração da DI (serviços e repositórios, automapper, etc)
builder.Services.AddScoped<UserDtoValidator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// DI do automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.MapControllers();
app.Run();

// Feito!