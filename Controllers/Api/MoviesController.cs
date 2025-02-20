using AutoMapper;
using System;
using System.Linq;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // Get/api/movies
        public IHttpActionResult GetMovies()
        {

            var movieDto = _context.movies.ToList().Select(Mapper.Map<Movie, MovieDto>);

            return Ok(movieDto);
        }

        // Get/api/movie/1
        [HttpGet]
        public IHttpActionResult GetMovie(int id)
        {

            var movieInDb = _context.movies.SingleOrDefault(x => x.Id == id);

            if (movieInDb == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Movie, MovieDto>(movieInDb));

        }


        //Post/api/movies
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);

            movie.DateAdded = DateTime.Now;
            movie.Id = movieDto.Id;


            _context.movies.Add(movie);
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);

        }

        //Put/api/movie/1
        [HttpPut]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movieInDb = _context.movies.SingleOrDefault(x => x.Id == id);

            if (movieInDb == null)
                return NotFound();

            Mapper.Map(movieDto, movieInDb);
            _context.SaveChanges();


            return Ok();
        }

        //Delete/api/movie/1
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {

            var movieInDb = _context.movies.SingleOrDefault(x => x.Id == id);
            if (movieInDb == null)
            {
                return NotFound();
            }

            _context.movies.Remove(movieInDb);
            _context.SaveChanges();

            return Ok();
        }



    }
}
