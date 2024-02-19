using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications.MovieSpecs
{
    ///Data Binding:Clients often send data to the server as part of the URL query string. 
    ///and using [FromQuery], ASP.NET Core will automatically bind the value of genreId OR sort
    ///from the query string to the specParams parameter in the GetAllAsync method.
    public class MovieSpecParams
    {
        private const int MaxPageSize = 10; 

        //(Fullproperty) => To Can Controle(Validate) on pageSize
        private int pageSize = 5; // Default value
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        
        public int PageIndex { get; set; } = 1; //Default value

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public string? Sort { get; set; }

        public int? GenreId { get; set; }

    }
}
