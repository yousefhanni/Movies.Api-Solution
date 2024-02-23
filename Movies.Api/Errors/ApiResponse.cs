
namespace Movies.Api.Errors
{
    //Standard Response about (Not Found,badRequest,Non-Authorized) error
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;

            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            //Switch Expression, feature appeared at C# 8 
            //switch on statusCode, if 400 return Bad Request ,else if.... 

            return statusCode switch
            {
                400 => "Bad Request,you have made",
                401 => "You are not Authorized",
                404 => "Resource was not found",
                500 => "Internal Server error",
                _ => null,
            };
        }
    }
}
