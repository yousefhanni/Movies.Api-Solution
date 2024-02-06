using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.BL.Interfaces.Repository;
using Movies.BL.Repositories;
using Movies.DL.Data.Contexts;
using Movies.DL.Data.DataSeeding;

namespace Movies.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ///Will Create WebApplicationBuilder that from Through it will
            ///detrmine Services and Configurations that App will use it 

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container    .
            #region Configure Services 
            ///Services Collection => container ,I put inside it Services that will are available to App 
            ///that may Use it as dependencies through DI 

            //Add the DbContext as a service to the dependency injection container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
            // sets the Connection string for the SQL Server by retrieving it from the application's configuration  
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Allow DI(Register to IGenericRepository) 
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));


            ///AddControllers:Add Services to DI Container That App need it
            ///Such as MVC Services,AddCors,AddAuthorization,AddFormatterMappings,...
            builder.Services.AddControllers();
            //=>AddEndpointsApiExplorer:related to configuring or adding functionality related to API documentation.
            builder.Services.AddEndpointsApiExplorer();

      
            builder.Services.AddCors();

            ///used to configure and enable Swagger documentation generation for your API. 
            ///ApiExplorer + AddSwaggerGen => Used to Generate Swagger documentation
            builder.Services.AddSwaggerGen();
            #endregion

            var app = builder.Build();

            #region Implement Automate database migration and seed data 

            ///Database migrations enhance the development process by automating database schema updates,
            ///improving version control, and providing mechanisms for effective error handling and logging

            using var Scope = app.Services.CreateScope();

            var Services = Scope.ServiceProvider;

            var _dbContext = Services.GetRequiredService<ApplicationDbContext>();

            var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync(); //Update Database
                await DataSeeding.SeedAsync(_dbContext);  //Data Seeding

            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has occured during apply the migration");

            } 
            #endregion


            #region  Configure MiddleWares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //Register 2 MiddleWares To Give You Access on Documentation
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Add MiddleWare That will Redirection Http request to Https
            app.UseHttpsRedirection();

            app.UseCors();

            ///Add MiddleWare That will enable(Allow) Authorization Capabilities,
            ///This Not activation Authorization, to active Authorization you Must Add Configurations  
            app.UseAuthorization();

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