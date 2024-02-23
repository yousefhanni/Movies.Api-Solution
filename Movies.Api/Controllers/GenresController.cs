using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Dtos;
using Movies.Api.Errors;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.BL.Specifications.GenreSpecs;
using Movies.DL.Models;

namespace Movies.Api.Controllers
{
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
        public async Task<ActionResult<IReadOnlyList<Genre>>> GetAllAsync(string? sort)
        {
            var spec = new GenreSpecifiations(sort);

            var genres = await _unitOfWork.GetRepository<Genre>().GetAllWithSpecsAsync(spec);
            return Ok(genres);
        } 

        ///Get specific Genre by id 
        [HttpGet("{id}")]
        public async Task<ActionResult>GetbyIdAsync(int id)
        {
            var existingGenre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}"));
            }
            return Ok(existingGenre);
        }

        [HttpPost]
        public async Task<ActionResult> AddGenreAsync(GenreDto genre)
        {        
            //Mapping(Copying) data from GenreDto to Genre
            var mappedGenre = _mapper.Map<GenreDto, Genre>(genre);

            var Genre = await _unitOfWork.GetRepository<Genre>().AddAsync(mappedGenre);

            await _unitOfWork.CompleteAsync();  
            return Ok(Genre);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] GenreDto updatedGenreDto)
        {
            var existingGenre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (existingGenre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}")); //appropriate action if genre with given ID is not found
            }

            _mapper.Map(updatedGenreDto, existingGenre);

           _unitOfWork.GetRepository<Genre>().UpdateAsync(existingGenre);

            await _unitOfWork.CompleteAsync();

            return Ok(existingGenre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var Genre = await _unitOfWork.GetRepository<Genre>().GetByIdAsync(id);

            if (Genre == null)
            {
                return NotFound(new ApiResponse(404, $"No Genre was found with ID:{id}")); //appropriate action if genre with given ID is not found
            }

            _unitOfWork.GetRepository<Genre>().DeleteAsync(Genre);
            await _unitOfWork.CompleteAsync();

            return Ok(Genre);
        }

    }
}
