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

    public async Task<Movie> CreateMovieAsync(CreateMovieRequestDTO movie)
    {
      if (string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
      {
        throw new ErrorResponseException("Invalid data provided to create a movie.", HttpStatusCode.BadRequest);
      }

      var newMovie = new Movie()
      {
        Title = movie.Title,
        Description = movie.Description,
        Genre = movie.Genre,
        ReleaseDate = movie.ReleaseDate,
        RentalPrice = movie.RentalPrice,
        Stock = movie.Stock
      };

      _unitOfWork.Movies.Add(newMovie);
      _unitOfWork.Complete();

      return newMovie;
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
      return _unitOfWork.Movies.Get(id);
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
      return _unitOfWork.Movies.GetAll().ToList();
    }

    public async Task<int> UpdateMovieAsync(int id, UpdateMovieRequestDTO movie)
    {

      if (id <= 0)
      {
        throw new ErrorResponseException("Invalid data provided to update the movie.", HttpStatusCode.BadRequest);
      }
      var movieInDb = _unitOfWork.Movies.Get(id);
      if (movieInDb is null)
      {
        throw new ErrorResponseException($"Movie not found for the given id \"{id}\"", HttpStatusCode.NotFound);
      }

      if (movie.Title != null)
      {
        if (string.IsNullOrWhiteSpace(movie.Title))
        {
          throw new ErrorResponseException("Please specify the correct title.", HttpStatusCode.BadRequest);
        }
        movieInDb.Title = movie.Title;
      }

      if (movie.Description != null)
      {
        if (string.IsNullOrWhiteSpace(movie.Description))
        {
          throw new ErrorResponseException("Please specify the correct description.", HttpStatusCode.BadRequest);
        }
        movieInDb.Description = movie.Description;
      }

      if (movie.Genre != null)
      {
        if (string.IsNullOrWhiteSpace(movie.Genre))
        {
          throw new ErrorResponseException("Please specify the correct genre.", HttpStatusCode.BadRequest);
        }
        movieInDb.Genre = movie.Genre;
      }

      if (movie.ReleaseDate != null)
      {
        movieInDb.ReleaseDate = (DateTime)movie.ReleaseDate;
      }

      if (movie.RentalPrice != null)
      {
        if (movie.RentalPrice <= 0)
        {
          throw new ErrorResponseException("Please specify a valid rental price.", HttpStatusCode.BadRequest);
        }
        movieInDb.RentalPrice = (decimal)movie.RentalPrice;
      }

      _dbContext.Movie.Update(movieInDb);

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