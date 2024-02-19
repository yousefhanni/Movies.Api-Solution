﻿using Microsoft.EntityFrameworkCore;
using Movies.BL.Interfaces;
using Movies.BL.Interfaces.Repository;
using Movies.BL.Specifications;
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

        public async Task<IReadOnlyList<T>> GetAllAsync()
         =>  await _context.Set<T>().ToListAsync();


        public async Task<T?> GetByIdAsync(int id)
         =>  await _context.Set<T>().FindAsync(id);
        

        public async Task<IReadOnlyList<T>> GetAllWithSpecsAsync(ISpecifications<T> spec)
         =>  await ApplySpecifications(spec).ToListAsync();
        

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
         =>   await ApplySpecifications(spec).FirstOrDefaultAsync();
       

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
         =>   await ApplySpecifications(spec).CountAsync();
      
           
        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
         =>   SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), spec);
   

        public async Task<T?> AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            _context.SaveChanges(); // Save changes synchronously

            //Explicitly loads  => To load Genre at response 
            if (typeof(T) == typeof(Movie))
            {
                var movie = item as Movie;
                await _context.Entry(movie).Reference(m => m.Genre).LoadAsync();
            }

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

        ///Checks if there is any genre in the database with the provided genre ID and
        ///returns true if such a genre exists, and false otherwise.
        public Task<bool> IsvalidGenre(int genreId) 
            => _context.Genres.AnyAsync(g => g.Id == genreId);
    
    }
    }
