using Finnldy.BLL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finnldy.DAL
{
    public interface ISwipeApiService
    {
        Task<bool> SaveSwipeAsync(User user, Movies movie, SwipeType swipeType);

        Task<List<ApiMovieResult>> GetResultsAsync();
    }
}