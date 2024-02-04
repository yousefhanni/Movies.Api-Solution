using System.ComponentModel.DataAnnotations;

namespace Movies.DL.Models
{
    public class Movie:BaseModel
    {
     
        public string? Title { get; set; }  

        public int Year { get; set; }

        public double Rate { get; set; }

        public string? StoryLine { get; set; }//it's the sequence of events that unfolds in the movie
                                              
        public string PosterUrl { get; set; }    

        public int GenreId { get; set; } //F.K

        public Genre Genre { get; set; }      //N.P[ONE]

    }
}
