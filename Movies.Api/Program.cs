using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Extensions;
using Movies.Api.Helpers;
using Movies.Api.MiddleWares;
using Movies.DL.Data;
using Movies.DL.Data.DataSeeding;
using Movies.DL.Identity;
using Movies.DL.Identity.SeedData;
using Movies.DL.Models.Identity;
using Movies.DL.Services;
using System.Text;

namespace Movies.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ///Will Create builder that from Through it will
            ///detrmine Services and Configurations that App will use it 

            var builder = WebApplication.CreateBuilder(args);
            // Access configuration
            var configuration = builder.Configuration;

            // Add services to the container    .
            #region Configure Services 

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();

            //Add the DbContext as a service to the dependency injection container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
            // sets the Connection string for the SQL Server by retrieving it from the application's configuration  
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Call extension Function Contain on Some services,Function exists at user defined class(ApplicationServicesExtension)
            builder.Services.AddApplicationServices();

            // Add DbContext for AppIdentity with SQL Server configuration
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            //DI to AuthService
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Add Identity services to the application
            // IdentityRole: Represents a role in the ASP.NET Core Identity system
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
            // Bind the JWT configuration section from appsettings.json to the JWT settings class
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

            // Add JWT Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization();

            #endregion

            var app = builder.Build();


            #region Implement Automate database migration and seed data 

            ///Database migrations enhance the development process by automating database schema updates,
            ///improving version control, and providing mechanisms for effective error handling and logging

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var _dbContext = services.GetRequiredService<ApplicationDbContext>();
                var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();

                // Perform database migrations
                await _dbContext.Database.MigrateAsync();
                await DataSeeding.SeedAsync(_dbContext);

                await _identityDbContext.Database.MigrateAsync();

                // Seed roles during application startup
                await SeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has occurred during apply the migration or seeding roles.");
            }

            #endregion


            #region  Configure MiddleWares

            //Will Execute Custom (Invoke Function) to handle Exception or Servererror 
            app.UseMiddleware<ExceptionMiddleware>();   

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //Register 2 MiddleWares To Give You Access on Documentation
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); //0 =>code,Redirect to Specific Endpoint

            //Add MiddleWare That will Redirection Http request to Https
            app.UseHttpsRedirection();
            app.UseCors();
            ///Add MiddleWare That will enable(Allow) Authorization Capabilities,
            ///This Not activation Authorization, to active Authorization you Must Add Configurations  
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStaticFiles();
           ///When a request is made to your API, the MapControllers method ensures that the
            ///appropriate controller and action are invoked based on the request's route.
            app.MapControllers();
            /// It's often used as a final middleware in the pipeline and
            /// after execute it App will Run 
            app.Run();
            #endregion

        }
    }
}