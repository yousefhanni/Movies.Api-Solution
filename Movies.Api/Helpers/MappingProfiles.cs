using AutoMapper;
using Movies.Api.Dtos;
using Movies.DL.Models;

namespace Movies.Api.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<GenreDto, Genre>();
        }
    }
}
