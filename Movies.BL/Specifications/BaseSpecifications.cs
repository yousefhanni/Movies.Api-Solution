using Movies.BL.Interfaces;
using Movies.DL.Models;
using System.Linq.Expressions;


namespace Movies.BL.Specifications
{
    ///This class is designed to be a base class(Common\Container class for all Models) for creating specifications
    ///that can be used for querying data with criteria, includes, ordering, and pagination

    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        //Query=>Set<T>().Where(p=>p.Id==7).OrderBy(p=>p.Rate).Include(P=>P.Genre);

        //Used for filtering data.
        public Expression<Func<T, bool>> Criteria { get; set; } 

        ///The Includes property is a list of include expressions,
        ///indicating related entities that should be loaded with the main entity
        ///Initialize Includes => Empty List
        public List<Expression<Func<T, object>>> Includes { get ; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get ; set ; }

        public Expression<Func<T, object>> OrderByDesc { get ; set ; }

        public int Skip { get; set; }

        public int Take { get; set; }

        ///If true(Apply Pagination) => Set skip and Take with The value
        ///and If false set skip and Take with Default value => (Zero)
        public bool IsPaginationEnabled { get; set; }


        ///When Create object form Parameterless Cons, new keyword will allocate object + initial value of Propery(null) and excute Cons,
        ///but must be (Includes) are not null must be Empty list => 
        ///because must point on object at heap to can put inside it expressions that you want to be included(Genres)       
        ///So, You must initialize to Includes with Empty List => To Can Add inside this list 
        ///This Cons => (Get All Movies) 
        ///Use cons to Create object from BaseSpecifications and Set Criteria with null and build Query that will Get all Movies 
        public BaseSpecifications()
        {
            // OrderByDesc/OrderBy/Criteria = null;
             //Includes = Empty List ;
        }


        ///This Cons => (Get Specific Movie OR Specific Movies(if Get All movies by genreId))
        ///Use cons to Create object from BaseSpecifications and Set Criteria with value and build Query Get Specific Movie based on Id
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;   
                                             
        }


        //Two Methods act as Setter to OrderBy/Desc 
        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression; //c => c.Rate
        }

        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {
            OrderByDesc = OrderByDescExpression;
        }

        //To Apply Pagination must be call Method 
        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }

    }
}
