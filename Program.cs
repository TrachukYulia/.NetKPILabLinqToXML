using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Data;
namespace LINQToXML
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Actor> actors = CreateListOfActors();
            List<Movie> movies = CreateListOfMovie();
            List<DataLinkActorsAndMovie> dataLinkActorsAndMovies = CreateDataLinkActorsAndMovie();
            List<DataLinkActorsAndPerfomance> dataLinkActorsAndPerfomances = CreateDataLinkActorsAndPerfomance();
            List<Perfomance> perfomances = CreateListOfPerfomance();
            string nameOfXmlFile = "cinema.xml";
            CreateXMLFile(actors, movies, perfomances, dataLinkActorsAndMovies, dataLinkActorsAndPerfomances, nameOfXmlFile);
            Console.OutputEncoding = Encoding.UTF8;
        
            Console.WriteLine();
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("1. Виведемо всі вистави ");
            XDocument xmlDoc = XDocument.Load(nameOfXmlFile);
            foreach (XElement userElement in
           xmlDoc.Element("Cinema").Element("perfomances").Elements("Perfomance"))
            {
                var id = userElement.Element("Id").Value;
                var name = userElement.Element("Name").Value;
                var genre = userElement.Element("Genre").Value;
                Console.WriteLine(string.Format("Id: {0}, Name: {1}, Genre: {2}", id, name, genre));
            }
            Console.WriteLine();

            Console.WriteLine("2. Виведемо всіх режисерів ");
            foreach (XElement userElement in
         xmlDoc.Element("Cinema").Element("movies").Elements("Movie"))
            {
                var id = userElement.Element("Id").Value;
                var fullName = userElement.Element("FullNameOfDirector").Value;
                Console.WriteLine(string.Format("Id: {0}, Name: {1}", id, fullName));
            }
            Console.WriteLine();

            Console.WriteLine("3. Виведемо всіх акторів, кінофільмів у яких вони грають та жанр");
            var query3 =
                 from x in xmlDoc.Root.Element("dataLinkActorsAndMovies").Elements("DataLinkActorsAndMovies")
                 from y in xmlDoc.Root.Element("movies").Elements("Movie")
                 from z in xmlDoc.Root.Element("actors").Elements("Actor")
                 let v1 = (Int32.Parse(x.Element("IdOfActor").Value) == Int32.Parse(z.Element("Id").Value))
                 let v2 = (Int32.Parse(x.Element("IdOfMovie").Value) == Int32.Parse(y.Element("Id").Value))
                 where v1 && v2
                 select new { y, z};
            foreach (var item in query3)
                Console.WriteLine("Actor:{0}, Movie:{1}, Genre:{2}", item.z.Element("FullName").Value, 
                item.y.Element("Name").Value, item.y.Element("Genre").Value);

            Console.WriteLine();

            Console.WriteLine("4. Сортування акторів по к-ть ролей");
            var query4 = from x in xmlDoc.Root.Element("actors").Elements("Actor")
                         join y in xmlDoc.Root.Element("dataLinkActorsAndMovies").Elements("DataLinkActorsAndMovies")
                         on Int32.Parse(x.Element("Id").Value) equals Int32.Parse(y.Element("IdOfActor").Value) into temp
                         orderby temp.Count() descending
                        select new { Name = x.Element("FullName").Value, counter = temp.Count() };
            foreach (var item in query4)
                Console.WriteLine("Actor:{0}, Counter:{1}", item.Name, item.counter);
            Console.WriteLine();

            Console.WriteLine("5. Виведемо всіх акторів, які є також режисерами");
            var query5 = from y in xmlDoc.Root.Element("movies").Elements("Movie")
                         from z in xmlDoc.Root.Element("actors").Elements("Actor")
                         where y.Element("FullNameOfDirector").Value == z.Element("FullName").Value
                         select new { fullName = z.Element("FullName").Value, nameOfMovie = y.Element("Name").Value };
            foreach (var item in query5)
                Console.WriteLine("Actor:{0}, Movie like Director:{1}", item.fullName, item.nameOfMovie);
            Console.WriteLine();

            Console.WriteLine("6. Виведемо акторів, амплуа яких > 3, відсортуємо за спаданням");
            var query6 = xmlDoc.Element("Cinema").
                Elements("actors").Elements("Actor").
                Where(p => p.Element("Amplua").Value.Split().Count() >= 3).
                OrderByDescending(p => p.Element("Amplua").Value.Split().Count());
            foreach (var item in query6)
            {
                Console.WriteLine("Name:{0}, Amplua:{1}, Counter:{2}", item.Element("FullName").Value,
                    item.Element("Amplua").Value, item.Element("Amplua").Value.Split().Count() - 1);
            }
            Console.WriteLine();

            Console.WriteLine("7. Виведемо перелік акторів, фамілії яких починаються з літери «C»");
            var query7 = from t in xmlDoc.Root.Element("actors").Elements("Actor")
                        where t.Element("FullName").Value.ToUpper().StartsWith("C")
                        select t;
            foreach (var item in query7)
            {
                Console.WriteLine("Full Name: {0}", item.Element("FullName").Value);
            }
            Console.WriteLine();

            Console.WriteLine("8. Виведемо акторів, які народились після 1980 ");
            var query8 = from x in xmlDoc.Root.Element("actors").Elements("Actor")
                        from amplua in (x.Element("Amplua").Nodes()).ToString()
                        where DateTime.Parse(x.Element("Date").Value).Year > 1980
                        select x;
            foreach (var item in query8.Distinct())
                Console.WriteLine("Full Name: {0}, Date Of Birthday: {1}", item.Element("FullName").Value, item.Element("Date").Value);
            Console.WriteLine();

            Console.WriteLine("9. Виведемо всіх акторів, котрі грали і в спектаклі, і в фільмі");
            var query9 = from x in xmlDoc.Root.Element("dataLinkActorAndPerfomance").Elements("DataLinkActorAndPerfomance")
                        join y in xmlDoc.Root.Element("dataLinkActorsAndMovies").Elements("DataLinkActorsAndMovies")
                        on  Int32.Parse(x.Element("IdOfActor").Value) equals Int32.Parse(y.Element("IdOfActor").Value) into temp
                        from t in temp
                        from z in xmlDoc.Root.Element("actors").Elements("Actor")
                        where Int32.Parse(t.Element("IdOfActor").Value) == Int32.Parse(z.Element("Id").Value)
                        select new { Id = z.Element("Id").Value, Name = z.Element("FullName").Value };
            foreach (var item in query9.Distinct())
                Console.WriteLine("Id:{0}, Actor:{1}", item.Id, item.Name);
            Console.WriteLine();

            Console.WriteLine("10. Виведемо всіх акторів, котрі не грали в спектаклі, але грали у фільмі");
            var query10_1 = from x in xmlDoc.Root.Element("dataLinkActorAndPerfomance").Elements("DataLinkActorAndPerfomance")
                          join y in xmlDoc.Root.Element("dataLinkActorsAndMovies").Elements("DataLinkActorsAndMovies") 
                          on Int32.Parse(x.Element("IdOfActor").Value) equals Int32.Parse(y.Element("IdOfActor").Value) into temp
                          from t in temp
                          select new { id = Int32.Parse(t.Element("IdOfActor").Value) };
            var query10_2 = from z in xmlDoc.Root.Element("actors").Elements("Actor")
                          select new { id = Int32.Parse(z.Element("Id").Value)};

            var query10_3 = from x in query10_2.Except(query10_1)
                            from y in xmlDoc.Root.Element("actors").Elements("Actor")
                            where x.id == int.Parse(y.Element("Id").Value)
                            select y;
            foreach (var item in query10_3)
                Console.WriteLine("Id:{0}", item.Element("FullName").Value);
            Console.WriteLine();

            Console.WriteLine("11. Виведемо всі фільми і спектаклі");
            var query11_1 = from x in xmlDoc.Root.Element("movies").Elements("Movie")
                           select new { Name = x.Element("Name").Value };
            var query11_2 = from y in xmlDoc.Root.Element("perfomances").Elements("Perfomance")
                           select new { Name = y.Element("Name").Value };
            foreach (var item in query11_2.Concat(query11_1))
                Console.WriteLine("Name:{0}", item.Name);
            Console.WriteLine();

            Console.WriteLine("12. Згрупуємо акторів по роках народження і порахуємо к-ть по роках");
            var query12 = from x in xmlDoc.Root.Element("actors").Elements("Actor")
                         group x by DateTime.Parse(x.Element("Date").Value).Year into g
                         let a = (g.Elements("Amplua").Nodes().Count())
                         where g.Any(x => x.Elements("Amplua").Nodes().Count() > 0)
                         select new
                         {
                             Key = g.Key,
                             cnt = g.Count(),
                         };
            foreach (var item in query12)
            {
                Console.WriteLine("Date of birthday:{0}, Counter: {1}  ", item.Key, item.cnt);
            }
            Console.WriteLine();

            Console.WriteLine("13. Згрупуємо вистави по жанрам та підрахуємо їх кількість");
            var query13 = from x in xmlDoc.Root.Element("perfomances").Elements("Perfomance")
                         group x by x.Element("Genre").Value into g
                         select new
                         {
                             Key = g.Key,
                             Values = g,
                             cnt = g.Count(),
                         };
            foreach (var item in query13)
            {
                Console.WriteLine("List of Amplua:{0}, Counter:{1}", item.Key, item.cnt);
            }
            Console.WriteLine();

            Console.WriteLine("14. Найдемо найстаріший фільм");
            var query14 = from x in xmlDoc.Root.Element("movies").Elements("Movie")
                          orderby DateTime.Parse(x.Element("DateOfIssue").Value).Year descending
                          select new
                          {
                              Name = x.Element("Name").Value,
                              year = DateTime.Parse(x.Element("DateOfIssue").Value).Year
                          };
                Console.WriteLine("Name and year:{0}", query14.Last());
            Console.WriteLine();

            Console.WriteLine("15. Найдемо актора який має більше всіх ролей");
            var query15 = from x in xmlDoc.Root.Element("actors").Elements("Actor")
                         join y in xmlDoc.Root.Element("dataLinkActorsAndMovies").Elements("DataLinkActorsAndMovies") 
                         on Int32.Parse(x.Element("Id").Value) equals Int32.Parse(y.Element("IdOfActor").Value) into temp
                         orderby temp.Count() descending
                         select new { Name = x.Element("FullName").Value, counter = temp.Count() };
                Console.WriteLine("Actor and counter:{0}", query15.First());
            Console.WriteLine();


            Console.WriteLine("Найдемо актора який має більше всіх ролей");
            foreach (var node in xmlDoc.Element("Cinema").Element("actors").Elements("Actor"))
            {

                var id = Int32.Parse(node.Element("Id").Value);
                var fullName = node.Element("FullName").Value.ToString();
                var dateOfBirthday = DateTime.Parse(node.Element("Date").Value);
                var amplua = node.Element("Amplua").Value;
                Console.WriteLine(string.Format("Id: {0}, Fullname: {1}, Date: {2}, Amplua: {3}", id, fullName, dateOfBirthday, amplua));
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(nameOfXmlFile);
            foreach (XmlNode node in doc.DocumentElement.GetElementsByTagName("Actor"))
            {
                var id = Int32.Parse(node["Id"].InnerText);
                var fullName = node["FullName"].InnerText;
                var dateOfBirthday = DateTime.Parse(node["Date"].InnerText);
                var amplua = node["Amplua"].InnerText;
                Console.WriteLine(string.Format("Id: {0}, Fullname: {1}, Date: {2}, Amplua: {3}", id, fullName, dateOfBirthday, amplua));
            }
        }
        static List<Actor> CreateListOfActors()
        {
            List<Actor> actors = new List<Actor>()
            {
               new Actor(idOfActor: 1, fullName: "Willard Smith Carroll",
                dateOfBirthday: new DateTime(1977, 12, 1), amplua: new List<string>{"Heroe", "Moralist" }),
                new Actor(idOfActor: 2, fullName: "Kunis Mila Markivna",
                dateOfBirthday: new DateTime(1977, 4, 11), amplua: new List<string>{"Loved", "Friend" }),
                new Actor(idOfActor: 3, fullName: "Clarke Emilia Rous",
                dateOfBirthday: new DateTime(1988, 10, 1), amplua: new List<string>{"Loved", "Courtesan", "Villain"}),
                new Actor(idOfActor: 4, fullName: "Chan Kong-sang",
                dateOfBirthday: new DateTime(1954, 4, 5), amplua: new List<string>{"Figth", "Guardian", "Moralist", "Friend"}),
                new Actor(idOfActor: 5, fullName: "Catherine Elise Blanchett",
                dateOfBirthday: new DateTime(1969, 5, 14), amplua: new List<string>{ "Promoting"}),
                new Actor(idOfActor: 6, fullName: "Jolie Angelina Voyt",
                dateOfBirthday: new DateTime(1975, 6, 4), amplua: new List<string>{ "Villain", "Guardian", "Loved"}),
                new Actor(idOfActor: 7, fullName: "Cassel Vincent Crochon",
                dateOfBirthday: new DateTime(1966, 10, 23), amplua: new List<string>{ "Scientist", "Moralist", "Heroe"})
            };
            return actors;
        }
        static List<Movie> CreateListOfMovie()
        {
            List<Movie> movies = new List<Movie>()
            {
                new Movie(idOfMovie: 1, name: "Harry Potter", dateOfIssue: new DateTime(2003, 10, 21),
                genreOfMovie:new List<string>{"Fantasy"}, fullNameOfDirector: "Woody Allen"),
                new Movie(idOfMovie: 2, name: "Black Panther", dateOfIssue: new DateTime(2018, 6, 12),
                genreOfMovie:new List<string>{"Action", "Adventure", "Fantasy"},fullNameOfDirector: "Thom Andersen"),
                new Movie(idOfMovie: 3, name: "Game of Thrones", dateOfIssue: new DateTime(2019, 12, 6),
                genreOfMovie:new List<string>{"Drama", "Adventure", "Fantasy"}, fullNameOfDirector: "Ryan Coogler"),
                new Movie(idOfMovie: 4, name: "Malificenta", dateOfIssue: new DateTime(2019, 5, 24),
                genreOfMovie:new List<string>{"Drama", "Fantasy"}, fullNameOfDirector:"John Mallory Asher"),
                new Movie(idOfMovie: 5, name: "Jackie Chan", dateOfIssue: new DateTime(2005, 8, 13),
                genreOfMovie:new List<string>{"Action", "Adventure", "Figth"}, fullNameOfDirector: "Broncho Billy Anderson"),
                  new Movie(idOfMovie: 6, name: "Cinnderela", dateOfIssue: new DateTime(2014, 3, 30),
                genreOfMovie:new List<string>{"Drama", "Adventure"}, fullNameOfDirector:"Jolie Angelina Voyt"),
                    new Movie(idOfMovie: 7, name: "Beauty and the Beast", dateOfIssue: new DateTime(2014, 5, 20),
                genreOfMovie:new List<string>{ "Fantasy", "Romance"}, fullNameOfDirector: "Cassel Vincent Crochon"),
                    new Movie(idOfMovie: 8, name: "Pirates of the Carybbean", dateOfIssue: new DateTime(2010, 10, 5),
                genreOfMovie:new List<string>{ "Fantasy", "Adventure"}, fullNameOfDirector: "Woody Allen")
            };
            return movies;
        }
        static List<DataLinkActorsAndMovie> CreateDataLinkActorsAndMovie()
        {
            List<DataLinkActorsAndMovie> dataLinkActorsAndMovies = new List<DataLinkActorsAndMovie>()
            {
                new DataLinkActorsAndMovie(idOfActorOnMovie:1, idOfActor:5, idOfMovies: 1),
                new DataLinkActorsAndMovie(idOfActorOnMovie:2, idOfActor:2, idOfMovies: 1),
                new DataLinkActorsAndMovie(idOfActorOnMovie:3, idOfActor:1, idOfMovies: 2),
                new DataLinkActorsAndMovie(idOfActorOnMovie:4, idOfActor:3, idOfMovies: 3),
                new DataLinkActorsAndMovie(idOfActorOnMovie:5, idOfActor:1, idOfMovies: 3),
                new DataLinkActorsAndMovie(idOfActorOnMovie:6, idOfActor:6, idOfMovies: 4),
                new DataLinkActorsAndMovie(idOfActorOnMovie:7, idOfActor:5, idOfMovies: 4),
                new DataLinkActorsAndMovie(idOfActorOnMovie:8, idOfActor:4, idOfMovies: 5),
                new DataLinkActorsAndMovie(idOfActorOnMovie:9, idOfActor:5, idOfMovies: 6),
                new DataLinkActorsAndMovie(idOfActorOnMovie:10, idOfActor:2, idOfMovies: 6),
                new DataLinkActorsAndMovie(idOfActorOnMovie:11, idOfActor:7, idOfMovies: 6),
                new DataLinkActorsAndMovie(idOfActorOnMovie:11, idOfActor:7, idOfMovies: 7),
                new DataLinkActorsAndMovie(idOfActorOnMovie:11, idOfActor:6, idOfMovies: 7),
            };
            return dataLinkActorsAndMovies;
        }
        static List<DataLinkActorsAndPerfomance> CreateDataLinkActorsAndPerfomance()
        {
            List<DataLinkActorsAndPerfomance> dataLinkActorsAndPerfomances = new List<DataLinkActorsAndPerfomance>
            {
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:1, idOfActor:1, idOfPerfomance: 1),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:2, idOfActor:3, idOfPerfomance: 1),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:3, idOfActor:6, idOfPerfomance: 2),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:4, idOfActor:5, idOfPerfomance: 2),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:5, idOfActor:7, idOfPerfomance: 3),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:6, idOfActor:3, idOfPerfomance: 4),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:7, idOfActor:4, idOfPerfomance: 4),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:7, idOfActor:5, idOfPerfomance: 4),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:8, idOfActor:4, idOfPerfomance: 5),
                new DataLinkActorsAndPerfomance(idOfActorOnPerfomance:8, idOfActor:7, idOfPerfomance: 5),

            };
            return dataLinkActorsAndPerfomances;
        }
        static List<Perfomance> CreateListOfPerfomance()
        {
            List<Perfomance> perfomances = new List<Perfomance>()
            {
                new Perfomance(idOfPerfomance:1, name: "Master and Margarita",
                genreOfPerfomance: "Drama"),
                new Perfomance(idOfPerfomance:2, name: "Shoot",
                genreOfPerfomance: "Adventure"),
                new Perfomance(idOfPerfomance:3, name: "Space X",
                genreOfPerfomance: "Scientific"),
                new Perfomance(idOfPerfomance:4, name: "Phoenix",
                genreOfPerfomance: "Drama"),
                 new Perfomance(idOfPerfomance:5, name: "Triumph",
                genreOfPerfomance: "Adventure"),
            };
            return perfomances;
        }
       
        static void CreateXMLFile(List<Actor> actors, List<Movie> movies, List<Perfomance> perfomances,
            List<DataLinkActorsAndMovie> dataLinkActorAndMovie,
            List<DataLinkActorsAndPerfomance> dataLinkActorsAndPerfomances, string nameOfXMLfile)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(nameOfXMLfile, settings))
            {
                writer.WriteStartElement("Cinema");
                writer.WriteStartElement("actors");
                foreach (var actor in actors)
                {
                    writer.WriteStartElement("Actor");
                    writer.WriteElementString("Id", actor.IdOfActor.ToString());
                    writer.WriteElementString("FullName", actor.FullName);
                    writer.WriteElementString("Date", actor.DateOfBirthday.ToLongDateString());
                    writer.WriteElementString("Amplua", actor.GetListOfAmplua());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("movies");
                foreach (var item in movies)
                {
                    writer.WriteStartElement("Movie");
                    writer.WriteElementString("Id", item.IdOfMovie.ToString());
                    writer.WriteElementString("Name", item.Name);
                    writer.WriteElementString("DateOfIssue", item.DateOfIssue.ToLongDateString());
                    writer.WriteElementString("FullNameOfDirector", item.FullNameOfDirector);
                    writer.WriteElementString("Genre", item.GetListOfGenre());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("perfomances");
                foreach (var item in perfomances)
                {
                    writer.WriteStartElement("Perfomance");
                    writer.WriteElementString("Id", item.IdOfPerfomance.ToString());
                    writer.WriteElementString("Name", item.Name);
                    writer.WriteElementString("Genre", item.GenreOfPerfomance);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("dataLinkActorsAndMovies");
                foreach (var item in dataLinkActorAndMovie)
                {
                    writer.WriteStartElement("DataLinkActorsAndMovies");
                    writer.WriteElementString("IdOfMovie", item.IdOfMovie.ToString());
                    writer.WriteElementString("IdOfActor", item.IdOfActor.ToString());
                    writer.WriteElementString("IdOfActorOnMovie", item.IdOfActorOnMovie.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("dataLinkActorAndPerfomance");
                foreach (var item in dataLinkActorsAndPerfomances)
                {
                    writer.WriteStartElement("DataLinkActorAndPerfomance");
                    writer.WriteElementString("IdOfPerfomance", item.IdOfPerfomance.ToString());
                    writer.WriteElementString("IdOfActor", item.IdOfActor.ToString());
                    writer.WriteElementString("IdOfActorOnPerfomance", item.IdOfActorOnPerfomance.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();

            }
        }
       

    }
}
