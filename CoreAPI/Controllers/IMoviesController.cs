using System;
using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    public interface IMoviesController
    {
        Movie[] Get();
        Movie Get(Guid id);
        Movie Search(string query);
        Movie[] SearchByGenre(string[] query);
        Movie Put([FromBody] Movie movie);
        Movie Rate(Guid id, int rating);
    }
}