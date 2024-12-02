using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MovieReservation
{
    internal class ShowTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime StartDate { get; set; }
        public int MovieId { get; set; }
        public Movie movie { get; set; }
        public virtual ICollection<Reservation> reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public void InitializeSeats()
        {
            for (int i = 0; i <= 10; i++)
            {
                Seats.Add(new Seat { SeatNumber = i, IsReserved = false});
            }
        }
    }   
}
