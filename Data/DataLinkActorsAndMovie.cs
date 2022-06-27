using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataLinkActorsAndMovie
    {
        public int IdOfActorOnMovie { get; private set; }
        public int IdOfActor { get; private set; }
        public int IdOfMovie { get; private set; }
        public DataLinkActorsAndMovie(int idOfActorOnMovie, int idOfActor, int idOfMovies)
        {
            IdOfActorOnMovie = idOfActorOnMovie;
            IdOfActor = idOfActor;
            IdOfMovie = idOfMovies;
        }

    }
}
