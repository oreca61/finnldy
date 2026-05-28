using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finnldy.BLL
{
    public class ResponseToAPI
    {
        public bool Status;
        public Movies? Movie;
        public Movies? Result;

        public ResponseToAPI(bool status, Movies? movie, Movies? result)
        {
            Status = status;
            Movie = movie;
            Result = result;
        }
    }
}
