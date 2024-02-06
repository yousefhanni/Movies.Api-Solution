using Movies.DL.Models;

namespace Movies.BL.Interfaces.Repository
{
    ///Why used BaseModel not Class keyword ? 
    ///Because T must be Domain Model and doesn't any class may is Domain Model(which does not use Class keyword )
    ///so i use BaseModel as Domain Model,T will is BaseModel and  any class will inherit from BaseModel

    public interface IGenericRepository<T> where T : BaseModel
    {
       Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllByIdAsync(int id);

        Task<T?> GetByIdAsync(int id);

        Task<T?> AddAsync(T item);

        Task<T> UpdateAsync(T item);    

        Task<T> DeleteAsync(T item);
    }
}
