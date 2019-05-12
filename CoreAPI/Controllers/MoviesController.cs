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
        private readonly List<Movie> _movies;

        public MoviesController()
        {
            _movieRepo = new MovieRepo();
            _movies = JsonConvert.DeserializeObject<List<Movie>>(_movieRepo.Get()).ToList();
            // This is the only time we get the movies from the database. After that we hold them in a list which doesnt really feel optimal... 
            // You could add a function that checks if _movies is null/empty before we do anything with it. If thats the case we can
            // call from the database again.
            // You could also make some checks here that the database seems ok. Meaning no movies with the same title or the same guid. 
            // There is some ux considerations regarding that. Should it really crash for a user in those cases? Is it better to just add a log.warning?
        }

        [HttpGet]
        public Movie[] Get()
        {
            return _movies.ToArray();
        }

        [HttpGet("{id}")]
        public Movie Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            var listOfMoviesWithMatchingId = _movies.FindAll(movie => movie.Id == id);
            if (listOfMoviesWithMatchingId.Count > 1)
            {
                throw new Exception($"The database has more than one movie with this Guid: {id}");
            }
            return listOfMoviesWithMatchingId.Count == 0 ? null : listOfMoviesWithMatchingId.First();
        }

        [HttpPut]
        public Movie Put([FromBody] Movie movie)
        {
            if (movie.Id == Guid.Empty)
            {
                movie.Id = Guid.NewGuid();
            }

            var movieAsJsonString = JsonConvert.SerializeObject(movie);           
            var successfullyUpdatedRating = _movieRepo.Put(movieAsJsonString);
            if (!successfullyUpdatedRating)
            {
                throw new Exception("An error occured while trying to put movie");
            }

            UpdateLocalVersionOfDatabase(movie);
            return movie;
        }

        [HttpPost("{id}{rating}")]
        public Movie Rate(Guid id, int rating)
        {
            if (rating < 0 || rating > 10)
            {
                return null;
            }

            if (id == Guid.Empty)
            {
                return null;
            }

            var listOfMoviesWithMatchingId = _movies.FindAll(movie => movie.Id == id);
            if (listOfMoviesWithMatchingId.Count > 1)
            {
                throw new Exception($"The database has more than one movie with this Guid: {id}");
            }

            if (listOfMoviesWithMatchingId.Count == 0)
            {
                throw new Exception($"Movie with Guid: {id} does not exist in the database");
            }
            var moviePicked = listOfMoviesWithMatchingId.First();
            var newRating = (moviePicked.Ratings * moviePicked.Rating + rating) / (moviePicked.Ratings + 1);
            moviePicked.Rating = newRating;
            moviePicked.Ratings += 1;

            var movieAsJsonString = JsonConvert.SerializeObject(moviePicked);
            var successfullyUpdatedRating = _movieRepo.Put(movieAsJsonString);
            if (!successfullyUpdatedRating)
            {
                throw new Exception("An error occured while trying to update movie rating");
            }

            UpdateLocalVersionOfDatabase(moviePicked);
            return moviePicked;
        }

        [HttpGet("[action]/{query}")]
        public Movie Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }
            var movie = _movies.First(q => q.Title.Equals(query, StringComparison.InvariantCultureIgnoreCase));
            return movie;
        }

        [HttpGet("[action]/{query}")]
        public Movie[] SearchByGenre(string[] queryList)
        {
            // Would be best if Genre was an array instead of a string in the database.
            // You could "solve" that by splitting the string in to an array with the | as split character here.
            // As it is now if you add a Genre called "romantic comedy" you will get false hits
            // if somemone searches for just "comedy".

            var movies = new List<Movie>();
            foreach (var movie in _movies)
            {
                movies.AddRange(from query in queryList where movie.Genre.Contains(query, StringComparison.InvariantCultureIgnoreCase) select movie);
            }
            return movies.ToArray();
        }

        private void UpdateLocalVersionOfDatabase(Movie movie)
        {
            // This will change the order of the list but I cant see how it makes any difference.
            _movies.Remove(movie);
            _movies.Add(movie);
        }
    }
}