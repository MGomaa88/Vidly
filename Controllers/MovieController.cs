using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;


namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext _context;

        public MovieController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movie
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Random()
        {
            Movie movie = new Movie() { Name = "Shrek!" };
            // create a list of action so we can asign to our randomMovieViewModel object
            var customers = new List<Customer>()
            {new Customer { Id=1, Name="Mohamed Ibrahim"},
                new Customer {Id=2, Name="Mai Gomaa" }
            };

            var viewModel = new RandomMovieViewModel() { Movie = movie, Customers = customers };
            return View(viewModel);


            // return HttpNotFound();
            //return Content("Hello world");
            //return EmptyResult();
            //return RedirectToAction("Name of the action", "The controller", new {page=1, sortBy="name"});
        }
        public ActionResult NewMovie()
        {
            var movieGenre = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = movieGenre
            };

            return View("MovieForm", viewModel);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                   
                    Genres = _context.Genres.ToList(),  


                };

                return View("MovieForm",viewModel);
            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.movies.Add(movie);
            }
            else
            {
                var movieDb = _context.movies.Single(m => m.Id == movie.Id);

                movieDb.Name = movie.Name;
                movieDb.GenreId = movie.GenreId;
                movieDb.NumberInStock = movie.NumberInStock;
                movieDb.ReleaseDate = movie.ReleaseDate;

            }

            _context.SaveChanges();


            return RedirectToAction("Index", "Movie");
        }

        public ActionResult Edit(int id)
        {

            var Movie = _context.movies.SingleOrDefault( m => m.Id== id);

            if (Movie == null)
            {
                return HttpNotFound();
            }

            var movieModel = new MovieFormViewModel(Movie)
            {
                Genres = _context.Genres.ToList(),
              

            };

            return View("MovieForm",movieModel);
        }

      
        public ViewResult Index()
        {
            var movies = _context.movies.Include(m => m.Genre).ToList();

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.movies.Include(c => c.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);

        }

        private IEnumerable<Movie> GetMovies()
        {

            return new List<Movie>
            { new Movie{Id=1, Name="World War Z"},
                new Movie{Id=2, Name="I robot!" }

            };
        }

        [Route("movie/released/{year}/{month:regex(\\d{2})}")]
        public ActionResult ByReleaseDate(int year, int month)
        {

            return Content(year + " / " + month);
        }

        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    if (!pageIndex.HasValue)
        //    {
        //        pageIndex = 1;
        //    }
        //    if (String.IsNullOrEmpty(sortBy))
        //    {
        //        sortBy = "Name";
        //    }
        //    return Content(String.Format("pageIndex={0}&sortBy{1}", pageIndex, sortBy));
        //}
    }
}