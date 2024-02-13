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
        {
            if (typeof(T) == typeof(Genre))
                return (IEnumerable<T>) await _context.Genres.OrderBy(x => x.Name).ToListAsync();

            else
               return (IEnumerable<T>)await _context.Movies.OrderByDescending(c=>c.Rate).Include(x=>x.Genre).ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllByIdAsync(int id)
        {
            if (typeof(T) == typeof(Movie))
                return (IEnumerable<T>)await _context.Movies.Where(m => m.GenreId == id || id == 0).OrderByDescending(c => c.Rate).Include(x => x.Genre).ToListAsync();

            else
            return await _context.Set<T>().Where(entity => entity.Id == id).ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Movie))
                //Include() Method no accessible(accept) FindAsync method and accept FirstOrDefaultAsync()
                return await _context.Set<Movie>().Include(g => g.Genre).FirstOrDefaultAsync(m => m.Id ==id) as T;

            else
                return await _context.Set<T>().FindAsync(id);
        }

       
        public async Task<T?> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            _context.SaveChanges(); // Save changes synchronously

            //Explicitly loads  =>
            if (typeof(T) == typeof(Movie))
            {
                var movie = item as Movie;
                await _context.Entry(movie).Reference(m => m.Genre).LoadAsync();
            }

            return item;
        }

        //public async Task<T?> AddAsync(T item)
        //{
        //   await _context.Set<T>().AddAsync(item);   
        //    _context.SaveChanges(); //change state to added 
        //    return item;
        //}


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

        ///Checks if there is any genre in the database with the provided genre ID and
        ///returns true if such a genre exists, and false otherwise.
        public Task<bool> IsvalidGenre(int genreId) 
            => _context.Genres.AnyAsync(g => g.Id == genreId);
       
        }
    }
