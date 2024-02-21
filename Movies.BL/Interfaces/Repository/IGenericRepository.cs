using Microsoft.EntityFrameworkCore;
using Movies.DL.Models;
namespace Movies.BL.Interfaces.Repository
{
    ///Why used BaseModel not Class keyword ? 
    ///Because T must be Domain Model and doesn't any class may is Domain Model(which does not use Class keyword )
    ///so i use BaseModel as Domain Model,T will is BaseModel and  any class will inherit from BaseModel

    public interface IGenericRepository<T> where T : BaseModel
    { 
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec);

        Task<T?> GetByIdAsync(int id);

        Task<T> GetWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);

        Task<T?> AddAsync(T item);

        void UpdateAsync(T entity);

        void DeleteAsync(T entity);

        Task<bool> IsvalidGenre(int genreId);
    }
}
