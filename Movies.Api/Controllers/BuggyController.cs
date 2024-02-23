using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Errors;
using Movies.DL.Data.Contexts;

namespace Movies.Api.Controllers
{
    ///Benefits of this Controller =>
    ///1.aiding in the testing and debugging process of error handling logic in the API.
    ///2.The controller serves as documentation by showcasing different error responses
    public class BuggyController : ApiBaseController
    {
            private readonly ApplicationDbContext _dbContext;

            public BuggyController(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            //1-Show Shape(structure) of Response to Notfound error  

            [HttpGet("notfound")]// Get : api/Buggy/notfound

        public async Task<ActionResult> GetNotFoundRequest()
            {
                var movie = await _dbContext.Movies.FindAsync(100);

                if (movie == null)
                    return NotFound(new ApiResponse(404));

                return Ok(movie);
            }

            //2-Show Shape of Response to Server error(Exception)    

            [HttpGet("servererror")] //Get : api/Buggy/servererror
        public async Task<ActionResult> GetServerError()
            {
                var movie = await _dbContext.Movies.FindAsync(100);

                var movieToReturn = movie.ToString(); // Will Throw  Exception [NullReferenceException]

                return Ok(movieToReturn);
            }

            //3-show Shape of Response to BadRequest error

            [HttpGet("badrequest")]

            public ActionResult GetBadRequest() //Get : api/Buggy/badrequest
            {
                return BadRequest(new ApiResponse(400));

            }

            //4-show Shape of Response to Validation error => this is type from types of badrequest

            [HttpGet("badrequest/{id}")]    //Get : api/Buggy/badrequest/five

        public ActionResult GetBadRequest(int id)  //Validation error   
            {
                return Ok();
            }

        //5-show Shape of Response to Notfound end point =>that Will Redirect to Error Contoller 



        //6-show Shape of Response to UnauthorizedError

        [HttpGet("unauthorized")] //Get : /api/Buggy/unauthorized
        public ActionResult GetUnauthorizedError()
            {
                return Unauthorized(new ApiResponse(401));
            }
        }
}
