using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Rootics.API.Helpers;
using Rootics.Core.InterFaces;
using Rootics.Core.Models;
using Rootics.EF;
using Rootics.EF.Repsotories;
using System.Globalization;
using System.Text;

namespace Rootics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddJsonFile("appsettings.json"); // Add your configuration file
            // Add AutoMapper
            //builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add Identity To My Project
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IAuthUser, AuthUser>();
            //builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            // Load connection string from appsettings.json
            var Configuration = builder.Configuration;
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Register the DbContext as a service with migrations assembly specified
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = false;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidIssuer = Configuration["JWT:Issuer"],
                       ValidAudience = Configuration["JWT:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                   };
               });

            var app = builder.Build();

            // Enable CORS
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            // Configure the HTTP request pipeline.
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
}
