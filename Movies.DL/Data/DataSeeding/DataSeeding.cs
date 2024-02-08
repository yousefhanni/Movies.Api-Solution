using Movies.DL.Data.Contexts;
using Movies.DL.Models;
using System.Text.Json;

namespace Movies.DL.Data.DataSeeding
{
    public static class DataSeeding
    {
        //Why use _dbContext ? => to Seed data you must to add Database
        public async static Task SeedAsync(ApplicationDbContext _dbContext)
        {
            ///Dataseeding occurs 1 time only per Server 
            ///So,Check if there are no Genres in the database 

            if (_dbContext.Genres.Count() == 0)
            {
                // Read Genres data from a JSON file
                var GenresData = File.ReadAllText("../Movies.DL/Data/DataSeeding/Genres.json");

                //Deserialize(Convert) from Json file(JavaScript) to List of C# objects(GenresData)
                var Genres = JsonSerializer.Deserialize<List<Genre>>(GenresData);

                // Check if there are any Genres to add
                if (Genres?.Count() > 0)
                {
                    // Iterate through the GenresData and add them to the database
                    foreach (var Genre in Genres)
                    {
                        _dbContext.Set<Genre>().Add(Genre);

                    }
                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();
                }
            }


            if (_dbContext.Movies.Count() == 0)
            {
                var MoviesData = File.ReadAllText("../Movies.DL/Data/DataSeeding/Movies.json");

                var Movies = JsonSerializer.Deserialize<List<Movie>>(MoviesData);

                if (Movies?.Count() > 0)
                {

                    foreach (var Movie in Movies)
                    {
                        _dbContext.Set<Movie>().Add(Movie);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }

}