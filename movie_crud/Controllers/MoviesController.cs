using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movie_crud.Migrations;
using movie_crud.Models;
using movie_crud.ViewModels;
using NToastNotify;

namespace movie_crud.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }
        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieFormViewModel
            {
                Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync()
            };
            
            return View("MovieForm",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var files = Request.Form.Files;

            if (!files.Any())
            {
                model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View("MovieForm", model);
            }
            var poster = files.FirstOrDefault();
           
            if(!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower())) 
            {
                model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View("MovieForm", model);
            }
            if(poster.Length > _maxAllowedPosterSize)
            {
                model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "poster cannot be more than 1 MB!");
                return View("MovieForm", model);
            }

            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);
            var movies = new Movie
            {
                Title = model.Title,
                GenereId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                Storyline = model.Storyline,
                Poster = dataStream.ToArray(),
            };
            _context.Movies.Add(movies);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie created successfully");
            return RedirectToAction(nameof(Index));

        }
      public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();
            var viewModel = new MovieFormViewModel
            {
                Id=movie.Id,
                Title=movie.Title,
                GenreId=movie.GenereId,
                Rate =movie.Rate,
                Year = movie.Year,
                Storyline =movie.Storyline,
                Poster = movie.Poster,
                Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync()
            };
            return View("MovieForm",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var movie = await _context.Movies.FindAsync(model.Id);

            if (movie == null)
                return NotFound();
            var files = Request.Form.Files;
            if(files.Any())
            {
                var poster = files.FirstOrDefault();
                using var dataStream =new MemoryStream();
                await poster.CopyToAsync(dataStream);
                model.Poster = dataStream.ToArray();
                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Please select movie poster!");
                    return View("MovieForm", model);
                }
                if (poster.Length > _maxAllowedPosterSize)
                {
                    model.Generes = await _context.Generes.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "poster cannot be more than 1 MB!");
                    return View("MovieForm", model);
                }
                movie.Poster = model.Poster;
            }

            movie.Title = model.Title;
            movie.GenereId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.Storyline = model.Storyline;

            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Movie updated successfully");

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.Include(m => m.Genere).SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok();
        }
    }
    
}
