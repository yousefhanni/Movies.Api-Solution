
namespace Movies.DL.Models
{
    public class Genre : BaseModel
    {
        public string? Name { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); //N.P[Many]
    }
}
