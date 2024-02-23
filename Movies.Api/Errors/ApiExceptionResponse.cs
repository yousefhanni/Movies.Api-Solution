using Movies.Api.Errors;

namespace Movies.Api.Extensions
{
    //This class => Body of Exception Response Message contain Object => 1.Message  2.status Code  3.Details of Exception

    public class ApiExceptionResponse : ApiResponse
    {

        public string? Details { get; set; }

        public ApiExceptionResponse(int statusCode, string? message = null, string? details = null)
            : base(statusCode, message)
        {
            Details = details;
        }

    }
}
