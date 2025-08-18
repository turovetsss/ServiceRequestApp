using System.Text;
using Application.Interfaces;
using Infrastructure.Data;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Application.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Npgsql;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(
        configuration.GetConnectionString("WebApiDatabase"),
        npgsql => npgsql.MigrationsAssembly("Infrastructure")
    )
);
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service Request API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

