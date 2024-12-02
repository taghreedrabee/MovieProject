using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    internal class ShowTimeRepository
    {

        private readonly MovieDB _context;

        public ShowTimeRepository(MovieDB context)
        {
            _context = context;
        }

        public ShowTime AddShowtime(int movieId, DateTime startTime)
        {
            var showtime = new ShowTime
            {
                MovieId = movieId,
                StartDate = startTime
            };
            showtime.InitializeSeats();
            _context.ShowTimes.Add(showtime);
            _context.SaveChanges();

            return showtime;

        }

        public ShowTime UpdateShowtime(int id, DateTime startTime)
        {
            var showtime = _context.ShowTimes.Find(id);

            if(showtime != null)
            {
                showtime.StartDate = startTime;
                _context.SaveChanges();
            }
            return showtime;
        }

        public bool DeleteShowtime(int id)
        {
            
            var showtime = _context.ShowTimes.Find(id);
            if (showtime == null) return false;
            _context.ShowTimes.Remove(showtime);
            return _context.SaveChanges() > 0;

        }

        public List<ShowTime> GetShowTimes(int movieId)
        {
            return _context.ShowTimes.Where(s => s.MovieId == movieId).ToList();
        }

    }
}
