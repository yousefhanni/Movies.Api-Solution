using Movies.DL.Models;


namespace Movies.BL.Specifications.MovieSpecs
{
    //This class will Add values of Specifications to Get Methods that will pass this Specifications to(GetQuery)mehtod that will build query with Dynamic way
    public class MovieWithGenreSpecifiations:BaseSpecifications<Movie>
    {
        ///This Constructor will chain on Empty Parameterless Constructor
        ///that used for Creating an Object,That Will be used to Get All Movies
        public MovieWithGenreSpecifiations( MovieSpecParams specParams)
            :base(g => //Criteria(filtering)
                  //if (!specParams.GenreId.HasValue) is True will Get All movies and if part 02 is true will Get all Movies that have the same genreId
                   (!specParams.GenreId.HasValue || g.GenreId == specParams.GenreId.Value) &&
                  (string.IsNullOrEmpty(specParams.Search)|| g.Title.ToLower().Contains(specParams.Search))//Searching
                                             
            )

        {
            Includes.Add(g=>g.Genre);  

             
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "YearAsc":
                        AddOrderBy(c=>c.Year); 
                        break;
                    case "YearDesc":
                        AddOrderByDesc(c => c.Year);
                        break;
                    case "TitleAsc":
                        AddOrderBy(c => c.Title);
                        break;
                    case "TitleDesc":
                        AddOrderBy(c => c.Title); 
                        break;
                    case "GenreNameAsc":
                        AddOrderBy(c => c.Genre.Name);
                        break;
                    case "GenreNameDesc":
                        AddOrderBy(c => c.Genre.Name);
                        break;
                    default:
                        AddOrderByDesc(c=>c.Rate);
                        break;
                }
            } 

            else 
                AddOrderByDesc(c=> c.Rate);


            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }


        ///This Constructor will chain on Constructor that will Set Criteria
        ///That used for Creating an Object,That Will be used to Get a Specific movie with by id
        public MovieWithGenreSpecifiations(int id)
            :base(m=>m.Id==id)    // Criteria = (m => m.Id==7)
        {
            Includes.Add(g => g.Genre);
        }




    }
}
