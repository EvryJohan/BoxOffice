using CoreAPI.Controllers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CoreAPITests
{
    [TestFixture, Description("Tests the rating functions of the BoxOffice CoreAPI")]

    public class RatingTester
    {

        private static MoviesController _movieController;

        [OneTimeSetUp]
        public void Setup()
        {
            _movieController = new MoviesController();

        }

        [Test, Description("Checks if movie implements ratings."), MaxTime(500)]
        public void MoviesHasRating()
        {

            var movies = _movieController.Get().ToList();

            movies.ForEach(movie =>
            {
                var t = movie.GetType();
                var rating = t.GetProperty("Rating");
                var ratings = t.GetProperty("Ratings");

                Assert.IsNotNull(rating, "Property Rating is missing on object Movie");
                Assert.IsNotNull(ratings, "Property Ratings is missing on object Movie");

            });
        }

        [Test, Description("Checks that wrongly inputed guids for ratings are handled."), MaxTime(500)]
        public void EmptyGuidCheck()
        {
            Assert.DoesNotThrow(() => _movieController.Rate(Guid.Empty, 5), "Is unable to handle empty guids.");            
        }

        [Test, Description("Checks that only ratings between 0 and 5 can be added.")]
        public void RatingValueCheck()
        {
            Assert.IsNull(_movieController.Rate(new Guid("4ee04b48-8421-4aae-9000-12356c9e1936"), -1), "Accepts negative values.");
            Assert.IsNull(_movieController.Rate(new Guid("4ee04b48-8421-4aae-9000-12356c9e1936"), 11), "Accepts too high values.");
        }

        [Test, Description("Checks that ratings are increased correctly."), MaxTime(500)]
        public void RatingIsIncreasedCorrectly()
        {

            var movieBeforeRating = _movieController.Get(new Guid("4ee04b48-8421-4aae-9000-12356c9e1936"));
            var originalRating = GetRating(movieBeforeRating);

            var movieAfterRating = _movieController.Rate(new Guid("4ee04b48-8421-4aae-9000-12356c9e1936"), 10);
            var newRating = GetRating(movieAfterRating);

            Assert.GreaterOrEqual(newRating, originalRating);

            ResultIsWithinLimits(newRating, 3.752f);
        }

        [Test, Description("Checks that ratings are decreased correctly."), MaxTime(500)]
        public void RatingIsDecreasedCorrectly()
        {

            var movieBeforeRating = _movieController.Get(new Guid("3719cc0c-ffca-453c-9734-9e581cf28179"));
            var originalRating = GetRating(movieBeforeRating);

            var movieAfterRating = _movieController.Rate(new Guid("3719cc0c-ffca-453c-9734-9e581cf28179"), 0);
            var newRating = GetRating(movieAfterRating);

            Assert.Less(newRating, originalRating);

            ResultIsWithinLimits(newRating, 1.017f);

        }

        private static void ResultIsWithinLimits(string newRating, float expected, float within = 0.01f)
        {
            double.TryParse(newRating,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var result);
            Assert.That(result, Is.EqualTo(expected).Within(within));
        }

        private string GetRating(object movie)
        {
            var jsonmovie = JsonConvert.SerializeObject(movie);
            var dictionaryMovie = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonmovie);
            Assert.That(dictionaryMovie.ContainsKey("Rating") && dictionaryMovie.ContainsKey("Ratings"), "Rating and / or Ratings properties are missing from Movie class");

            dictionaryMovie.TryGetValue("Rating", out var rating);

            return rating;
        }
    }
}
