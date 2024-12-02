using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MovieReservation
{
    internal class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string username { get; set; }
        public string hashPassword { get; set; }
        public userrole role { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
    public enum userrole {
        User,
        Admin
    }

}
