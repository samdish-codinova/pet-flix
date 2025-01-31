using Models;

namespace BusinessLogicLayer
{
    public interface IMoviesService
    {
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> CreateMovieAsync(CreateMovieRequestDTO movie);
        Task<int> UpdateMovieAsync(Movie movie);
        Task<int> DeleteMovieAsync(int id);
    }
}
