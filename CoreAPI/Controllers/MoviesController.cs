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


        public MoviesController()
        {
            _movieRepo = new MovieRepo();
        }

        [HttpGet]
        public Movie[] Get()
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(_movieRepo.Get());
            return movies.ToArray();
        }

        [HttpGet("{id}")]
        public Movie Get(Guid id)
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(_movieRepo.Get()).ToList();
            return movies.First(movie => movie.Id == id);
        }

        [HttpPut]
        public Movie Put([FromBody] Movie movie)
        {
            throw new NotImplementedException();
        }

        public Movie Rate(Guid id, int rating)
        {
            throw new NotImplementedException();
        }

        [HttpGet("[action]/{query}")]
        public Movie Search(string query)
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(_movieRepo.Get()).ToList();
            var movie = movies.First(q => q.Title.Contains(query));
            return movie;
        }

        [HttpGet("[action]/{query}")]
        public Movie[] SearchByGenre(string[] query)
        {
            var movies = JsonConvert.DeserializeObject<Movie[]>(_movieRepo.Get());
            movies = movies.Where(q => q.Genre.Contains(query[0])).ToArray();
            return movies;
        }
    }
}