using AutoMapper;
using Movies.Api.Dtos;
using Movies.DL.Models;

namespace Movies.Api.Helpers
{

    public class MoviePitureUrlResolver : IValueResolver<Movie, MovieDto, string?>
    {
        private readonly IConfiguration _configuration;

        public MoviePitureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Movie source, MovieDto destination, string? destMember, ResolutionContext context)
        {
            //if PosterUrl exist 
            if (!string.IsNullOrEmpty(source.PosterUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.PosterUrl}";

            return string.Empty;
        }
    }
}
