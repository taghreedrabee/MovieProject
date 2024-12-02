using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservation
{
    internal class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public movieType Type { get; set; }
        public ICollection<ShowTime> ShowTimes { get; set; }
    }

    public enum movieType
    {
        Action ,
        Romance,
        Comedy,
        Herror
    }
}
