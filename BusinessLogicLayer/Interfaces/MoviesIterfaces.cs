using Models;

namespace BusinessLogicLayer
{
    public interface IMoviesService
    {
        Task<List<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> CreateMovieAsync(CreateMovieRequestDTO movie);
        Task<int> UpdateMovieAsync(int id, UpdateMovieRequestDTO movie);
        Task<int> DeleteMovieAsync(int id);
    }
}
