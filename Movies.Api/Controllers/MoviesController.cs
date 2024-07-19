using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Dtos;
using Movies.Api.Errors;
using Movies.Api.Helpers;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.BL.Specifications.MovieSpecs;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MoviesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult<Pagination<MovieDetailsDto>>> GetAllAsync([FromQuery] MovieSpecParams specParams)
        {
            var spec = new MovieWithGenreSpecifiations(specParams);
            var movies = await _unitOfWork.GetRepository<Movie>().GetAllWithSpecsAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Movie>, IReadOnlyList<MovieDetailsDto>>(movies);

            var countSpec = new MoviesSpecsForCount(specParams);
            var count = await _unitOfWork.GetRepository<Movie>().GetCountWithSpecAsync(countSpec);

            return Ok(new Pagination<MovieDetailsDto>(specParams.PageIndex, specParams.PageSize, count, data));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult> GetbyIdAsync(int id)
        {
            var spec = new MovieWithGenreSpecifiations(id);
            var existingMovie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (existingMovie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}"));
            }
            return Ok(_mapper.Map<Movie, MovieDetailsDto>(existingMovie));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddMovieAsync(MovieDto movie)
        {
            var isValidGenre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(movie.GenreId);
            if (isValidGenre == null)
                return BadRequest(new ApiResponse(400, "Invalid genre ID!"));

            var mappedMovie = _mapper.Map<MovieDto, Movie>(movie);
            var addedMovie = await _unitOfWork.GetRepository<Movie>().AddAsync(mappedMovie);
            await _unitOfWork.CompleteAsync();
            return Ok(addedMovie);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var spec = new MovieWithGenreSpecifiations(id);
            var movie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (movie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}"));
            }

            _unitOfWork.GetRepository<Movie>().DeleteAsync(movie);
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.Map<Movie, MovieDetailsDto>(movie));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] MovieDto updatedMovieDto)
        {
            var spec = new MovieWithGenreSpecifiations(id);
            var existingMovie = await _unitOfWork.GetRepository<Movie>().GetWithSpecAsync(spec);

            if (existingMovie == null)
            {
                return NotFound(new ApiResponse(404, $"No Movie was found with ID:{id}"));
            }

            var isValidGenre = await _unitOfWork.GetRepository<Movie>().GetByIdAsync(updatedMovieDto.GenreId);
            if (isValidGenre == null)
                return BadRequest(new ApiResponse(400, "Invalid genre ID!"));

            if (updatedMovieDto.PosterUrl == null)
            {
                updatedMovieDto.PosterUrl = existingMovie.PosterUrl;
            }

            _mapper.Map(updatedMovieDto, existingMovie);
            _unitOfWork.GetRepository<Movie>().UpdateAsync(existingMovie);
            await _unitOfWork.CompleteAsync();
            return Ok(existingMovie);
        }
    }
}
