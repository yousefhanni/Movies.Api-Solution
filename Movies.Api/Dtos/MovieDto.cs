using Movies.DL.Models;
using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Dtos
{
    public class MovieDto
    {
        [MaxLength(250)]
        [Required]
        public string? Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        [Required]
        public string? StoryLine { get; set; }//it's the sequence of events that unfolds in the movie

        public int GenreId { get; set; }

        public string? PosterUrl { get; set; }  
         
    }
}
