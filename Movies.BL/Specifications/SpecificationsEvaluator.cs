using Microsoft.EntityFrameworkCore;
using Movies.BL.Interfaces;
using Movies.DL.Models;

namespace Movies.BL.Specifications
{
    ///This Class will contain on method that Will build query with Dynamic way(Logic of Query)
    ///query => _dbcontext.Set<Movie>().Where(P => P.Id == id).Include(p => p.Genre) 

    public static class SpecificationsEvaluator<T> where T : BaseModel
    { 
        //build query with Dynamic way
        public static IQueryable<T> GetQuery(IQueryable<T> startQuery, ISpecifications<T> spec)
        {
            var query = startQuery;  //_dbContext.Set<Movie>()

        //Make Filteration than sorting then Pagination because in Sqlserver (Where) executed at the first then (Orderby) then Top(Skip and take)

            //set criteria 
            if (spec.Criteria is not null)
                query =query.Where(spec.Criteria); //query= _dbContext.Set<Movie>().Where(m=>m.Id==7)


            if(spec.OrderBy is not null ) //c => c.Rate
                query = query.OrderBy(spec.OrderBy); //query= _dbContext.Set<Movie>().OrderBy(c => c.Rate)

            else if(spec.OrderByDesc is not null )
                query = query.OrderByDescending(spec.OrderByDesc); //query= _dbContext.Set<Movie>().OrderByDesc(c => c.Rate)


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            //add the list of includes     //Set<T>().Where(p=>p.Id==7).OrderBy(p=>p.Rate).Skip(10).Take(5).Include(P=>P.Genre);
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            //query= _dbContext.Set<Movie>().OrderByDesc(c => c.Rate).Include(g=>g.Genre);

            return query;


        }

    }
}
