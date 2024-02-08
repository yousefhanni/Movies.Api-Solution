using Microsoft.EntityFrameworkCore;
using Movies.Api.Consts;
using Movies.BL.Interfaces.Repository;
using Movies.DL.Data.Contexts;
using Movies.DL.Models;
using System.Linq.Expressions;

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

        //Get all With No Criteria
        // public async Task<IEnumerable<T>> GetAllAsync()
        //=>  await _context.Set<T>().ToListAsync();  

        //Get all With Sorting Criteria
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> orderby = null, string orderByDirection = Orderby.Ascending)
        {
            IQueryable<T> query = _context.Set<T>();

            if (orderby != null)
            {
                if(orderByDirection == Orderby.Ascending)
                query = query.OrderBy(orderby);
                else
                    query = query.OrderByDescending(orderby);
            }

            return await query.ToListAsync();
        }



        public async Task<IEnumerable<T>> GetAllByIdAsync(int id)
        => await _context.Set<T>().Where(entity=>entity.Id == id).ToListAsync();   
        

        public async Task<T?> GetByIdAsync(int id)
        =>   await _context.Set<T>().FindAsync(id);
      
        public async Task<T?> AddAsync(T item)
        {
           await _context.Set<T>().AddAsync(item);   
            _context.SaveChanges(); //change state to added 
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
