using CoreAPI.Controllers;
using NUnit.Framework;

namespace CoreAPITests
{
    [TestFixture, Description("Tests the search functions of the BoxOffice CoreAPI")]
    public class GenreSearchTester
    {
        private static MoviesController _movieController;


        [OneTimeSetUp]
        public void Setup()
        {
            _movieController = new MoviesController();

        }

        [Test, Description("Should find a list of movies with the genre 'Drama'"), MaxTime(500)]
        public void SearchForDrama()
        {
            var genre = new string[] { "Drama" };
            var movies = _movieController.SearchByGenre(genre);
            Assert.Greater(movies.Length, 0, $"Unable to find any movies with the {genre} genre");
        }

        [Test, Description("Should find at least 20 movies with the Action and Crime genres."), MaxTime(500)]
        public void SearchForActionAndCrime()
        {
            var genres = new string[] { "Crime", "Action" };

            var movies = _movieController.SearchByGenre(genres);
            Assert.Greater(movies.Length, 20, $"Unable to find at least 20 movies with the {genres[0]} and {genres[1]} genres");
        }

    }
}
