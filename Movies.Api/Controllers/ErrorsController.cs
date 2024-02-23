using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Errors;

namespace Movies.Api.Controllers
{
    //This class to Handel NotfoundEndpoint error

    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    public class ErrorsController : ControllerBase
    {
        //EndPoint that Will Redirect to it incase that execute Not found Endpoint  

        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code, "The EndPoint You are try to Get, Not Found"));
        }
    }
}