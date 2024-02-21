using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications.GenreSpecs
{
    public class GenreSpecifiations : BaseSpecifications<Genre>
    {
        public GenreSpecifiations(string? sort)
            :base()
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "NameAsc":
                        AddOrderBy(c => c.Name);
                        break;
                    case "NameDesc":
                        AddOrderByDesc(c => c.Name);
                        break;
                    default:
                        AddOrderBy(c => c.Name);
                        break;
                }
            }

            else
                AddOrderBy(c => c.Name);
        }

    }
}
