using Microsoft.AspNetCore.Mvc;
using Movies.Api.Errors;
using Movies.Api.Helpers;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.BL.Repositories;

namespace Movies.Api.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services )
        {
            //Registering services into the dependency injection container.

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddAutoMapper(typeof(MappingProfiles));

            // This Configuration to handel Validation Errors and is created only once per the project 
            services.Configure<ApiBehaviorOptions>(Options =>
            {
                // InvalidModelStateResponseFactory: Configures the behavior when model state is invalid.
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)    
                                                        .SelectMany(P => P.Value.Errors) // Merging arrays of error messages into one array.
                                                        .Select(E => E.ErrorMessage)     // Extracting error messages.
                                                        .ToArray();                      // Converting to an array.

                    //Creating a response object containing the errors.
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                   
                    return new BadRequestObjectResult(validationErrorResponse);
                };

            });
            // Returning the updated service collection.
            return services;

        }
    }
}
