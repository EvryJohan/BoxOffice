# Welcome to BoxOffice!

BoxOffice is a simple movie API that lets a user look up, add and rate movies... or that is atleast what it should do if it was working.

The project got started, a controller was created, but not all of its methods are fully implemented or optimized.

    * Get - Gets all movies from our repository.
    * Get(Guid id) - Gets a specific movie using its Id, works well but can be slow.
    * Search(string query) - Searches for a movie with a specific title. Works, but not well.
    * SearchByGenre(string[] query) - Searches for movies with matching genres. Does not work as intended.
    * Put(Movie movie) - Should insert a movie into the database once it has been implemented.
    * Rate(Guid id, int rating) - Adds a rating to a specified movie between 0 and 10. Not currently implemented.

Our external movie repository is not working very well, it is slow and only implements two methods. 

    * Get - which returns all movies in the repo.
    * Put - which either updates an existing movie or inserts a new one.

On top of this, it is not persistant and resets on app start - Making it persistant is not part of this test, but it is worth noting.

To verify the application, a series of tests have been written in a separate project.

The goal of this test is to implement missing methods and improve existing ones in the controller. 
Refactoring the solution and improving the test project is not necessary but we will discuss the solution as a whole once the task has been completed.

Good luck!
