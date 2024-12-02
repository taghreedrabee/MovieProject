using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MovieDB())
            {
                var movieReservation = new movieReservation(context);
                movieReservation.Run();
            }
        }
    }
}
