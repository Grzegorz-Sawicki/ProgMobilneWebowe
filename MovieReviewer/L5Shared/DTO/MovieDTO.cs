using L5Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5Shared.DTO
{
    public class MovieDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }

        //Details
        public DateTime ReleaseDate { get; set; }
        public int Length { get; set; }

        //Note
        public float Rating { get; set; }
        public string Review { get; set; }

        //Director
        public int DirectorID { get; set; }
        public string DirectorName { get; set; }

        //Actor
        public ICollection<ActorDTO> Actors { get; set; } = new List<ActorDTO>();
    }
}
