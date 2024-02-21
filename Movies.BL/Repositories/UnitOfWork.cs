using Movies.BL.Interfaces.Repository;
using Movies.BL.Interfaces.UnitOfWork;
using Movies.DL.Data.Contexts;
using Movies.DL.Models;
using System.Collections;

namespace Movies.BL.Repositories
{
    //Class Responsible for deal with Database through Dbcontext
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        ///(Dictionary/Hashtable):Store in it repositories that will request at request,
        ///To if you needed again through request don't create same object with same request .

        private Hashtable _repositories;

        //Ask From ClR Create Object from DbContext 
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            _repositories=new Hashtable();
        }

        //Use This method to (Create or retrieve object of GenericRepository per Request for specific entity types.) 
        public IGenericRepository<T> GetRepository<T>() where T : BaseModel
        {
          var key = typeof(T).Name; //Movie

          //Which if repository first time required   
            if (!_repositories.ContainsKey(key))
            {
                //=> (Create object of GenericRepository of specific entity type per Request) 
                var repository = new GenericRepository<T>(_dbContext);
                 
                _repositories.Add(key, repository);
            }
            return _repositories[key] as IGenericRepository<T>;
        }

        //Save changes one time for all operations(ensures that all changes made across different parts of your application are saved together as a group. If any part of the group fails, none of the changes are saved, preserving the consistency of your database.)
        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync(); 

        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

    }
}
