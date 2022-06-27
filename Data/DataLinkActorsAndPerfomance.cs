using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataLinkActorsAndPerfomance
    {
        public int IdOfActorOnPerfomance { get; private set; }
        public int IdOfActor { get; private set; }
        public int IdOfPerfomance { get; private set; }
        public DataLinkActorsAndPerfomance(int idOfActorOnPerfomance, int idOfActor, int idOfPerfomance)
        {
            IdOfActorOnPerfomance = idOfActorOnPerfomance;
            IdOfActor = idOfActor;
            IdOfPerfomance = idOfPerfomance;
        }
    }
}
