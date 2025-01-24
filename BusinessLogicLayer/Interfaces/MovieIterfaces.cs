using Models;

namespace BusinessLogicLayer
{
    public interface IMovieService
    {
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<int> UpdateMovieAsync(Movie movie);
        Task<int> DeleteMovieAsync(int id);
    }
}
