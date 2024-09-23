using BL.Models.Options;
using BL.Services.HashService;
using BL.Services.TaskService;
using BL.Services.TokenService;
using BL.Services.UserService;
using DAL;
using DAL.Repositories.TaskRepository;
using DAL.Repositories.TaskRepository.TaskRepository;
using DAL.Repositories.UserRepository.UserRepository;
using Lection2_Core_BL.Services.HashService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace TaskAppEBA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppEfContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            builder.Services.AddSingleton<IHashService, HashService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();

            var authOptions = builder.Configuration.GetSection(nameof(AuthOptions));
            builder.Services.Configure<AuthOptions>(authOptions);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(authOptions["Key"]!))
                    };
                });
            builder.Services.AddControllers();

            builder.Logging.AddConsole(); 

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Test API",
                        Version = "v1",
                        Description = "Description"
                    });

                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT"
                    };

                    c.AddSecurityDefinition("Bearer", securityScheme);

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });
                });

            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
