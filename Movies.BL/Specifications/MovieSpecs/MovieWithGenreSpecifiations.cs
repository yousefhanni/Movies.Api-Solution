using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications.MovieSpecs
{
    //This class will Add values of Specifications to Get Methods that will pass this Specifications to(GetQuery)mehtod that will build query with Dynamic way
    public class MovieWithGenreSpecifiations:BaseSpecifications<Movie>
    {
        ///This Constructor will chain on Empty Parameterless Constructor
        ///that used for Creating an Object,That Will be used to Get All Movies
        public MovieWithGenreSpecifiations():base()
        {
            Includes.Add(g=>g.Genre); // Criteria = null;
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
