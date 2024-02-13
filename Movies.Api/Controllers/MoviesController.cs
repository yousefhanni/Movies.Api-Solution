using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Dtos;
using Movies.BL.Interfaces.Repository;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    public class MoviesController : ApiBaseController
    {
        private readonly IGenericRepository<Movie> _moviesRepo;
        private readonly IMapper _mapper;

        public MoviesController(IGenericRepository<Movie> moviesRepo, IMapper mapper)
        {
            _moviesRepo = moviesRepo;
            _mapper = mapper;
        }


        //Fetches all movies from the database, orders them by their Rating in descending order,
        //includes their associated genre information, and returns them 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllAsync()
        {
            var movies = await _moviesRepo.GetAllAsync();

          return Ok(_mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDetailsDto>>(movies));

        }

       
        // Get specific Movie by Movie id  
        [HttpGet("{id}")]
        public async Task<ActionResult> GetbyIdAsync(int id)
        {
            var existingMovie = await _moviesRepo.GetByIdAsync(id);

            if (existingMovie == null)
            {
                return NotFound($"No Movie was found with ID:{id}");
            }
            return Ok(_mapper.Map<Movie, MovieDetailsDto>(existingMovie));

        }


        //Get All Movies that have same Genreid(that have same Genre)
        [HttpGet("GetbyGenreId")]    // GET: api/Movies/GetbyGenreId?genreId=2
        public async Task<ActionResult> GetbyGenreIdAsync(int genreid)
        {
            var existingMovie = await _moviesRepo.GetAllByIdAsync(genreid);

            /// If no movies are found for the provided genre ID(null) or the collection is empty,
            ///return a 404 Not Found response
            if (existingMovie == null || !existingMovie.Any())
            {
                return NotFound($"No Movie was found with genreid:{genreid}");
            }

            return Ok(_mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDetailsDto>>(existingMovie));

        }


        [HttpPost]
        public async Task<ActionResult> AddMovieAsync(MovieDto movie)
        {
            // Ensure that only valid genre IDs are accepted before proceeding with further operations related to the movie
            var isValidGenre = await _moviesRepo.IsvalidGenre(movie.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");

            var mappedMovie = _mapper.Map<MovieDto, Movie>(movie);

            var addedMovie = await _moviesRepo.AddAsync(mappedMovie);

            return Ok(addedMovie);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var movie = await _moviesRepo.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound($"No genre was found with ID:{id}"); //appropriate action if movie with given ID is not found
            }

            var deletedMovie = await _moviesRepo.DeleteAsync(movie);

            return Ok(_mapper.Map<Movie, MovieDetailsDto>(deletedMovie));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] MovieDto updatedMovieDto)
        {
            // Get specific movie
            var existingMovie = await _moviesRepo.GetByIdAsync(id);

            if (existingMovie == null)
            {
                return NotFound($"No movie was found with ID:{id}"); // Appropriate action if movie with given ID is not found
            }
            
            var isValidGenre = await _moviesRepo.IsvalidGenre(updatedMovieDto.GenreId);
               if (!isValidGenre)
                   return BadRequest("Invalid genere ID!");
            
            // Check if the PosterUrl property is provided in the updatedMovieDto
            if (updatedMovieDto.PosterUrl == null)
            {
                // If PosterUrl is not provided, retain the existing value
                updatedMovieDto.PosterUrl = existingMovie.PosterUrl;
            }

            // Map from updatedMovieDto to existingMovie
            _mapper.Map(updatedMovieDto, existingMovie);

            // Update the movie in the repository
            var updatedMovie = await _moviesRepo.UpdateAsync(existingMovie);

            return Ok(updatedMovie);
        }

    }
}
