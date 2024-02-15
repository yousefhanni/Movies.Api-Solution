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

            CreateMap<MovieDto, Movie>().ReverseMap()
              //.ForMember(d => d.PosterUrl, m => m.MapFrom(s =>$"https://localhost:7038/{s.PosterUrl}"));
                .ForMember(d => d.PosterUrl, O => O.MapFrom<MoviePitureUrlResolver>());

            CreateMap<MovieDetailsDto, Movie>().ReverseMap();


        }
    }
}
