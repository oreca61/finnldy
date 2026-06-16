using Finnldy.BLL;

namespace Finnldy.Tests;

public static class TestDataFactory
{
    public static Movies CreateMovie(int id, string name, params int[] genreIds)
    {
        return new Movies(
            apiMovieId: id,
            name: name,
            description: "Testbeschreibung",
            genreIds: genreIds.ToList(),
            cover: "",
            releaseDate: "2026-01-01",
            originalLanguage: "de",
            adult: false,
            voteAverage: 8.0
        );
    }
}