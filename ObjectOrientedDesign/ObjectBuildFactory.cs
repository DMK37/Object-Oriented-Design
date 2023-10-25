using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ObjectOrientedDesign
{
    [Serializable]
    [XmlInclude(typeof(AuthorBuilder))]
    [XmlInclude(typeof(EpisodeBuilder))]
    [XmlInclude(typeof(MovieBuilder))]
    [XmlInclude(typeof(SeriesBuilder))]
    [XmlInclude(typeof(AuthorBuilderBase))]
    [XmlInclude(typeof(AuthorBuilderSecondary))]
    [XmlInclude(typeof(MovieBuilderBase))]
    [XmlInclude(typeof(MovieBuilderSecondary))]
    [XmlInclude(typeof(EpisodeBuilderBase))]
    [XmlInclude(typeof(EpisodeBuilderSecondary))]
    [XmlInclude(typeof(SeriesBuilderBase))]
    [XmlInclude(typeof(SeriesBuilderSecondary))]
    public abstract class ObjectBuildFactory
    {
        protected static BitflixDb BitflixDb = BitflixDb.Instance;
        public abstract Object Build();
        public abstract bool CheckInput(string input);

        public abstract Object Create();
    }

    [Serializable]
    public abstract class AuthorBuilder : ObjectBuildFactory
    {
        public string Name = "";
        public string Surname = "";
        public int BirthYear = -1;
        public int Awards = -1;

        public override bool CheckInput(string input)
        {

            var pat = new Regex(@"^(Name|Surname|BirthYear|Awards)=((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$)");

            var mat = pat.Match(input);
            if (mat.Success)
            {
                string name;
                string value;
                int last = input.LastIndexOf('\"');
                if (last != -1)
                    input = input.Remove(last);
                int k = input.IndexOf('=');
                name = input.Substring(0, k);
                if (input[k + 1] == '\"')
                {
                    value = input.Substring(k + 2);
                }
                else
                {
                    value = input.Substring(k + 1);
                }

                switch (name)
                {
                    case "Name":
                        Name = value;
                        break;
                    case "Surname":
                        Surname = value;
                        break;
                    case "BirthYear":
                        if (int.TryParse(value, out var tmp))
                        {
                            BirthYear = tmp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                    case "Awards":
                        if (int.TryParse(value, out var tp))
                        {
                            Awards = tp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                }

                return true;
            }

            return false;
        }
    }

    [Serializable]
    public abstract class EpisodeBuilder : ObjectBuildFactory
    {
        public string Title = "";
        public int Duration = -1;
        public int ReleaseYear = -1;
        public int AuthorId = -1;

        public override bool CheckInput(string input)
        {
            var pat = new Regex(@"^(Title|Duration|ReleaseYear|AuthorId)=((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$)");
            var mat = pat.Match(input);
            if (mat.Success)
            {
                string name;
                string value;
                int last = input.LastIndexOf('\"');
                if (last != -1)
                    input = input.Remove(last);
                int k = input.IndexOf('=');
                name = input.Substring(0, k);
                if (input[k + 1] == '\"')
                {
                    value = input.Substring(k + 2);
                }
                else
                {
                    value = input.Substring(k + 1);
                }

                switch (name)
                {
                    case "Title":
                        Title = value;
                        break;
                    case "Duration":
                        if (int.TryParse(value, out var tmp))
                        {
                            Duration = tmp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                    case "ReleaseYear":
                        if (int.TryParse(value, out var tp))
                        {
                            ReleaseYear = tp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                    case "AuthorId":
                        if (int.TryParse(value, out var t))
                        {
                            AuthorId = t;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                }

                return true;
            }

            return false;
        }
    }

    [Serializable]
    public abstract class MovieBuilder : ObjectBuildFactory
    {
        public string Title = " ";
        public string Genre = " ";
        public int ReleaseYear = -1;
        public int Duration = -1;
        public int AuthorId = -1;

        public override bool CheckInput(string input)
        {
            var pat = new Regex(
                @"^(Title|Genre|Duration|ReleaseYear|AuthorId)=((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$)");
            var mat = pat.Match(input);
            if (mat.Success)
            {
                string name;
                string value;
                int last = input.LastIndexOf('\"');
                if (last != -1)
                    input = input.Remove(last);
                int k = input.IndexOf('=');
                name = input.Substring(0, k);
                if (input[k + 1] == '\"')
                {
                    value = input.Substring(k + 2);
                }
                else
                {
                    value = input.Substring(k + 1);
                }

                switch (name)
                {

                    case "Title":
                        Title = value;
                        break;
                    case "Genre":
                        Genre = value;
                        break;
                    case "Duration":
                        int tmp;
                        if (int.TryParse(value, out tmp))
                        {
                            Duration = tmp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                    case "ReleaseYear":
                        int tp;
                        if (int.TryParse(value, out tp))
                        {
                            ReleaseYear = tp;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                    case "AuthorId":
                        if (int.TryParse(value, out var t))
                        {
                            AuthorId = t;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                }

                return true;
            }

            return false;
        }
    }

    [Serializable]
    public abstract class SeriesBuilder : ObjectBuildFactory
    {
        public string Title = " ";
        public string Genre = " ";
        public int AuthorId = -1;

        public override bool CheckInput(string input)
        {
            var pat = new Regex(@"^(Title|Genre|AuthorId)=((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$)");
            var mat = pat.Match(input);
            if (mat.Success)
            {
                string name;
                string value;
                int last = input.LastIndexOf('\"');
                if (last != -1)
                    input = input.Remove(last);
                int k = input.IndexOf('=');
                name = input.Substring(0, k);
                if (input[k + 1] == '\"')
                {
                    value = input.Substring(k + 2);
                }
                else
                {
                    value = input.Substring(k + 1);
                }

                switch (name)
                {
                    case "Title":
                        Title = value;
                        break;
                    case "Genre":
                        Genre = value;
                        break;
                    case "AuthorId":
                        if (int.TryParse(value, out var t))
                        {
                            AuthorId = t;
                        }
                        else
                        {
                            Console.WriteLine("Input not int");
                        }

                        break;
                }

                return true;
            }

            return false;
        }
    }

    [Serializable]
    public class AuthorBuilderBase : AuthorBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Author"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    Console.WriteLine("[Author Created]");
                    
                    return new Author(Name, Surname, BirthYear, Awards);
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Author creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");
                
            }

            return null;
        }

        public override object Create()
        {
            return new Author(Name, Surname, BirthYear, Awards);
        }
    }

    [Serializable]
    public class EpisodeBuilderBase : EpisodeBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Episode"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();


                if (input == "DONE")
                {
                    Console.WriteLine("[Episode Created]");
                    Author ath;
                    if (AuthorId != -1)
                        ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
                    else
                        ath = BitflixDb.GetAuthor(0).ToAuthor();
                     
                    return new Episode(Title, Duration, ReleaseYear, ref ath);
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Episode creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        public override object Create()
        {
            Author ath;
            if (AuthorId != -1)
                ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
            else
                ath = BitflixDb.GetAuthor(0).ToAuthor();
            return new Episode(Title, Duration, ReleaseYear, ref ath);
        }
    }

    [Serializable]
    public class MovieBuilderBase : MovieBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Movie"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();

                if (input == "DONE")
                {
                    Console.WriteLine("[Movie Created]");
                    Author ath;
                    if (AuthorId != -1)
                        ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
                    else
                        ath = BitflixDb.GetAuthor(0).ToAuthor();
                    
                    return new Movie(Title, Genre, ref ath, Duration, ReleaseYear);
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Movie creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        
        public override object Create()
        {
            Author ath;
            if (AuthorId != -1)
                ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
            else
                ath = BitflixDb.GetAuthor(0).ToAuthor();
            return new Movie(Title, Genre, ref ath, Duration, ReleaseYear);
        }
    }

    [Serializable]
    public class SeriesBuilderBase : SeriesBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Series"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();

                if (input == "DONE")
                {
                    Console.WriteLine("[Series Created]");
                    Author ath;
                    if (AuthorId != -1)
                        ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
                    else
                        ath = BitflixDb.GetAuthor(0).ToAuthor();
                    
                    return new Series(Title, Genre, ref ath, new List<Episode>());
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Series creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        
        public override object Create()
        {
            Author ath;
            if (AuthorId != -1)
                ath = BitflixDb.GetAuthor(AuthorId).ToAuthor();
            else
                ath = BitflixDb.GetAuthor(0).ToAuthor();
            return new Series(Title, Genre, ref ath, new List<Episode>());
        }
    }

    [Serializable]
    public class AuthorBuilderSecondary : AuthorBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Author"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    Console.WriteLine("[Author Created]");
                    
                    return new AuthorAdapter2(new AuthorPartial($"{Name}+{Surname}+{BirthYear}^{Awards}^"));
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Author creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        
        public override object Create()
        {
            return new AuthorAdapter2(new AuthorPartial($"{Name}+{Surname}+{BirthYear}^{Awards}^"));
        }
    }

    [Serializable]
    public class EpisodeBuilderSecondary : EpisodeBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Episode"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();


                if (input == "DONE")
                {
                    Console.WriteLine("[Episode Created]");
                    int athid = (AuthorId == -1) ? 0 : AuthorId;
                    return new EpisodeAdapter2(new EpisodePartial(Title, Duration, ReleaseYear, athid.ToString()));
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Episode creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        
        public override object Create()
        {
            int athid = (AuthorId == -1) ? 0 : AuthorId;
            return new EpisodeAdapter2(new EpisodePartial(Title, Duration, ReleaseYear, athid.ToString()));
        }
    }

    [Serializable]
    public class MovieBuilderSecondary : MovieBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Movie"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();

                if (input == "DONE")
                {
                    Console.WriteLine("[Movie Created]");
                    int athid = (AuthorId == -1) ? 0 : AuthorId;
                    return new MovieAdapter2(new MoviePartial(Title, Genre, athid.ToString(), Duration));
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Movie creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");

            }

            return null;
        }
        
        public override object Create()
        {
            int athid = (AuthorId == -1) ? 0 : AuthorId;
            return new MovieAdapter2(new MoviePartial(Title, Genre, athid.ToString(), Duration));
        }
    }

    [Serializable]
    public class SeriesBuilderSecondary : SeriesBuilder
    {
        public override Object Build()
        {
            string input;
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Series"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                input = Console.In.ReadLine();

                if (input == "DONE")
                {
                    Console.WriteLine("[Series Created]");
                    int athid = (AuthorId == -1) ? 0 : AuthorId;
                    
                    return new SeriesAdapter2(new SeriesPartial($"{Title}/{Genre}", athid.ToString(), ""));
                    break;
                }

                if (input == "EXIT")
                {
                    Console.WriteLine("[Series creation abandoned]");
                    break;
                }

                if (!CheckInput(input))
                    Console.WriteLine("Wrong params");
            }

            return null;
        }
        
        public override object Create()
        {
            int athid = (AuthorId == -1) ? 0 : AuthorId;
            return new SeriesAdapter2(new SeriesPartial($"{Title}/{Genre}", athid.ToString(), ""));
        }
    }
}