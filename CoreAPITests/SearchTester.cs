using CoreAPI.Controllers;
using NUnit.Framework;

namespace CoreAPITests
{
    [TestFixture, Description("Tests the search functions of the BoxOffice CoreAPI")]
    public class SearchTester
    {
        private static MoviesController _movieController;


        [OneTimeSetUp]
        public void Setup()
        {
            _movieController = new MoviesController();

        }

        [Test, Description("Should find a movie with the title 'Teen Spirit'. Tests for matching title."), MaxTime(500)]
        public void SearchForTeenSpirit()
        {
            var title = "Teen Spirit";
            var movie = _movieController.Search(title);

            Assert.AreEqual(title, movie.Title);
        }

        [Test, Description("Should find a movie with the title 'Super Hero Party Clown'. Tests for lowercase."), MaxTime(500)]
        public void SearchForSuperHeroPartyClown()
        {
            var title = "super hero party clown";
            var movie = _movieController.Search(title);

            Assert.AreEqual(title, movie.Title.ToLower());
        }

        [Test, Description("Should find a movie with the title 'George of the Jungle'. Tests for movies with similar names."), MaxTime(500)]
        public void SearchForGeorgeOfTheJungle()
        {
            var title = "George of the Jungle";
            var movie = _movieController.Search(title);

            Assert.AreEqual(title, movie.Title);
        }

        [Test, Description("Should not find anything any movies."), MaxTime(500)]
        public void SearchForNothing()
        {
            var movie = _movieController.Search(null);

            Assert.IsNull(movie);
        }
    }
}
