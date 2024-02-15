using Movies.BL.Interfaces;
using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BL.Specifications
{
    ///This class is designed to be a base class(Common\Container class for all Models) for creating specifications
    ///that can be used for querying data with criteria, includes, ordering, and pagination

    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        //Query=>Set<T>().Where(p=>p.Id==7).Include(P=>P.Genre).OrderBy(p=>p.Name);

        //Used for filtering data.
        public Expression<Func<T, bool>> Criteria { get; set; }  

        ///The Includes property is a list of include expressions,
        ///indicating related entities that should be loaded with the main entity
        ///Initialize Includes => Empty List
        public List<Expression<Func<T, object>>> Includes { get ; set; } = new List<Expression<Func<T, object>>>();


        ///When Create object form Parameterless Cons, new keyword will allocate object + initial value of Propery(null) and excute Cons,
        ///but must be (Includes) are not null must be Empty list => 
        ///because must point on object at heap to can put inside it expressions that you want to be included(Genres)       
        ///So, You must initialize to Includes with Empty List => To Can Add inside this list 


        ///This Cons => (Get All Movies) 
        ///Use cons to Create object from BaseSpecifications and Set Criteria with null and build Query that will Get all Movies 
        public BaseSpecifications()
        {
            // Criteria = null;
        }


        ///This Cons => (Get Specific Movie)
        ///Use cons to Create object from BaseSpecifications and Set Criteria with value and build Query Get Specific Movie based on Id
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;    //(p => p.Id == 7)
        }

    }
}
