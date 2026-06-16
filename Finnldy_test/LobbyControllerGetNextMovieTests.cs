using Finnldy.BLL;
using Xunit;

namespace Finnldy.Tests;

public class LobbyControllerGetNextMovieTests
{
    [Fact]
    public void GetNextMovie_WhenMovieIsInList_ReturnsNextMovie()
    {

        var controller = new LobbyController();



        var firstMovie = TestDataFactory.CreateMovie(1, "Film 1");


        var secondMovie = TestDataFactory.CreateMovie(2, "Film 2");



        controller.Moviesfüllen(new List<Movies>
        {

            firstMovie,
            secondMovie


        });


        Movies? result = controller.GetNextMovie(firstMovie);



        Assert.Same(secondMovie, result);


    }

    [Fact]
    public void GetNextMovie_WhenMovieIsLast_ReturnsNull()
    {

        var controller = new LobbyController();

        var firstMovie = TestDataFactory.CreateMovie(1, "Film 1");

        var lastMovie = TestDataFactory.CreateMovie(2, "Film 2");



        controller.Moviesfüllen(new List<Movies>
        {
            firstMovie,
            lastMovie


        });




        Movies? result = controller.GetNextMovie(lastMovie);

        Assert.Null(result);
    }
}