using System.Net;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _movieService;
        public MoviesController(IMoviesService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public async Task<ActionResult<MovieResponseDTO>> Create([FromBody] CreateMovieRequestDTO movie)
        {
            var savedMovie = await _movieService.CreateMovieAsync(movie);

            return CreatedAtAction(nameof(GetById), new { Id = savedMovie.Id }, savedMovie);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponseDTO>> GetById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie is null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = $"Movie not found for the given id \"id\"" });
            }

            return new MovieResponseDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Genre = movie.Genre,
                ReleaseDate = movie.ReleaseDate,
                RentalPrice = movie.RentalPrice,
                Stock = movie.Stock
            };
        }

        [HttpGet]
        public async Task<List<MovieResponseDTO>> Get()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            var mappedMovies = movies.Select(movie =>
            {
                return new MovieResponseDTO()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    ReleaseDate = movie.ReleaseDate,
                    RentalPrice = movie.RentalPrice,
                    Stock = movie.Stock
                };
            }).ToList();

            return mappedMovies;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieResponseDTO>> Update(int id, [FromBody] UpdateMovieRequestDTO movie)
        {
            await _movieService.UpdateMovieAsync(id, movie);
            var updatedMovie = await _movieService.GetMovieByIdAsync(id);

            return Ok(new MovieResponseDTO()
            {
                Id = updatedMovie.Id,
                Title = updatedMovie.Title,
                Description = updatedMovie.Description,
                Genre = updatedMovie.Genre,
                ReleaseDate = updatedMovie.ReleaseDate,
                RentalPrice = updatedMovie.RentalPrice,
                Stock = updatedMovie.Stock
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _movieService.DeleteMovieAsync(id);

            return Ok(new { status = HttpStatusCode.OK, message = "Movie has been deleted." });
        }
    }
}