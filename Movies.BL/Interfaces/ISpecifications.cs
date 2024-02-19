using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Interfaces
{
    ///This interface that define shape of Specification object(Parameter of Method)
    ///that i will send it to Method as Parameter,this method that take (Dbset,Object from any class that implement this interface) 
    ///Specification => Query that will run against DBset<T> Which (T) must be Entity
    ///For Examaple about Two Specifications => (where and include,Order by,Take,Skip,...) 
    ///Inside This interface Make=>Property signatures for each Specification and each Property carry value of Spec(where or inlcude or etc..)

    public interface ISpecifications<T> where T : BaseModel
    {
        //Query=>Set<T>().Where(p=>p.Id==7).Include(P=>P.Genre).OrderBy(p=>p.Name);

        //Criteria=> that carry value that will be sent to (Where)
        public Expression<Func<T, bool>> Criteria { get; set; }

        //There More than (include) => so, i will use list 
        public List<Expression<Func<T, object>>> Includes { get; set; }

        //Two  Properties to Sorting 
        public Expression<Func<T, object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        ///If true(Apply Pagination) => Set skip and Take with The value
        ///and If false set skip and Take with Default value => (Zero)
        public bool IsPaginationEnabled { get; set; }

    }
}
