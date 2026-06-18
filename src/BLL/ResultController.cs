using System.Collections.Generic;
using System.Linq;

namespace Finnldy.BLL
{
    public class ResultController
    {
        private Dictionary<int, Result> resultsByMovieId = new Dictionary<int, Result>();

        public void AddSwipeToResult(Movies movie, SwipeType swipeType, string username)
        {
            if (movie == null)
            {
                return;
            }

            if (!resultsByMovieId.ContainsKey(movie.ApiMovieId))
            {
                resultsByMovieId[movie.ApiMovieId] = new Result(movie);
            }

            resultsByMovieId[movie.ApiMovieId].AddSwipe(username, swipeType);
        }

        // Sorft dafür das bei gleichem Score der Film mit der höheren Bewerung zuerst kommt

        public List<Result> GetBestResults()
        {
            return resultsByMovieId.Values
                .Where(result => result.Watched == 0)
                .OrderByDescending(result => result.Score)
                .ThenByDescending(result => result.Movie.VoteAverage)
                .Take(5)
                .ToList();
        }

        public void ClearResults()
        {
            resultsByMovieId.Clear();
        }
    }
}