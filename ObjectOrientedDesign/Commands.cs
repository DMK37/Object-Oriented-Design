using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ObjectOrientedDesign
{
    [XmlInclude(typeof(ListCommandAuthor))]
    [XmlInclude(typeof(ListCommandEpisode))]
    [XmlInclude(typeof(ListCommandMovie))]
    [XmlInclude(typeof(ListCommandSeries))]
    [XmlInclude(typeof(FindCommand))]
    [XmlInclude(typeof(FindCommandAuthor))]
    [XmlInclude(typeof(FindCommandEpisode))]
    [XmlInclude(typeof(FindCommandMovie))]
    [XmlInclude(typeof(FindCommandSeries))]
    [XmlInclude(typeof(AddCommand))]
    [XmlInclude(typeof(AddCommandAuthor))]
    [XmlInclude(typeof(AddCommandMovie))]
    [XmlInclude(typeof(AddCommandEpisode))]
    [XmlInclude(typeof(AddCommandSeries))]
    [XmlInclude(typeof(EditCommand))]
    [XmlInclude(typeof(EditCommandAuthor))]
    [XmlInclude(typeof(EditCommandMovie))]
    [XmlInclude(typeof(EditCommandEpisode))]
    [XmlInclude(typeof(EditCommandSeries))]
    [XmlInclude(typeof(DeleteCommand))]
    [XmlInclude(typeof(DeleteCommandAuthor))]
    [XmlInclude(typeof(DeleteCommandEpisode))]
    [XmlInclude(typeof(DeleteCommandMovie))]
    [XmlInclude(typeof(DeleteCommandSeries))]
    [Serializable]
    public abstract class Command
    {
        protected static BitflixDb _bitflixDb = BitflixDb.Instance;

        //public string Input;
        public abstract void Execute();

        protected static T GetPropertyValue<T>(object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName)?.GetValue(obj, null);
        }
    }
    
    
    [Serializable]
    public class ListCommandAuthor : Command
    {
        private List<IAuthorAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetAuthors();
            foreach (var author in _list)
            {
                Console.WriteLine(author);
            }
        }

        public override string ToString()
        {
            return "list Author";
        }
    }

    [Serializable]
    public class ListCommandEpisode : Command
    {
        private List<IEpisodeAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetEpisodes();
            foreach (var episode in _list)
            {
                Console.WriteLine(episode);
            }
        }
        public override string ToString()
        {
            return "list Episode";
        }
    }

    [Serializable]
    public class ListCommandMovie : Command
    {
        private List<IMovieAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetMovies();
            foreach (var movie in _list)
            {
                Console.WriteLine(movie);
            }
        }

        public override string ToString()
        {
            return "list Movie";
        }
    }
    
    [Serializable]
    public class ListCommandSeries : Command
    {
        private List<ISeriesAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetSeries();
            foreach (var series in _list)
            {
                Console.WriteLine(series);
            }
        }

        public override string ToString()
        {
            return "list Series";
        }
    }

    [Serializable]
    public abstract class FindCommand : Command
    {
        protected void FindProcessing<T>(List<T> list, List<string> fields, Action<T> action)
        {
            string name = fields[1];
            //name = name.Remove(name.Length - 7);
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            int mode = 0;
            for (int i = 2; i < fields.Count; i++)
            {

                int k = fields[i].IndexOf('=');
                int j = fields[i].IndexOf('>');
                int s = fields[i].IndexOf('<');
                int tmp = int.MaxValue;
                if (k != -1 && k < tmp)
                {
                    mode = 0;
                    tmp = k;
                }

                if (j != -1 && j < tmp)
                {
                    mode = 1;
                    tmp = j;
                }

                if (s != -1 && s < tmp)
                {
                    mode = 2;
                    tmp = s;
                }

                k = tmp;
                names.Add(fields[i].Substring(0, k));
                if (fields[i][k + 1] == '\"')
                {
                    values.Add(fields[i].Substring(k + 2));
                    values[values.Count - 1] = values[values.Count - 1].Remove(values[values.Count - 1].Length - 1);
                }
                else
                {
                    values.Add(fields[i].Substring(k + 1));
                }
            }

            foreach (var adapter in list)
            {
                var o = adapter.GetType().GetMethod("To" + name)?.Invoke(adapter, null);

                bool isPrint = true;
                for (int i = 0; i < names.Count; i++)
                {
                    var val = GetPropertyValue<Object>(o, names[i]);
                    switch (val)
                    {
                        case string s:
                        {
                            switch (mode)
                            {
                                case 0:
                                {
                                    if (s != values[i])
                                        isPrint = false;
                                    break;
                                }
                                case 1:
                                {
                                    if (String.Compare(s, values[i], StringComparison.Ordinal) >= 0)
                                        isPrint = false;
                                    break;
                                }
                                case 2:
                                {
                                    if (String.Compare(s, values[i], StringComparison.Ordinal) <= 0)
                                        isPrint = false;
                                    break;
                                }
                            }

                            break;
                        }
                        case int j:
                        {
                            if (!int.TryParse(values[i], out var bth))
                            {
                                Console.WriteLine("Wrong input");
                                return;
                            }

                            switch (mode)
                            {
                                case 0:
                                {
                                    if (j != bth)
                                        isPrint = false;
                                    break;
                                }
                                case 1:
                                {
                                    if (j <= bth)
                                        isPrint = false;
                                    break;
                                }
                                case 2:
                                {
                                    if (j >= bth)
                                        isPrint = false;
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                if (isPrint)
                    action(adapter);
            }
        }
    }

    [Serializable]
    public class FindCommandAuthor : FindCommand
    {
        private List<IAuthorAdapter> _list;
        public List<string> fields;

        public override void Execute()
        {
            _list = _bitflixDb.GetAuthors();
            FindProcessing(_list, fields, Console.WriteLine);
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < fields.Count; i++)
            {
                res += fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class FindCommandEpisode : FindCommand
    {
        private List<IEpisodeAdapter> _list;
        public List<string> fields;

        public override void Execute()
        {
            _list = _bitflixDb.GetEpisodes();
            FindProcessing(_list, fields, Console.WriteLine);
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < fields.Count; i++)
            {
                res += fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class FindCommandMovie : FindCommand
    {
        private List<IMovieAdapter> _list;
        public List<string> fields;

        public override void Execute()
        {
            _list = _bitflixDb.GetMovies();
            FindProcessing(_list, fields, Console.WriteLine);
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < fields.Count; i++)
            {
                res += fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
        
    }

    [Serializable]
    public class FindCommandSeries : FindCommand
    {
        private List<ISeriesAdapter> _list;
        public List<string> fields;

        public override void Execute()
        {
            _list = _bitflixDb.GetSeries();
            FindProcessing(_list, fields, Console.WriteLine);
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < fields.Count; i++)
            {
                res += fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public abstract class AddCommand : Command
    {
        public ObjectBuildFactory objectBuild;
        public bool isDone;
    }

    [Serializable]
    public class AddCommandAuthor : AddCommand
    {
        public override void Execute()
        {
            var obj = objectBuild.Build();
            if (obj == null)
            {
                isDone = false;
                return;
            }

            isDone = true;
            _bitflixDb.Add((IAuthorAdapter)obj);
        }
        public override string ToString()
        {
            return objectBuild switch
            {
                AuthorBuilderBase basic => "add Author base",
                AuthorBuilderSecondary secondary => "add Author secondary",
                _ => ""
            };
        }
    }

    [Serializable]
    public class AddCommandEpisode : AddCommand
    {
        public override void Execute()
        {
            var obj = objectBuild.Build();
            if (obj == null)
            {
                isDone = false;
                return;
            }

            isDone = true;
            _bitflixDb.Add((IEpisodeAdapter)obj);
        }

        public override string ToString()
        {
            return objectBuild switch
            {
                EpisodeBuilderBase basic => "add Episode base",
                EpisodeBuilderSecondary secondary => "add Episode secondary",
                _ => ""
            };
        }
    }

    [Serializable]
    public class AddCommandMovie : AddCommand
    {
        public override void Execute()
        {
            var obj = objectBuild.Build();
            if (obj == null)
            {
                isDone = false;
                return;
            }

            isDone = true;
            _bitflixDb.Add((IMovieAdapter)obj);
        }
        
        public override string ToString()
        {
            switch (objectBuild)
            {
                case MovieBuilderBase basic:
                    return "add Movie base";
                    break;
                case MovieBuilderSecondary secondary:
                    return "add Movie secondary";
                    break;
            }

            return "";
        }

    }

    [Serializable]
    public class AddCommandSeries : AddCommand
    {
        public override void Execute()
        {
            var obj = objectBuild.Build();
            if (obj == null)
            {
                isDone = false;
                return;
            }

            isDone = true;
            _bitflixDb.Add((ISeriesAdapter)obj);
        }

        public override string ToString()
        {
            switch (objectBuild)
            {
                case SeriesBuilderBase basic:
                    return "add Series base";
                    break;
                case SeriesBuilderSecondary secondary:
                    return "add Series secondary";
                    break;
            }

            return "";
        }
    }

    [Serializable]
    public abstract class EditCommand : FindCommand
    {
        public List<string> Fields;
        public bool isDone;
    }

    [Serializable]
    public class EditCommandAuthor : EditCommand
    {
        private List<IAuthorAdapter> _list;
        public List<IAuthorAdapter> authors;
        public List<(string Name, string Surname, int BirthYear, int Awards)> prevFields;
        public AuthorBuilder authorBuilder;
        public override void Execute()
        {
            _list = _bitflixDb.GetAuthors();
            authors = new List<IAuthorAdapter>();
            prevFields = new List<(string , string, int, int)>();
            FindProcessing(_list, Fields, o =>
            {
                var a = o.ToAuthor();
                prevFields.Add((a.Name, a.Surname, a.BirthYear, a.Awards));
                authors.Add(o);
            });
            authorBuilder = new AuthorBuilderBase();
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Author"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");
            while (true)
            {
                string input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    isDone = true;
                    Console.WriteLine("[Authors Updated]");
                    foreach (var author in authors)
                    {
                        switch (author)
                        {
                            case Author ath:
                            {
                                if (authorBuilder.Name != "")
                                    ath.Name = authorBuilder.Name;
                                if (authorBuilder.Surname != "")
                                    ath.Surname = authorBuilder.Surname;
                                if (authorBuilder.BirthYear != -1)
                                    ath.BirthYear = authorBuilder.BirthYear;
                                if (authorBuilder.Awards != -1)
                                    ath.Awards = authorBuilder.Awards;
                                break;
                            }
                            case AuthorAdapter2 athAdapter:

                                var param = AuthorAdapter2.Parse(athAdapter._authorPartial.Info);
                                if (authorBuilder.Name != "")
                                    param[0] = authorBuilder.Name;
                                if (authorBuilder.Surname != "")
                                    param[1] = authorBuilder.Surname;
                                if (authorBuilder.BirthYear != -1)
                                    param[2] = authorBuilder.BirthYear.ToString();
                                if (authorBuilder.Awards != -1)
                                    param[3] = authorBuilder.Awards.ToString();
                                athAdapter._authorPartial.Info =
                                    $"{param[0]}+{param[1]}+{param[2]}^{param[3]}^";
                                break;
                        }
                    }

                    break;
                }

                if (input == "EXIT")
                {
                    isDone = false;
                    Console.WriteLine("[Authors update abandoned]");
                    break;
                }

                if (!authorBuilder.CheckInput(input))
                    Console.WriteLine("Wrong params");
            }
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Fields.Count; i++)
            {
                res += Fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class EditCommandEpisode : EditCommand
    {
        private List<IEpisodeAdapter> _list;
        public List<IEpisodeAdapter> episodes;
        public List<(string Title, int Duration, int ReleaseYear)> prevFields;
        public EpisodeBuilder episodeBuilder;

        public override void Execute()
        {
            _list = _bitflixDb.GetEpisodes();
            episodes = new List<IEpisodeAdapter>();
            prevFields = new List<(string Title, int Duration, int ReleaseYear)>();
            FindProcessing(_list, Fields, o =>
            {
                var e = o.ToEpisode();
                prevFields.Add((e.Title, e.Duration, e.ReleaseYear));
                episodes.Add(o);
            });
            episodeBuilder = new EpisodeBuilderBase();
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Episode"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                string input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    isDone = true;
                    Console.WriteLine("[Episodes Updated]");
                    foreach (var episode in episodes)
                    {
                        switch (episode)
                        {
                            case Episode ep:
                            {
                                if (episodeBuilder.Title != "")
                                    ep.Title = episodeBuilder.Title;
                                if (episodeBuilder.ReleaseYear != -1)
                                    ep.ReleaseYear = episodeBuilder.ReleaseYear;
                                if (episodeBuilder.Duration != -1)
                                    ep.Duration = episodeBuilder.Duration;
                                if (episodeBuilder.AuthorId != -1)
                                    ep.Director = _bitflixDb.GetAuthor(episodeBuilder.AuthorId);
                                break;
                            }
                            case EpisodeAdapter2 epAdapter:
                                if (episodeBuilder.Title != "")
                                    epAdapter._episodePartial.Title = episodeBuilder.Title;
                                if (episodeBuilder.ReleaseYear != -1)
                                    epAdapter._episodePartial.ReleaseYear = episodeBuilder.ReleaseYear;
                                if (episodeBuilder.Duration != -1)
                                    epAdapter._episodePartial.Duration = episodeBuilder.Duration;
                                if (episodeBuilder.AuthorId != -1)
                                    epAdapter._episodePartial.Director = episodeBuilder.AuthorId.ToString();
                                break;
                        }
                    }

                    break;
                }

                if (input == "EXIT")
                {
                    isDone = false;
                    Console.WriteLine("[Episodes update abandoned]");
                    break;
                }

                if (!episodeBuilder.CheckInput(input))
                    Console.WriteLine("Wrong params");

            }
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Fields.Count; i++)
            {
                res += Fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class EditCommandMovie : EditCommand
    {
        private List<IMovieAdapter> _list;
        public List<IMovieAdapter> movies;
        public List<(string Title, string Genre, int ReleaseYear, int Duration)> prevFields;
        public MovieBuilder movieBuilder;
        
        public override void Execute()
        {
            _list = _bitflixDb.GetMovies();
            movies = new List<IMovieAdapter>();
            prevFields = new List<(string Title, string Genre, int ReleaseYear, int Duration)>();
            FindProcessing(_list, Fields, o =>
            {
                var m = o.ToMovie();
                prevFields.Add((m.Title, m.Genre, m.ReleaseYear, m.Duration));
                movies.Add(o);
            });
            movieBuilder = new MovieBuilderBase();
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Movie"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                string input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    isDone = true;
                    Console.WriteLine("[Movies Updated]");
                    foreach (var movie in movies)
                    {
                        switch (movie)
                        {
                            case Movie mov:
                            {
                                if (movieBuilder.Title != "")
                                    mov.Title = movieBuilder.Title;
                                if (movieBuilder.Genre != "")
                                    mov.Genre = movieBuilder.Genre;
                                if (movieBuilder.ReleaseYear != -1)
                                    mov.ReleaseYear = movieBuilder.ReleaseYear;
                                if (movieBuilder.Duration != -1)
                                    mov.Duration = movieBuilder.Duration;
                                if (movieBuilder.AuthorId != -1)
                                    mov.Director = _bitflixDb.GetAuthor(movieBuilder.AuthorId);
                                break;
                            }
                            case MovieAdapter2 movAdapter:
                                var title = MovieAdapter2.ParseTitle(movAdapter._moviePartial.Title);
                                if (movieBuilder.ReleaseYear != -1)
                                    title[1] = movieBuilder.ReleaseYear.ToString();
                                if (movieBuilder.Title != " ")
                                    title[0] = movieBuilder.Title;
                                movAdapter._moviePartial.Title = $"{title[0]}({title[1]})";
                                if (movieBuilder.Genre != " ")
                                    movAdapter._moviePartial.Genre = movieBuilder.Genre;
                                if (movieBuilder.Duration != -1)
                                    movAdapter._moviePartial.Duration = movieBuilder.Duration;
                                if (movieBuilder.AuthorId != -1)
                                    movAdapter._moviePartial.Director = movieBuilder.AuthorId.ToString();
                                break;
                        }
                    }

                    break;
                }

                if (input == "EXIT")
                {
                    isDone = false;
                    Console.WriteLine("[Movies update abandoned]");
                    break;
                }

                if (!movieBuilder.CheckInput(input))
                    Console.WriteLine("Wrong params");

            }
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Fields.Count; i++)
            {
                res += Fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class EditCommandSeries : EditCommand
    {
        private List<ISeriesAdapter> _list;
        public List<ISeriesAdapter> series;
        public List<(string Title, string Genre)> prevFields;
        public SeriesBuilder seriesBuilder;
        public override void Execute()
        {
            _list = _bitflixDb.GetSeries();
            series = new List<ISeriesAdapter>();
            prevFields = new List<(string Title, string Genre)>();
            FindProcessing(_list, Fields, o =>
            {
                var s = o.ToSeries();
                prevFields.Add((s.Title, s.Genre));
                series.Add(o);
            });
            seriesBuilder = new SeriesBuilderBase();
            Console.Write($"[Available fields: ");
            foreach (var field in RegexControl.fieldsOfClasses["Series"])
            {
                Console.Write(field + " ");
            }

            Console.WriteLine("]");

            while (true)
            {
                string input = Console.In.ReadLine();
                if (input == "DONE")
                {
                    isDone = true;
                    Console.WriteLine("[Series Updated]");
                    foreach (var serial in series)
                    {
                        switch (serial)
                        {
                            case Series ser:
                            {
                                if (seriesBuilder.Title != " ")
                                    ser.Title = seriesBuilder.Title;
                                if (seriesBuilder.Genre != " ")
                                    ser.Genre = seriesBuilder.Genre;
                                if (seriesBuilder.AuthorId != -1)
                                    ser.Showrunner = _bitflixDb.GetAuthor(seriesBuilder.AuthorId);
                                break;
                            }
                            case SeriesAdapter2 serAdapter:
                                var title = SeriesAdapter2.ParseTitle(serAdapter._seriesPartial.Title);
                                if (seriesBuilder.Title != "")
                                    title[0] = seriesBuilder.Title;
                                if (seriesBuilder.Genre != "")
                                    title[1] = seriesBuilder.Genre;
                                if (seriesBuilder.AuthorId != -1)
                                    serAdapter._seriesPartial.Showrunner = seriesBuilder.AuthorId.ToString();
                                serAdapter._seriesPartial.Title = $"{title[0]}/{title[1]}";
                                break;
                        }
                    }

                    break;
                }

                if (input == "EXIT")
                {
                    isDone = false;
                    Console.WriteLine("[Series update abandoned]");
                    break;
                }

                if (!seriesBuilder.CheckInput(input))
                    Console.WriteLine("Wrong params");
            }
        }
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Fields.Count; i++)
            {
                res += Fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }
    
    [Serializable]
    public abstract class DeleteCommand : FindCommand
    {
        
        public List<Object> ToAdd = new List<Object>();
        
        public List<string> Fields;
        
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < Fields.Count; i++)
            {
                res += Fields[i] + " ";
            }

            res.Remove(res.Length - 1);
            return res;
        }
    }

    [Serializable]
    public class DeleteCommandAuthor : DeleteCommand
    {
        private List<IAuthorAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetAuthors();
            FindProcessing(_list, Fields, o =>
            {
                ToAdd.Add(o);
                _bitflixDb.Remove(o);
            });
        }
    }
    
    [Serializable]
    public class DeleteCommandMovie : DeleteCommand
    {
        private List<IMovieAdapter> _list;
        public override void Execute()
        {
            _list = _bitflixDb.GetMovies();
            FindProcessing(_list, Fields, o =>
            {
                ToAdd.Add(o);
                _bitflixDb.Remove(o);
            });
        }
    }
    
    [Serializable]
    public class DeleteCommandSeries : DeleteCommand
    {
        private List<ISeriesAdapter> _list;

        public override void Execute()
        {
            _list = _bitflixDb.GetSeries();
            FindProcessing(_list, Fields, o =>
            {
                ToAdd.Add(o);
                _bitflixDb.Remove(o);
            });
        }
    }
    
    [Serializable]
    public class DeleteCommandEpisode : DeleteCommand
    {
        private List<IEpisodeAdapter> _list;
        
        public override void Execute()
        {
            _list = _bitflixDb.GetEpisodes();
            FindProcessing(_list, Fields, o =>
            {
                ToAdd.Add(o);
                _bitflixDb.Remove(o);
            });
        }
    }
}