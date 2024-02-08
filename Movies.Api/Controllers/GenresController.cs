using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Consts;
using Movies.Api.Dtos;
using Movies.BL.Interfaces.Repository;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenericRepository<Genre> _genresRepo;
        private readonly IMapper _mapper;

        public GenresController(IGenericRepository<Genre> genresRepo, IMapper mapper)
        {
            _genresRepo = genresRepo;
            _mapper = mapper;
        }

        //Get all With Sorting Criteria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllAsync()
        {
            // Pass a lambda expression to specify sorting by genre name and pass orderDirection to specify type of sorting
            var genres = await _genresRepo.GetAllAsync(g => g.Name, Orderby.Ascending);
            return Ok(genres);
        }


        [HttpPost]
        public async Task<ActionResult> AddGenreAsync(GenreDto genre)
        {
            //Mapping(Copying) data from GenreDto to Genre
            var mappedGenre = _mapper.Map<GenreDto, Genre>(genre);

            var Genre = await _genresRepo.AddAsync(mappedGenre);

            return Ok(Genre);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] GenreDto updatedGenreDto)
        {
            var existingGenre = await _genresRepo.GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound($"No genre was found with ID:{id}"); //appropriate action if genre with given ID is not found
            }

            _mapper.Map(updatedGenreDto, existingGenre);

            var updatedGenre = await _genresRepo.UpdateAsync(existingGenre);

            return Ok(updatedGenre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var Genre = await _genresRepo.GetByIdAsync(id);

            if (Genre == null)
            {
                return NotFound($"No genre was found with ID:{id}"); //appropriate action if genre with given ID is not found
            }

            var deletedGenre = await _genresRepo.DeleteAsync(Genre);

            return Ok(deletedGenre);
        }

    }
}
