using Microsoft.EntityFrameworkCore;
using Movies.BL.Interfaces;
using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications
{
    ///This Class will contain on method that Will build query with Dynamic way
    ///query => _dbcontext.Set<Movie>().Where(P => P.Id == id).Include(p => p.Genre) 

    public static class SpecificationsEvaluator<T> where T : BaseModel
    { 
        //build query with Dynamic way
        public static IQueryable<T> GetQuery(IQueryable<T> startQuery, ISpecifications<T> spec)
        {

            var query = startQuery;  //_dbContext.Set<Movie>()

            //set criteria 
            if (spec.Criteria is not null)
                query =query.Where(spec.Criteria); //query= _dbContext.Set<Movie>().Where(m=>m.Id==7)


            //add the list of includes
          query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));


            return query;


        }

    }
}
