using CoreAPI.Controllers;
using NUnit.Framework;
using System;

namespace CoreAPITests
{

    [TestFixture, Description("Tests the Get Methods of the BoxOffice CoreAPI")]
    public class GetTester
    {
        private static MoviesController _movieController;

        [OneTimeSetUp]
        public void Setup()
        {
            _movieController = new MoviesController();

        }


        [Test, Description("Should return all movies in the repository in a timly manner"), MaxTime(500)]
        public void Get()
        {
            var movies = _movieController.Get();
            Assert.Greater(movies.Length, 999);
        }

        [Test, Description("Should find a movie with the id '1c72feb1-3e90-45d8-be48-d3c2e148ecd4'"), MaxTime(500)]

        public void GetMovie()
        { 

            var id = new Guid("1c72feb1-3e90-45d8-be48-d3c2e148ecd4");
            var result = _movieController.Get(id);
            Assert.AreEqual(id, result.Id);
        }

        [Test, Description("Should find a movie with the id 'dd258617-aa5d-4c21-b781-6f32053aeb6d'"), MaxTime(500)]

        public void GetMovie2()
        {

            var id = new Guid("dd258617-aa5d-4c21-b781-6f32053aeb6d");
            var result = _movieController.Get(id);
            Assert.AreEqual(id, result.Id);
        }

        [Test, Description("Should find a movie with the id '1f1a185c-3fa5-4fd9-a22e-3826fdef443f'"), MaxTime(500)]
        public void GetMovie3()
        {

            var id = new Guid("1f1a185c-3fa5-4fd9-a22e-3826fdef443f");
            var result = _movieController.Get(id);
            Assert.AreEqual(id, result.Id);
        }

    }
}
