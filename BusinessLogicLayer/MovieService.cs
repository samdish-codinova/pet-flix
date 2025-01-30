using System.Net;
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
        throw new ErrorResponseException("Invalid data provided to create a movie.", HttpStatusCode.BadRequest);
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
        throw new ErrorResponseException("Invalid data provided to update the movie.", HttpStatusCode.BadRequest);
      }
      var movieInDb = await _dbContext.Movie.FirstOrDefaultAsync(m => m.Id == movie.Id);
      if (movieInDb is null)
      {
        throw new ErrorResponseException($"Movie not found for the given id \"{movie.Id}\"", HttpStatusCode.NotFound);
      }

      _dbContext.Movie.Update(movie);

      return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteMovieAsync(int id)
    {
      var movie = await GetMovieByIdAsync(id);
      if (movie is null)
      {
        throw new ErrorResponseException($"Movie not found for the given id \"{id}\"", HttpStatusCode.NotFound);
      }

      _dbContext.Movie.Remove(movie);
      return await _dbContext.SaveChangesAsync();
    }
  }
}