using BusinessLogicLayer;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BusinessLogicLayer
{
  public class MovieService : IMovieService
  {
    private ApplicationDBContext _dbContext;

    public MovieService(ApplicationDBContext dBContext)
    {
      _dbContext = dBContext;
    }

    public async Task<Movie> CreateMovieAsync(Movie movie)
    {
      if (string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
      {
        throw new InvalidData();
      }

      await _dbContext.Movie.AddAsync(movie);
      await _dbContext.SaveChangesAsync();

      return movie;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
      return await _dbContext.Movie.FirstOrDefaultAsync(movie => movie.Id == id);
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
      return await _dbContext.Movie.ToListAsync();
    }

    public async Task<int> UpdateMovieAsync(Movie movie)
    {

      if (movie.Id == 0 || string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
      {
        throw new InvalidData();
      }
      var movieInDb = await _dbContext.Movie.FirstOrDefaultAsync(m => m.Id == movie.Id);
      if (movieInDb is null)
      {
        throw new MovieNotFound();
      }

      _dbContext.Movie.Update(movie);

      return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteMovieAsync(int id)
    {
      var movie = await GetMovieByIdAsync(id);
      if (movie is null)
      {
        throw new MovieNotFound();
      }

      _dbContext.Movie.Remove(movie);
      return await _dbContext.SaveChangesAsync();
    }
  }
}