using Microsoft.EntityFrameworkCore;
using Movies.Api.Extensions;
using Movies.Api.MiddleWares;
using Movies.DL.Data.Contexts;
using Movies.DL.Data.DataSeeding;

namespace Movies.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ///Will Create builder that from Through it will
            ///detrmine Services and Configurations that App will use it 

            var builder = WebApplication.CreateBuilder(args);

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