using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Dtos
{
    public class GenreDto
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
