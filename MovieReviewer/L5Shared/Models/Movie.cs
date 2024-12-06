using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5Shared.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public MovieDetails Details { get; set; }
        public MovieNote Note { get; set; }

        public int DirectorID { get; set; }
        public Director Director { get; set; }
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
