namespace Movies.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ///Will Create WebApplicationBuilder that from Through it will
            ///detrmine Services and Configurations that App will use it 

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container    .
            #region Configure Services 
            ///Services Collection => container ,I put inside it Services that will are available to App 
            ///that may Use it as dependencies through DI 

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