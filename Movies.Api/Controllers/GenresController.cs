using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Dtos;
using Movies.Api.Errors;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.BL.Specifications.GenreSpecs;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    [Authorize]
    public class GenresController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenresController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult<IReadOnlyList<Genre>>> GetAllAsync(string? sort)
        {
            var spec = new GenreSpecifiations(sort);
            var genres = await _unitOfWork.GetRepository<Genre>().GetAllWithSpecsAsync(spec);
            return Ok(genres);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult> GetbyIdAsync(int id)
        {
            var existingGenre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}"));
            }
            return Ok(existingGenre);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddGenreAsync(GenreDto genre)
        {
            var mappedGenre = _mapper.Map<GenreDto, Genre>(genre);
            var genreEntity = await _unitOfWork.GetRepository<Genre>().AddAsync(mappedGenre);
            await _unitOfWork.CompleteAsync();
            return Ok(genreEntity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] GenreDto updatedGenreDto)
        {
            var existingGenre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}"));
            }

            _mapper.Map(updatedGenreDto, existingGenre);
            _unitOfWork.GetRepository<Genre>().UpdateAsync(existingGenre);
            await _unitOfWork.CompleteAsync();
            return Ok(existingGenre);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var genre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}"));
            }

            _unitOfWork.GetRepository<Genre>().DeleteAsync(genre);
            await _unitOfWork.CompleteAsync();
            return Ok(genre);
        }
    }
}
