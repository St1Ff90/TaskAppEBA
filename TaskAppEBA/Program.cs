
using BL.Profiles;
using BL.Services;
using BL.Services.HashService;
using BL.Services.TokenService;
using DAL;
using DAL.Repositories;
using Lection2_Core_BL.Services.HashService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace TaskAppEBA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppEfContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            builder.Services.AddScoped<UserService>();

            builder.Services.AddSingleton<IHashService, HashService>();

            builder.Services.AddSingleton<ITokenService, TokenService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var assemblies = new[]
            {
                typeof(UserProfile).Assembly
            };
            builder.Services.AddAutoMapper(assemblies);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
