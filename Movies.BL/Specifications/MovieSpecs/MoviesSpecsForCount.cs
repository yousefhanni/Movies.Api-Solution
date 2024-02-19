using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications.MovieSpecs
{
    ///Get count => If exist Filteration=> Get Count Data After Filteration but before pagination
    ///and if does not exist Filertation will Get All Data before pagination
    public class MoviesSpecsForCount:BaseSpecifications<Movie>
    {
        public MoviesSpecsForCount(MovieSpecParams specParams)
            :base(g =>
                  (string.IsNullOrEmpty(specParams.Search) || g.Title.ToLower().Contains(specParams.Search)) //Searching
                 && (!specParams.GenreId.HasValue || g.GenreId == specParams.GenreId.Value)  //Criteria(Filtering)
            )
        {

         }
    }
}
