using System;
using System.Collections.Generic;
using System.Linq;
using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MovieRepository;
using Newtonsoft.Json;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase, IMoviesController
    {
        private static IMovieRepo _movieRepo;
        private static Movie[] _movies;

        public MoviesController()
        {
            _movieRepo = new MovieRepo();
            _movies = JsonConvert.DeserializeObject<Movie[]>(_movieRepo.Get());
            this._sortMovies();
        }

        [HttpGet]
        public Movie[] Get()
        {
            return _movies;
        }

        [HttpGet("{id}")]
        public Movie Get(Guid id)
        {
            return _movies.FirstOrDefault(movie => movie.Id == id);
        }

        [HttpPut]
        public Movie Put([FromBody] Movie movie)
        {
            bool newMovie = false;
            if (movie.Id.Equals(Guid.Empty)) {
                movie.Id = Guid.NewGuid();
                newMovie = true; 
            }
            // Not using returned bool right now. But could be nice for debugging.
            bool successfullyUpdated = this._updateMovies(movie, newMovie);

            return movie;
        }

        public Movie Rate(Guid id, int rating)
        {
            if (id == null) {
                return null;
            }
            if (!(0 <= rating && rating <= 10)) {
                return null;
            }
            Movie movieToRate = _movies.FirstOrDefault(movie => movie.Id == id);
            if (movieToRate == null) {
                return null;
            }

            float ratingBefore = movieToRate.Rating;
            long nrOfRatingsBefore = movieToRate.Ratings;

            long nrOfRatingsAfter = nrOfRatingsBefore + 1;
            float ratingAfter = ((ratingBefore * nrOfRatingsBefore) + rating) / nrOfRatingsAfter;

            movieToRate.Rating = ratingAfter;
            movieToRate.Ratings = nrOfRatingsAfter;

            // Not using returned bool right now. But could be nice for debugging.
            bool successfullyUpdatedMovie = this._updateMovies(movieToRate, false);

            return movieToRate;
        }

        [HttpGet("[action]/{query}")]
        public Movie Search(string query)
        {
            if (query == null) {
                return null;
            }
            var movie = _movies.FirstOrDefault(q => q.Title.ToLower().Contains(query.ToLower()));
            return movie;
        }

        [HttpGet("[action]/{query}")]
        public Movie[] SearchByGenre(string[] query)
        {
            IEnumerable<Movie> matchingMovies = new List<Movie>();
            foreach (string genre in query)
            {
                // Could do a intersect between Genre arrays in order to skip this foreach loop.
                matchingMovies = matchingMovies.Concat(_movies.Where(q => q.Genre.Contains(genre)));
            }
            
            return matchingMovies.ToArray();
        }

        private bool _updateMovies(Movie movie, bool newMovie) {

            string movieString = JsonConvert.SerializeObject(movie);
            bool success = _movieRepo.Put(movieString);
            if (success) {
                if (!newMovie) {
                    _movies = _movies.Where(mov => !mov.Id.Equals(movie.Id)).ToArray();
                }
                _movies = _movies.Append(movie).ToArray();
                this._sortMovies();
            }
            return success;
        }

        private void _sortMovies() {
            Array.Sort(_movies, (a, b) => String.Compare(a.Title, b.Title));
        }
    }
}