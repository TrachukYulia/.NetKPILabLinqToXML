using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Perfomance
    {
        public int IdOfPerfomance { get; set; }
        public string Name { get; set; }
        public string GenreOfPerfomance { get; set; }
        public Perfomance(int idOfPerfomance, string name, string genreOfPerfomance)
        {
            IdOfPerfomance = idOfPerfomance;
            Name = name;
            GenreOfPerfomance = genreOfPerfomance;
        }
        public override string ToString()
        {
            return string.Format($"(id={IdOfPerfomance};  Name ={Name}; Genre={GenreOfPerfomance})");
        }

    }
}
