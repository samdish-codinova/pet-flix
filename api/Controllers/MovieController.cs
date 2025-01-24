using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var savedMovie = await _movieService.CreateMovieAsync(movie);

                return CreatedAtAction(nameof(GetById), new { Id = movie.Id }, movie);
            }
            catch (InvalidData)
            {
                return BadRequest("Invalid request to create movie");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie is null)
            {
                return NotFound();
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
            try
            {
                var isUpdated = await _movieService.UpdateMovieAsync(movie);

                return Ok();
            }
            catch (InvalidData)
            {
                return BadRequest("Invalid request to update movie");
            }
            catch (MovieNotFound)
            {
                return NotFound("Movie does not exist.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _movieService.DeleteMovieAsync(id);

                return Ok();
            }
            catch (MovieNotFound)
            {
                return NotFound();
            }

        }
    }
}