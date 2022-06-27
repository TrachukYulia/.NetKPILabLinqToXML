using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Movie
    {
        public int IdOfMovie { get; set; }
        public string Name { get; set; }
        public DateTime DateOfIssue { get; set; }
        public List<string> GenreOfMovie { get; set; }
        public string FullNameOfDirector { get; set; }
        public Movie(int idOfMovie, string name, DateTime dateOfIssue, List<string> genreOfMovie, string fullNameOfDirector)
        {
            IdOfMovie = idOfMovie;
            Name = name;
            DateOfIssue = dateOfIssue;
            GenreOfMovie = genreOfMovie;
            FullNameOfDirector = fullNameOfDirector;
        }
        public string GetListOfGenre()
        {
            string genreOfMovieString = null;
            foreach (var item in GenreOfMovie)
            {
                genreOfMovieString = genreOfMovieString + item + " ";
            }
            genreOfMovieString.TrimEnd();
            return genreOfMovieString;
        }
    }
}
