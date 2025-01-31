using System.Net;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BusinessLogicLayer
{
  public class MovieService : IMoviesService
  {
    private ApplicationDBContext _dbContext;
    private IUnitOfWork _unitOfWork;

    public MovieService(ApplicationDBContext dBContext, IUnitOfWork unitOfWork)
    {
      _dbContext = dBContext;
      _unitOfWork = unitOfWork;
    }

    public async Task<Movie> CreateMovieAsync(Movie movie)
    {
      if (string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
      {
        throw new ErrorResponseException("Invalid data provided to create a movie.", HttpStatusCode.BadRequest);
      }

      _unitOfWork.Movies.Add(movie);
      _unitOfWork.Complete();

      return movie;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
      return _unitOfWork.Movies.Get(id);
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
      return _unitOfWork.Movies.GetAll().ToList();
    }

    // TODO: Fix and refactor this method.
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
      var movie = _unitOfWork.Movies.Get(id);
      if (movie is null)
      {
        throw new ErrorResponseException($"Movie not found for the given id \"{id}\"", HttpStatusCode.NotFound);
      }

      _unitOfWork.Movies.Remove(movie);
      return _unitOfWork.Complete();
    }
  }
}