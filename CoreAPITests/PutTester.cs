using CoreAPI.Controllers;
using CoreAPI.Models;
using NUnit.Framework;
using System;

namespace CoreAPITests
{
    [TestFixture, Description("Tests the put functions of the BoxOffice CoreAPI")]

    public class PutTester
    {
        private static MoviesController _movieController;

        [OneTimeSetUp]
        public void Setup()
        {
            _movieController = new MoviesController();

        }

        [Test, Description("Should insert the New Movie."), MaxTime(500)]
        public void InsertNewMovie()
        {

            var movie = new Movie() { Title = "New Movie", Description = "New Movie description", Genre = "Drama|Action" };
            _movieController.Put(movie);

            var databaseMovie = _movieController.Search(movie.Title);

            Assert.IsNotNull(databaseMovie, "Unable to find the inserted movie.");
            Assert.AreEqual(movie.Title, databaseMovie.Title, "Unable to find a movie with matching title");
            Assert.AreEqual(movie.Description, databaseMovie.Description, "Unable to find a movie with matching description");
            Assert.AreEqual(movie.Genre, databaseMovie.Genre, "Unable to find a movie with matching genre");

        }

        [Test, Description("Checks if newly inserted movies are given an Id."), MaxTime(500)]
        public void CheckForId()
        {

            var movie = new Movie() { Title = "New Movie", Description = "New Movie description", Genre = "Drama|Action" };
            var insertedMovie = _movieController.Put(movie);

            Assert.That(insertedMovie.Id != Guid.Empty, "Inserted movie is given an empty Id.");
        }

        [Test, Description("Number of movies in database should increase on insert."), MaxTime(500)]
        public void CheckForIncrement()
        {
            
            var originalNumber = _movieController.Get().Length;

            var movie = new Movie() { Title = "Incremented Movie", Description = "Incremented Movie description", Genre = "Crime|Fantasy" };
            _movieController.Put(movie);

            var newNumber = _movieController.Get().Length;

            //Assert.Greater(newNumber, originalNumber, "The number of movies in the database did not increase.");
            Assert.AreEqual(newNumber, originalNumber + 1, "The number of movies in the database did not increase by one.");
        }

        [Test, Description("Should insert a movie, then update its description."), MaxTime(500)]
        public void UpdateMovie()
        {

            var movie = new Movie() { Title = "Updatable Movie", Description = "Updatable Movie description", Genre = "Documentary" };
            movie = _movieController.Put(movie);

            var firstCheck = _movieController.Get(movie.Id).Description;
            movie.Description += " with more text!";
            _movieController.Put(movie);

            var secondCheck = _movieController.Get(movie.Id).Description;

            Assert.IsNotNull(firstCheck, "Unable to find inserted movie.");
            Assert.AreEqual(movie.Description, secondCheck, "Movie did not insert correctly.");
            Assert.AreNotEqual(firstCheck, secondCheck, "Movie did not update.");

        }

    }
}
