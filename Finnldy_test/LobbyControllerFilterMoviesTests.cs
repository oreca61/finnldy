

using System.Reflection;
using Finnldy.BLL;
using Xunit;

namespace Finnldy.Tests;

public class LobbyControllerFilterMoviesTests
{
    [Fact]
    public void FilterMovies_WhenUnwantedGenreExists_RemovesMoviesWithThatGenre()
    {
        var controller = new LobbyController();

        var actionMovie = TestDataFactory.CreateMovie(1, "Action Film", 28);

        var comedyMovie = TestDataFactory.CreateMovie(2, "Comedy Film", 35);
        var mixedMovie = TestDataFactory.CreateMovie(3, "Action Comedy", 28, 35);


        controller.Moviesfüllen(new List<Movies>
        {
            actionMovie,
            comedyMovie,
            mixedMovie


        });

        Lobby lobby = GetPrivateField<Lobby>(controller, "lobby");

        lobby.UnwantedGenreIds = new List<int> { 28 };


        controller.FilterMovies();


        MovieReposotory repository = GetPrivateField<MovieReposotory>(controller, "movies");


        Assert.DoesNotContain( repository.movies, movie =>movie.ApiMovieId == actionMovie.ApiMovieId);
        Assert.Contains(repository.movies, movie=> movie.ApiMovieId ==comedyMovie.ApiMovieId);

        Assert.DoesNotContain( repository.movies, movie=> movie.ApiMovieId == mixedMovie.ApiMovieId);
    }

    private static T GetPrivateField<T>(object target, string fieldName)
    {
        FieldInfo? field = target.GetType().GetField(
            fieldName,

            BindingFlags.Instance | BindingFlags.NonPublic


        );

        Assert.NotNull(field);

        object? value = field!.GetValue(target);

        Assert.NotNull(value );


        return Assert.IsType<T>(value );
    }
}