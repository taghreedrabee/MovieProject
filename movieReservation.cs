using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    internal class movieReservation
    {
        
            private readonly MovieDB _context;
            private readonly UserRepository _user;
            private readonly MovieRepository _movie;
            private readonly ShowTimeRepository _showtime;
            private readonly ReservationRepository _reservation;
            private User _currentUser;

            public movieReservation(MovieDB context)
            {
                _context = context;
                _user = new UserRepository(context);
                _movie = new MovieRepository(context);
                _showtime = new ShowTimeRepository(context);
                _reservation = new ReservationRepository(context);

            }


        public void Run()
            {
            
            while (true)
            {
                Console.WriteLine("Welcome to the Movie Reservation System!");
                var user = AuthenticateUser();

                if (user != null)
                {
                    if (user.role == userrole.Admin)
                    {
                        AdminMenu(user);
                    }
                    else
                    {
                        UserMenu(user);
                    }
                }
            }
            }

            private User AuthenticateUser()
            {
                while (true)
                {
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Create New User");
                    Console.WriteLine("3. Update Password");
                    Console.WriteLine("4. Exit");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            return Login();
                        case "2":
                            return Register();
                        case "3":
                            UpdatePassword();
                            break;
                        case "4":
                        Environment.Exit(0) ;
                        break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }

            private User Login()
            {
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("Password: ");
                var password = Console.ReadLine();

                _currentUser = _user.Login(username, password);

                if (_currentUser != null)
                {
                    Console.WriteLine($"Welcome, {_currentUser.username}!");
                    return _currentUser;
                }

                Console.WriteLine("Invalid username or password.");
                return null;
            }

            private User Register()
            {
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("Password: ");
                var password = Console.ReadLine();

                _currentUser = _user.Register(username, password, userrole.User);
                Console.WriteLine("User registered successfully!");
                return _currentUser;
            }

            private void UpdatePassword()
            {
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("Old Password: ");
                var oldPassword = Console.ReadLine();
                Console.Write("New Password: ");
                var newPassword = Console.ReadLine();

                if (_user.UpdatePassword(username, oldPassword, newPassword))
                {
                    Console.WriteLine("Password updated successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid username or old password.");
                }
            }

            private void AdminMenu(User user)
            {
                while (true)
                {
                    Console.WriteLine("1. Add Movie");
                    Console.WriteLine("2. Update Movie");
                    Console.WriteLine("3. Delete Movie");
                    Console.WriteLine("4. Add Showtime");
                    Console.WriteLine("5. Update Showtime");
                    Console.WriteLine("6. Delete Showtime");
                    Console.WriteLine("7. View Reservations");
                    Console.WriteLine("8. Logout");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddMovie();
                            break;
                        case "2":
                            UpdateMovie();
                            break;
                        case "3":
                            DeleteMovie();
                            break;
                        case "4":
                            AddShowtime();
                            break;
                        case "5":
                            UpdateShowtime();
                            break;
                        case "6":
                            DeleteShowtime();
                            break;
                        case "7":
                            ViewReservations();
                            break;
                        case "8":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }

            private void AddMovie()
            {
                Console.Write("Title: ");
                var title = Console.ReadLine();
                Console.Write("Description: ");
                var description = Console.ReadLine();
                Console.Write("Type (Action, Romance, Comedy, Herror): ");
                var type = Enum.Parse<movieType>(Console.ReadLine(), true);

                var movie = _movie.AddMovie(title, description, type);
                Console.WriteLine($"Movie added successfully with ID: {movie.id}");
            }

            private void UpdateMovie()
            {
                Console.Write("Movie ID: ");
                var id = int.Parse(Console.ReadLine());
                Console.Write("New Title: ");
                var title = Console.ReadLine();
                Console.Write("New Description: ");
                var description = Console.ReadLine();
                Console.Write("New Type: ");
                var type = Enum.Parse<movieType>(Console.ReadLine(), true);

            var movie = _movie.UpdateMovie(id, title, description, type);
                if (movie != null)
                {
                    Console.WriteLine($"Movie updated successfully with ID: {movie.id}");
                }
                else
                {
                    Console.WriteLine("Movie not found.");
                }
            }

            private void DeleteMovie()
            {
                Console.Write("Movie ID: ");
                var id = int.Parse(Console.ReadLine());

                _movie.DeleteMovie(id);
                Console.WriteLine("Movie deleted successfully.");
            }

            private void AddShowtime()
            {
            var movies = _movie.GetMovies();
            Console.WriteLine("\nAvailable Movies: ");
            foreach (var movie in movies) { 
                Console.WriteLine($"ID: {movie.id} - {movie.title}");
            }
                
                Console.Write("Movie ID: ");
                var movieId = int.Parse(Console.ReadLine());
                Console.Write("Start Time (yyyy-MM-dd HH:mm): ");
                var startTime = DateTime.Parse(Console.ReadLine());
                var showtime = _showtime.AddShowtime(movieId, startTime);
                Console.WriteLine($"Showtime added successfully with ID: {showtime.id}");
                _context.SaveChanges();
        }

            private void UpdateShowtime()
            {
                Console.Write("Showtime ID: ");
                var id = int.Parse(Console.ReadLine());
                Console.Write("New Start Time (yyyy-MM-dd HH:mm): ");
                var startTime = DateTime.Parse(Console.ReadLine());

                var showtime = _showtime.UpdateShowtime(id, startTime);
                if (showtime != null)
                {
                    Console.WriteLine($"Showtime updated successfully with ID: {showtime.id}");
                }
                else
                {
                    Console.WriteLine("Showtime not found.");
                }
            }

            private void DeleteShowtime()
            {
                
                Console.Write("Showtime ID: ");
                var id = int.Parse(Console.ReadLine());

                _showtime.DeleteShowtime(id);
                Console.WriteLine("Showtime deleted successfully.");
            }

            private void ViewReservations()
            {
            var reservations = _reservation.GetReservations();

            if (reservations.Any())
            {
                Console.WriteLine("Reservations:");
                foreach (var res in reservations)
                {
                    Console.WriteLine($"User: {res.user.username}, Movie: {res.ShowTime.movie.title}, Time: {res.ShowTime.StartDate}");
                }
            }
            else
            {
                Console.WriteLine("No reservations found.");
            }

        }

            private void UserMenu(User user)
                {
                    while (true)
                    {
                        Console.WriteLine($"===Welcome, {_currentUser.username}===");
                        Console.WriteLine("1. View Available Movies");
                        Console.WriteLine("2. Reserve Seats");
                        Console.WriteLine("3. View My Reservations");
                        Console.WriteLine("4. Cancel Reservation");
                        Console.WriteLine("5. Logout");
                        Console.Write("Choose an option: ");
                        var choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                ViewAvailableMovies();
                                break;
                            case "2":
                                ReserveSeats();
                                break;
                            case "3":
                                ViewMyReservations();
                                break;
                            case "4":
                                CancelReservation();
                                break;
                            case "5":
                                return;
                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }
                    }
                }

            private void ViewAvailableMovies()
            {
                var movies = _movie.GetMovies();

                Console.WriteLine("Available Movies:");
                foreach (var movie in movies)
                {
                    Console.WriteLine($"ID: {movie.id}");
                    Console.WriteLine($"Title: {movie.title}");
                    Console.WriteLine($"Type: {movie.Type}");
                    Console.WriteLine("\nshowtimes : ");
                    foreach(var showtime in movie.ShowTimes.OrderBy(st => st.StartDate))
                { 
                    Console.WriteLine($"ID: {showtime.id} - {showtime.StartDate:g}");
                }
                }
            }

            private void ReserveSeats()
            {
                Console.Write("Movie ID: ");
                var movieId = int.Parse(Console.ReadLine());
                Console.Write("Showtime ID: ");
                if(!int.TryParse(Console.ReadLine(), out int showTimeId))
                {
                    Console.WriteLine("Invalid ShowTime ID");
                }

                var showTime = _reservation.GetShowtime(showTimeId);
                if(showTime == null)
                {
                    Console.WriteLine("Show time not found!");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("\nAvailable seats:");
                var availableSeats = showTime.Seats.Where(s => !s.IsReserved).ToList();
                foreach (var seat in availableSeats)
                {
                    Console.WriteLine($"Seat {seat.SeatNumber}");
                }
                var selectedSeats = new List<int>();
                Console.Write("Enter seat numbers to reserve (comma separated, e.g, 1,2,3): ");
                string seatInput = Console.ReadLine();

                foreach (var seatStr in seatInput.Split(','))
                {
                    if (int.TryParse(seatStr.Trim(), out int SeatNumber))
                    {
                        selectedSeats.Add(SeatNumber);
                    }
                }
                var (success, message) = _reservation.ReserveSeats(_currentUser.id, showTimeId, selectedSeats);
                Console.WriteLine(message);
            

               
            }

            private void ViewMyReservations()
                    {
                        var reservations = _reservation.GetUserReservations(_currentUser.id);

                        Console.WriteLine("My Reservations:");
                        foreach (var res in reservations)
                        {
                            Console.WriteLine($"Resevation ID: {res.id}  Movie: {res.ShowTime.movie.title}, Time: {res.ShowTime.StartDate}" +
                                $"");
                        }
                    }

            private void CancelReservation()
            {
                Console.Write("Enter the Rservation ID To Cancel: ");
                if(!int.TryParse(Console.ReadLine(), out int reservationId))
            {
                Console.WriteLine("Invalid reservation ID. ");
                return;
            }

                Console.Write("Enter the Seat ID: ");
                var seatId = int.Parse(Console.ReadLine());

                var reservation = _context.Reservations
                    .FirstOrDefault(r => r.id == reservationId && r.userId == _currentUser.id);

                if (reservation != null)
                {
                    _reservation.CancelReservation(_currentUser.id, reservationId);
                    Console.WriteLine("Reservation canceled successfully.");
                }
                else
                {
                    Console.WriteLine("No matching reservation found.");
                }
            }

    }
}
