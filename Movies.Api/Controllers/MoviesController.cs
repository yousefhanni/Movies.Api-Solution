using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Dtos;
using Movies.Api.Errors;
using Movies.Api.Helpers;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.BL.Specifications.MovieSpecs;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    public class MoviesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MoviesController( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //Fetches all movies from the database, orders them by their Rating in descending order,
        //includes their associated genre information, and returns them 
        [HttpGet]
        public async Task<ActionResult<Pagination<MovieDetailsDto>>> GetAllAsync([FromQuery] MovieSpecParams specParams)
        {
           //[FromQuery]=>(Model Binding) instructs ASP.NET Core to bind data from the URL query string to the specParams parameter

            ///Create object from class MovieWithGenreSpecifiations that will Cary values of Specifications
            ///that will pass it to (GetAllWithSpecAsync) and (GetAllWithSpecAsync) will pass Specifications to (GetQuery) method that will build Query with Dynamic way
            ///And if sent (genreId) at URL will Get all Movies that have the same genreId
            var spec = new MovieWithGenreSpecifiations(specParams);  

            var movies = await _unitOfWork.GetRepository<Movie>().GetAllWithSpecsAsync(spec);
            //Data after Filteration,sorting and Pagination
            var data = _mapper.Map<IReadOnlyList<Movie>, IReadOnlyList<MovieDetailsDto>>(movies);

            var countSpec = new MoviesSpecsForCount(specParams);

            var count = await _unitOfWork.GetRepository<Movie>().GetCountWithSpecAsync(countSpec);
            ///Any endpoint work with Pagination always has standard response =>
            ///response is object consists of four properties =>1.PageIndex 2.PageSize 3.Count 4.Data itself 
            return Ok(new Pagination<MovieDetailsDto>(specParams.PageIndex,specParams.PageSize, count, data));
        }

       
        // Get specific Movie by Movie id  
        [HttpGet("{id}")]
        public async Task<ActionResult> GetbyIdAsync(int id)
        {
            ///Create object from class MovieWithGenreSpecifiations that will Cary values of Specifications
            ///that will pass it to (GetWithSpecAsync) and GetWithSpecAsync will pass Specifications to (GetQuery) method
            var spec = new MovieWithGenreSpecifiations(id);

            var existingMovie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (existingMovie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}"));
            }
            return Ok(_mapper.Map<Movie, MovieDetailsDto>(existingMovie));

        }


        [HttpPost]
        public async Task<ActionResult> AddMovieAsync(MovieDto movie)
        {

            //Ensure that only valid genre IDs are accepted at request body before proceeding with further operations related to the movie
            var isValidGenre = await _unitOfWork.GetRepository<Movie>().IsvalidGenre(movie.GenreId);
            if (!isValidGenre)
                return BadRequest(new ApiResponse(400, "Invalid genre ID!"));

            var mappedMovie = _mapper.Map<MovieDto, Movie>(movie);

            var addedMovie = await _unitOfWork.GetRepository<Movie>().AddAsync(mappedMovie);
            await _unitOfWork.CompleteAsync();

            return Ok(addedMovie);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var spec = new MovieWithGenreSpecifiations(id);

            var movie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (movie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}")); //appropriate action if movie with given ID is not found
            }

             _unitOfWork.GetRepository<Movie>().DeleteAsync(movie);

            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Movie, MovieDetailsDto>(movie));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] MovieDto updatedMovieDto)
        {
            // Get specific movie
            var spec = new MovieWithGenreSpecifiations(id);

            var existingMovie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (existingMovie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}")); //Appropriate action if movie with given ID is not found
            }
            
            var isValidGenre = await _unitOfWork.GetRepository<Movie>().IsvalidGenre(updatedMovieDto.GenreId);
               if (!isValidGenre)
                   return BadRequest(new ApiResponse(400, "Invalid genre ID!"));
            
            // Check if the PosterUrl property is provided in the updatedMovieDto
            if (updatedMovieDto.PosterUrl == null)
            {
                // If PosterUrl is not provided, retain the existing value
                updatedMovieDto.PosterUrl = existingMovie.PosterUrl;
            }

            // Map from updatedMovieDto to existingMovie
            _mapper.Map(updatedMovieDto, existingMovie);

            // Update the movie in the repository
         _unitOfWork.GetRepository<Movie>().UpdateAsync(existingMovie);
            await _unitOfWork.CompleteAsync();

            return Ok(existingMovie);
        }

    }
}
