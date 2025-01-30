using System.Net;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Movie movie)
        {
            var savedMovie = await _movieService.CreateMovieAsync(movie);

            return CreatedAtAction(nameof(GetById), new { Id = movie.Id }, movie);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie is null)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = $"Movie not found for the given id \"id\"" });
            }

            return movie;
        }

        [HttpGet]
        public async Task<List<Movie>> Get()
        {
            return await _movieService.GetAllMoviesAsync();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Movie movie)
        {
            await _movieService.UpdateMovieAsync(movie);

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _movieService.DeleteMovieAsync(id);

            return Ok(new { status = HttpStatusCode.OK, message = "Movie has been deleted." });
        }
    }
}