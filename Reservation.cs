using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservation
{
    internal class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int showTimeId { get; set; }
        public ShowTime ShowTime { get; set; }
        public DateTime ReservationTime { get; set; }
        public ICollection<Seat> RservedSeats { get; set; }
    }
}
