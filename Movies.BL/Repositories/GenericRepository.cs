using Microsoft.EntityFrameworkCore;
using Movies.BL.Interfaces.Repository;
using Movies.DL.Data.Contexts;
using Movies.DL.Models;

namespace Movies.BL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext _context;

        //Ask From ClR Create Object from DbContext 
        public GenericRepository( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
         =>   await _context.Set<T>().ToListAsync();   
     

        public async Task<IEnumerable<T>> GetAllByIdAsync(int id)
        => await _context.Set<T>().Where(entity=>entity.Id == id).ToListAsync();   
        

        public async Task<T?> GetByIdAsync(int id)
        =>   await _context.Set<T>().FindAsync(id);
      
        public async Task<T?> AddAsync(T item)
        {
           await _context.Set<T>().AddAsync(item);   
            _context.SaveChanges(); 
            return item;
        }

        public async Task<T> UpdateAsync(T item)
        {
            _context.Set<T>().Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<T> DeleteAsync(T item)
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
