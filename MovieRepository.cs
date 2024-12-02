using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    internal class MovieRepository
    {

        private readonly MovieDB _context;

        public MovieRepository(MovieDB context)
        {
            _context = context;
        }

        public Movie AddMovie(string title, string description, movieType type)
        {
            var movie = new Movie
            {
                title = title,
                description = description,
                Type =type
            };
            if (movie == null)
            {
                Console.WriteLine("Invalid Movie");
                return null;
            }
            else
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();

                return movie;
            }
        }


        public Movie UpdateMovie(int id,string title, string description, movieType type)
        {
            var movie = _context.Movies.Find(id);

            if(movie != null)
            {
                movie.title = title;
                movie.description = description;
                movie.Type = type;

                _context.SaveChanges();
            }
            return movie;
        }

        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);

            if(movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }

        public List<Movie> GetMovies()
        {
            return _context.Movies.ToList();
        }
        
    }
}
