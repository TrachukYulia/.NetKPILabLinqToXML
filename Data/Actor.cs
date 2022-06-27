using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class Actor
    {
        public string[] arr;
        public int IdOfActor { get; private set; }
        public string FullName { get; private set; }
        public DateTime DateOfBirthday { get; private set; }
        public List<string> Amplua { get; private set; }
        public Actor(int idOfActor, string fullName, DateTime dateOfBirthday, List<string> amplua)
        {
            IdOfActor = idOfActor;
            FullName = fullName;
            DateOfBirthday = dateOfBirthday;
            Amplua = amplua;
        }
        public string GetListOfAmplua
            ()
        {
            string ampluaOfActor = null;
            foreach (var item in Amplua)
            {
                ampluaOfActor = ampluaOfActor + item + " ";
            }
            ampluaOfActor.TrimEnd();
            return ampluaOfActor;
        }

    }
}
