using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservation
{
    internal class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        public int ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; }
        public Reservation? Reservation { get; set; }

    }
}
