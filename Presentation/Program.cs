using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Filters;
using EquipmentPhotoRepository = Infrastructure.Repositories.EquipmentPhotoRepository;
using ICompletedWorkPhotoRepository = Domain.Interfaces.ICompletedWorkPhotoRepository;
using IEquipmentPhotoRepository = Domain.Interfaces.IEquipmentPhotoRepository;
using IRequestPhotoRepository = Domain.Interfaces.IRequestPhotoRepository;
using RequestPhotoRepository = Infrastructure.Repositories.RequestPhotoRepository;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddControllers();
		var secret = builder.Configuration["Jwt:Secret"];
		var configuration = builder.Configuration;
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(
				configuration.GetConnectionString("WebApiDatabase"),
				npgsql => npgsql.MigrationsAssembly("Infrastructure")
			)
		);
		builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
					ValidateAudience = false,
					ValidateLifetime = false,
					RoleClaimType = ClaimTypes.Role
				};
			});
		builder.Services.AddAuthorization();
		builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
		builder.Services.AddSingleton<IFileStorageService, MinioStorageService>();
		builder.Services.AddScoped<IEquipmentPhotoRepository, EquipmentPhotoRepository>();
		builder.Services
			.AddScoped<Application.Interfaces.IEquipmentPhotoRepository,
				Application.Services.EquipmentPhotoRepository>();
		builder.Services.AddScoped<IRequestPhotoRepository, RequestPhotoRepository>();
		builder.Services
			.AddScoped<Application.Interfaces.IRequestPhotoRepository, Application.Services.RequestPhotoRepository>();
		builder.Services.AddScoped<IEquipmentService, EquipmentService>();
		builder.Services.AddScoped<IAuthService, AuthService>();
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<ICompanyService, CompanyService>();
		builder.Services.AddScoped<ICompletedWorkPhotoRepository, CompletedWorkPhotoRepository>();
		builder.Services.AddScoped<Application.Interfaces.ICompletedWorkPhotoRepository, CompletedWorkPhotoService>();
		builder.Services.AddScoped<IRequestService, RequestService>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		builder.Services.AddScoped<IRequestRepository, RequestRepository>();
		builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
		builder.Services.AddScoped<IRequestStatusHistoryRepository, RequestStatusHistoryRepository>();
		builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service Request API", Version = "v1" });
			c.AddSecurityDefinition("Bearer",
				new OpenApiSecurityScheme
				{
					Description = "Введите токен в формате: Bearer {token}",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT"
				});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
					},
					Array.Empty<string>()
				}
			});

			// Configure file upload support
			c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

			c.MapType<IFormFile[]>(() => new OpenApiSchema
			{
				Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" }
			});

			c.OperationFilter<FileUploadOperationFilter>();
		});
		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();
		app.MapControllers();
		app.Run();
	}
}
