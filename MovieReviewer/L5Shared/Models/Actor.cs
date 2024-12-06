using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5Shared.Models
{
    public class Actor
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
