using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    public class CreateMovieBody
    {
        public string Title;
        public decimal Price;
    }
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ApplicationDBContext _dbContext;
        public MovieController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<List<Movie>> Get()
        {
            return await _dbContext.Movie.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Movie> GetById(int id)
        {
            return await _dbContext.Movie.FirstOrDefaultAsync(movie => movie.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Movie movie)
        {
            Console.WriteLine("movie data: {0}", movie.Title);

            if (string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
            {
                return BadRequest("Invalid Request");
            }

            await _dbContext.Movie.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { Id = movie.Id }, movie);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Movie movie)
        {
            if (movie.Id == 0 || string.IsNullOrEmpty(movie.Title) || string.IsNullOrEmpty(movie.Description) || string.IsNullOrEmpty(movie.Genre))
            {
                return BadRequest("Invalid Request");
            }
            _dbContext.Movie.Update(movie);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            var movie = await GetById(id);
            if (movie is null) {
                return NotFound();
            }

            _dbContext.Movie.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}