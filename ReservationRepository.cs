using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    internal class ReservationRepository
    {
        private readonly MovieDB _context;

        public ReservationRepository(MovieDB context)
        {
            _context = context;
        }
        //public List<Seat> GetAvailableSeats(int showtimeId)
        //{
        //    var reservedSeats = _context.Reservations
        //        .Where(r => r.showTimeId == showtimeId && r.userId != null)
        //        .Select(r => r.SeatId)
        //        .ToList();

        //    return _context.Seats
        //        .Where(s => !reservedSeats.Contains(s.id))
        //        .ToList();
        //}
        public ShowTime GetShowtime(int showTimeId)
        {
            return _context.ShowTimes
                .Include(st => st.movie)
                .Include(st => st.Seats)
                .FirstOrDefault(st => st.id == showTimeId);
        }
        public (bool success, string message) ReserveSeats(int userId, int showtimeId, List<int> SeatNumbers)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var showtime = GetShowtime(showtimeId);
                if (showtime == null)
                {
                    return (false, "showtime not found. ");
                }
                var seats = showtime.Seats
                    .Where(s => SeatNumbers.Contains(s.SeatNumber)).ToList();
                if (seats.Count != SeatNumbers.Count)
                {
                    return (false, "One or more seats not found. ");
                }
                if (seats.Any(s => s.IsReserved))
                {
                    return (false, "One or more seats are already reserved. ");
                }
                var reservation = new Reservation
                {
                    userId = userId,
                    showTimeId = showtimeId,
                    ReservationTime = DateTime.Now,
                    RservedSeats = seats

                };
                foreach (var seat in seats)
                {
                    seat.IsReserved = true;
                }
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                transaction.Commit();
                return (true, $"Seccessfully reserved {seats.Count} seats! ");


            }
            catch (Exception ex) { 
                transaction.Rollback();
                return (false, $"Error making reservation: {ex.Message}");
            }
             

            
        }
        public List<Reservation> GetUserReservations(int userId)
        {
            return _context.Reservations
            .Where(r => r.userId == userId)
            .Include(r => r.ShowTime)
            .ThenInclude(st => st.movie)
            .Include(r => r.RservedSeats)
            .Include(r => r.user)
            .ToList();
        }

        public bool CancelReservation(int userId, int resrvationId)
        {
            var reservation = _context.Reservations
                .Include(r => r.RservedSeats)
                .Include(r => r.ShowTime)
                .FirstOrDefault(r => r.userId == userId && r.id == resrvationId);
            if (reservation != null)
            {
                foreach (var seat in reservation.RservedSeats)
                {
                    seat.IsReserved = false;
                }
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
                return true;
            }
            return false;

        }


        public IEnumerable<Reservation> GetReservations()
        {
            try
            {
                return _context.Reservations.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reservations: {ex.Message}");
                return new List<Reservation>();
            }

        }
    }
    
}