using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Consts;
using Movies.Api.Dtos;
using Movies.BL.Interfaces.Repository;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
    public class GenresController : ApiBaseController
    {
        private readonly IGenericRepository<Genre> _genresRepo;
        private readonly IMapper _mapper;

        public GenresController(IGenericRepository<Genre> genresRepo, IMapper mapper)
        {
            _genresRepo = genresRepo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllAsync()
        {
            var genres = await _genresRepo.GetAllAsync();
            return Ok(genres);
        } 

        ///Get specific Genre by id 
        [HttpGet("{id}")]
        public async Task<ActionResult>GetbyIdAsync(int id)
        {
            var existingGenre = await _genresRepo.GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound($"No genre was found with ID:{id}");
            }
            return Ok(existingGenre);
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
