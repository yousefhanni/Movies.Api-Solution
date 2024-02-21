using Movies.BL.Interfaces.Repository;
using Movies.DL.Models;

namespace Movies.BL.Interfaces.UnitOfWork
{
    public  interface IUnitOfWork:IAsyncDisposable
    {
      //Signature for Generic Method(This method intended to provide access to repositories for different entity types)
        IGenericRepository<T>GetRepository<T>() where T : BaseModel;

        //simulate the behavior of the SaveChanges method found in DbContext classes, returns the number of affected rows after saving changes to the database.  
        Task<int> CompleteAsync();
    }
}
