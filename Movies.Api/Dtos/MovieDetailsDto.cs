using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Dtos
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        public string? StoryLine { get; set; }//it's the sequence of events that unfolds in the movie

        public string PosterUrl { get; set; }

        public int GenreId { get; set; }

        public string? GenreName { get; set; }
    }
}
