using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ObjectOrientedDesign
{
    public class CommandQueue
    {
        private Queue<Command> commandQueue;
        public List<Command> listedQueue;
        public Stack<Command> undo;
        public Stack<Command> redo;
        public CommandQueue()
        {
            commandQueue = new Queue<Command>();
            listedQueue = new List<Command>();
            undo = new Stack<Command>();
            redo = new Stack<Command>();
        }

        public void Exec()
        {
            while (commandQueue.Count > 0)
            {
                var tuple = commandQueue.Dequeue();
                tuple.Execute();
                undo.Push(tuple);
            }
        }

        public void Add(string type, string input)
        {
            Command command = null;
            switch (type)
            {
                case "list":
                    var check = RegexControl.ListCheck(input);
                    if (check.Item1)
                    {
                        command = check.Item2[1] switch
                        {
                            "Author" => new ListCommandAuthor(),
                            "Movie" => new ListCommandMovie(),
                            "Series" => new ListCommandSeries(),
                            "Episode" => new ListCommandEpisode(),
                            _ => null
                        };
                    }
                    break;
                case "find":
                    check = RegexControl.FindCheck(input);
                    if (check.Item1)
                    {
                        command = check.Item2[1] switch
                        {
                            "Author" => new FindCommandAuthor
                            {
                                fields = check.Item2
                            },
                            "Movie" => new FindCommandMovie
                            {
                                fields = check.Item2
                            },
                            "Series" => new FindCommandSeries
                            {
                                fields = check.Item2
                            },
                            "Episode" => new FindCommandEpisode
                            {
                                fields = check.Item2
                            },
                            _ => null
                        };
                    }
                    break;
                case "add":
                    var ch = RegexControl.AddCheck(input);
                    if (ch.Item1)
                    {
                        switch (ch.Item2)
                        {
                            case "Author":
                                switch (ch.Item3)
                                {
                                    case "base":
                                        command = new AddCommandAuthor
                                        {
                                            objectBuild = (AuthorBuilderBase)ch.Item4
                                        };
                                        break;
                                    case "secondary":
                                        command = new AddCommandAuthor
                                        {
                                            objectBuild = (AuthorBuilderSecondary)ch.Item4
                                        };
                                        break;
                                }
                                break;
                            case "Movie":
                                switch (ch.Item3)
                                {
                                    case "base":
                                        command = new AddCommandMovie
                                        {
                                            objectBuild = (MovieBuilderBase)ch.Item4
                                        };
                                        break;
                                    case "secondary":
                                        command = new AddCommandMovie
                                        {
                                            objectBuild = (MovieBuilderSecondary)ch.Item4
                                        };
                                        break;
                                }
                                break;
                            case "Episode":
                                switch (ch.Item3)
                                {
                                    case "base":
                                        command = new AddCommandEpisode
                                        {
                                            objectBuild = (EpisodeBuilderBase)ch.Item4
                                        };
                                        break;
                                    case "secondary":
                                        command = new AddCommandEpisode
                                        {
                                            objectBuild = (EpisodeBuilderSecondary)ch.Item4
                                        };
                                        break;
                                }
                                break;
                            case "Series":
                                switch (ch.Item3)
                                {
                                    case "base":
                                        command = new AddCommandSeries
                                        {
                                            objectBuild = (SeriesBuilderBase)ch.Item4
                                        };
                                        break;
                                    case "secondary":
                                        command = new AddCommandSeries
                                        {
                                            objectBuild = (SeriesBuilderSecondary)ch.Item4
                                        };
                                        break;
                                }
                                break;
                        }
                        undo.Push(command);
                    }
                    break;
                case "edit":
                    check = RegexControl.EditCheck(input);
                    if (check.Item1)
                    {
                        command = check.Item2[1] switch
                        {
                            "Author" => new EditCommandAuthor
                            {
                                Fields = check.Item2
                            },
                            "Movie" => new EditCommandMovie
                            {
                                Fields = check.Item2
                            },
                            "Series" => new EditCommandSeries
                            {
                                Fields = check.Item2
                            },
                            "Episode" => new EditCommandEpisode
                            {
                                Fields = check.Item2
                            },
                            _ => null
                        };
                        undo.Push(command);
                    }
                    break;
                case "delete":
                    check = RegexControl.DeleteCheck(input);
                    if (check.Item1)
                    {
                        command = check.Item2[1] switch
                        {
                            "Author" => new DeleteCommandAuthor
                            {
                                Fields = check.Item2
                            },
                            "Movie" => new DeleteCommandMovie
                            {
                                Fields = check.Item2
                            },
                            "Series" => new DeleteCommandSeries
                            {
                                Fields = check.Item2
                            },
                            "Episode" => new DeleteCommandEpisode
                            {
                                Fields = check.Item2
                            },
                            _ => null
                        };
                        undo.Push(command);
                    }
                    
                    break;
            }

            if (command != null)
            { 
               listedQueue.Add(command);
               command.Execute();
            }
            
        }

        public void Undo()
        {
            if(undo.Count <= 0) return;
            Command command = undo.Pop();
            redo.Push(command);
            switch (command)
            {
                case AddCommand addCommand:
                    switch (addCommand)
                    {
                        case AddCommandAuthor ath:

                            if(ath.isDone)
                                BitflixDb.Instance.Remove((IAuthorAdapter)ath.objectBuild.Create());
                            break;
                        case AddCommandEpisode eps:
                            if(eps.isDone)
                                BitflixDb.Instance.Remove((IEpisodeAdapter)eps.objectBuild.Create());
                            break;
                        case AddCommandMovie mov:
                            if(mov.isDone)
                                BitflixDb.Instance.Remove((IMovieAdapter)mov.objectBuild.Create());
                            break;
                        case AddCommandSeries ser:
                            if(ser.isDone)
                                BitflixDb.Instance.Remove((ISeriesAdapter)ser.objectBuild.Create());
                            break;
                    }
                    break;
                case DeleteCommand deleteCommand:
                    switch (deleteCommand)
                    {
                        case DeleteCommandAuthor ath:
                            foreach (var auth in ath.ToAdd)
                            {
                                BitflixDb.Instance.Add((IAuthorAdapter)auth);
                            }
                            break;
                        case DeleteCommandEpisode eps:
                            foreach (var ep in eps.ToAdd)
                            {
                                BitflixDb.Instance.Add((IEpisodeAdapter)ep);
                            }
                            break;
                        case DeleteCommandMovie mov:
                            foreach (var m in mov.ToAdd)
                            {
                                BitflixDb.Instance.Add((IMovieAdapter)m);
                            }
                            break;
                        case DeleteCommandSeries series:
                            foreach (var s in series.ToAdd)
                            {
                                BitflixDb.Instance.Add((ISeriesAdapter)s);
                            }
                            break;
                    }
                    break;
                case EditCommand editCommand:
                    switch (editCommand)
                    {
                        case EditCommandAuthor author:
                            if (author.isDone)
                            {
                                var list = BitflixDb.Instance.GetAuthors();
                                for (int i = 0; i < author.authors.Count; i++)
                                {
                                    foreach (var ath in list)
                                    {
                                        if (Equals(ath.ToAuthor(), author.authors[i].ToAuthor()))
                                        {
                                            switch (ath)
                                            {
                                                case Author a:
                                                    a.Name = author.prevFields[i].Name;
                                                    a.Surname = author.prevFields[i].Surname;
                                                    a.BirthYear = author.prevFields[i].BirthYear;
                                                    a.Awards = author.prevFields[i].Awards;
                                                    break;
                                                case AuthorAdapter2 adapter2:
                                                    adapter2._authorPartial.Info =
                                                        $"{author.prevFields[i].Name}+{author.prevFields[i].Surname}+{author.prevFields[i].BirthYear}^{author.prevFields[i].Awards}^";
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case EditCommandEpisode episode:
                            var list2 = BitflixDb.Instance.GetEpisodes();
                            for (int i = 0; i < episode.episodes.Count; i++)
                            {
                                foreach (var eps in list2)
                                {
                                    if (Equals(eps.ToEpisode(), episode.episodes[i].ToEpisode()))
                                    {
                                        switch (eps)
                                        {
                                            case Episode a:
                                                a.Title = episode.prevFields[i].Title;
                                                a.ReleaseYear = episode.prevFields[i].ReleaseYear;
                                                a.Duration = episode.prevFields[i].Duration;
                                                break;
                                            case EpisodeAdapter2 adapter2:
                                                adapter2._episodePartial.Title = episode.prevFields[i].Title;
                                                adapter2._episodePartial.ReleaseYear = episode.prevFields[i].ReleaseYear;
                                                adapter2._episodePartial.Duration = episode.prevFields[i].Duration;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        case EditCommandMovie movie:
                            var list3 = BitflixDb.Instance.GetMovies();
                            for (int i = 0; i < movie.movies.Count; i++)
                            {
                                foreach (var mov in list3)
                                {
                                    if (Equals(mov.ToMovie(), movie.movies[i].ToMovie()))
                                    {
                                        switch (mov)
                                        {
                                            case Movie a:
                                                a.Title = movie.prevFields[i].Title;
                                                a.ReleaseYear = movie.prevFields[i].ReleaseYear;
                                                a.Duration = movie.prevFields[i].Duration;
                                                a.Genre = movie.prevFields[i].Genre;
                                                break;
                                            case MovieAdapter2 adapter2:
                                                adapter2._moviePartial.Title = $"{movie.prevFields[i].Title}({movie.prevFields[i].ReleaseYear})";
                                                adapter2._moviePartial.Duration = movie.prevFields[i].ReleaseYear;
                                                adapter2._moviePartial.Genre = movie.prevFields[i].Genre;
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        case EditCommandSeries series:
                            var list4 = BitflixDb.Instance.GetSeries();
                            for (int i = 0; i < series.series.Count; i++)
                            {
                                foreach (var ser in list4)
                                {
                                    if (Equals(ser.ToSeries(), series.series[i].ToSeries()))
                                    {
                                        switch (ser)
                                        {
                                            case Series a:
                                                a.Title = series.prevFields[i].Title;
                                                a.Genre = series.prevFields[i].Genre;
                                                break;
                                            case SeriesAdapter2 adapter2:
                                                adapter2._seriesPartial.Title = $"{series.prevFields[i].Title}/{series.prevFields[i].Genre}";
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public void Redo()
        {
            if(redo.Count <= 0) return;
            Command command = redo.Pop();
            switch (command)
            {
                case AddCommand addCommand:
                    switch (addCommand)
                    {
                        case AddCommandAuthor ath:

                            if(ath.isDone)
                                BitflixDb.Instance.Add((IAuthorAdapter)ath.objectBuild.Create());
                            break;
                        case AddCommandEpisode eps:
                            if(eps.isDone)
                                BitflixDb.Instance.Add((IEpisodeAdapter)eps.objectBuild.Create());
                            break;
                        case AddCommandMovie mov:
                            if(mov.isDone)
                                BitflixDb.Instance.Add((IMovieAdapter)mov.objectBuild.Create());
                            break;
                        case AddCommandSeries ser:
                            if(ser.isDone)
                                BitflixDb.Instance.Add((ISeriesAdapter)ser.objectBuild.Create());
                            break;
                    }
                    break;
                case DeleteCommand deleteCommand:
                    switch (deleteCommand)
                    {
                        case DeleteCommandAuthor ath:
                            foreach (var auth in ath.ToAdd)
                            {
                                BitflixDb.Instance.Remove((IAuthorAdapter)auth);
                            }
                            break;
                        case DeleteCommandEpisode eps:
                            foreach (var ep in eps.ToAdd)
                            {
                                BitflixDb.Instance.Remove((IEpisodeAdapter)ep);
                            }
                            break;
                        case DeleteCommandMovie mov:
                            foreach (var m in mov.ToAdd)
                            {
                                BitflixDb.Instance.Remove((IMovieAdapter)m);
                            }
                            break;
                        case DeleteCommandSeries series:
                            foreach (var s in series.ToAdd)
                            {
                                BitflixDb.Instance.Remove((ISeriesAdapter)s);
                            }
                            break;
                    }
                    break;
                case EditCommand editCommand:
                    switch (editCommand)
                    {
                        case EditCommandAuthor author:
                            if (author.isDone)
                            {
                                foreach (var ath in author.authors)
                                {
                                    switch (ath)
                                    {
                                        case Author a:
                                            if (author.authorBuilder.Name != "")
                                                a.Name = author.authorBuilder.Name;
                                            if (author.authorBuilder.Surname != "")
                                                a.Surname = author.authorBuilder.Surname;
                                            if (author.authorBuilder.BirthYear != -1)
                                                a.BirthYear = author.authorBuilder.BirthYear;
                                            if (author.authorBuilder.Awards != -1)
                                                a.Awards = author.authorBuilder.Awards;

                                            break;
                                        case AuthorAdapter2 adapter2:
                                            var param = AuthorAdapter2.Parse(adapter2._authorPartial.Info);
                                            if (author.authorBuilder.Name != "")
                                                param[0] = author.authorBuilder.Name;
                                            if (author.authorBuilder.Surname != "")
                                                param[1] = author.authorBuilder.Surname;
                                            if (author.authorBuilder.BirthYear != -1)
                                                param[2] = author.authorBuilder.BirthYear.ToString();
                                            if (author.authorBuilder.Awards != -1)
                                                param[3] = author.authorBuilder.Awards.ToString();
                                            adapter2._authorPartial.Info =
                                                $"{param[0]}+{param[1]}+{param[2]}^{param[3]}^";

                                            break;
                                    }
                                }
                            }
                            break;
                        case EditCommandEpisode episode:
                            if (episode.isDone)
                            {
                                foreach (var eps in episode.episodes)
                                {
                                    switch (eps)
                                    {
                                        case Episode a:
                                            if (episode.episodeBuilder.Title != "")
                                                a.Title = episode.episodeBuilder.Title;
                                            if (episode.episodeBuilder.ReleaseYear != -1)
                                                a.ReleaseYear = episode.episodeBuilder.ReleaseYear;
                                            if (episode.episodeBuilder.Duration != -1)
                                                a.Duration = episode.episodeBuilder.Duration;

                                            
                                            break;
                                        case EpisodeAdapter2 adapter2:
                                            if (episode.episodeBuilder.Title != "")
                                                adapter2._episodePartial.Title = episode.episodeBuilder.Title;
                                            if (episode.episodeBuilder.ReleaseYear != -1)
                                                adapter2._episodePartial.ReleaseYear = episode.episodeBuilder.ReleaseYear;
                                            if (episode.episodeBuilder.Duration != -1)
                                                adapter2._episodePartial.Duration = episode.episodeBuilder.Duration;
                                            break;
                                    }
                                }
                            }
                            break;
                        case EditCommandMovie movie:
                            if (movie.isDone)
                            {
                                foreach (var mov in movie.movies)
                                {
                                    switch (mov)
                                    {
                                        case Movie a:
                                            if (movie.movieBuilder.Title != "")
                                                a.Title = movie.movieBuilder.Title;
                                            if (movie.movieBuilder.Genre != "")
                                                a.Genre = movie.movieBuilder.Genre;
                                            if (movie.movieBuilder.ReleaseYear != -1)
                                                a.ReleaseYear = movie.movieBuilder.ReleaseYear;
                                            if (movie.movieBuilder.Duration != -1)
                                                a.Duration = movie.movieBuilder.Duration;
                                            
                                            break;
                                        case MovieAdapter2 adapter2:
                                            var title = MovieAdapter2.ParseTitle(adapter2._moviePartial.Title);
                                            if (movie.movieBuilder.ReleaseYear != -1)
                                                title[1] = movie.movieBuilder.ReleaseYear.ToString();
                                            if (movie.movieBuilder.Title != " ")
                                                title[0] = movie.movieBuilder.Title;
                                            adapter2._moviePartial.Title = $"{title[0]}({title[1]})";
                                            if (movie.movieBuilder.Genre != " ")
                                                adapter2._moviePartial.Genre = movie.movieBuilder.Genre;
                                            if (movie.movieBuilder.Duration != -1)
                                                adapter2._moviePartial.Duration = movie.movieBuilder.Duration;
                                            
                                            break;
                                    }
                                }
                            }
                            break;
                        case EditCommandSeries series:
                            if (series.isDone)
                            {
                                foreach (var ser in series.series)
                                {
                                    switch (ser)
                                    {
                                        case Series a:
                                            if (series.seriesBuilder.Title != " ")
                                                a.Title = series.seriesBuilder.Title;
                                            if (series.seriesBuilder.Genre != " ")
                                                a.Genre = series.seriesBuilder.Genre;
                                            break;
                                        case SeriesAdapter2 adapter2:
                                            var title = SeriesAdapter2.ParseTitle(adapter2._seriesPartial.Title);
                                            if (series.seriesBuilder.Title != "")
                                                title[0] = series.seriesBuilder.Title;
                                            if (series.seriesBuilder.Genre != "")
                                                title[1] = series.seriesBuilder.Genre;
                                            adapter2._seriesPartial.Title = $"{title[0]}/{title[1]}";
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        public void History()
        {
            foreach (var tuple in undo)
            {
                Console.WriteLine(tuple);
            }
        }

        public void Serialize(string filename)
        {
            using (var sw = new StreamWriter(filename))
            {
                XmlSerializer x = new XmlSerializer(this.GetType());
                x.Serialize(sw, this);
            }
        }

        public void PlainText(string filename)
        {
            using var writer = new StreamWriter(filename);
            foreach (var cmd in listedQueue)
            {
                writer.WriteLine(cmd);
            }
        }

        /*public void Dismiss()
        {
            commandQueue.Clear();
            listedQueue.Clear();
        }*/

        public void Load(string filename)
        {
            if(filename.Length < 5)
                Console.WriteLine("no file extension");
            if (filename.Substring(filename.Length - 4) == ".xml")
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(CommandQueue));

                // Declare an object variable of the type to be deserialized.
                CommandQueue i;

                using (Stream reader = new FileStream(filename, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    i = (CommandQueue)serializer.Deserialize(reader);
                }

                foreach (var cmd in i.listedQueue)
                {
                    commandQueue.Enqueue(cmd);
                    listedQueue.Add(cmd);
                }
                Exec();
            }

            if (filename.Substring(filename.Length - 4) == ".txt")
            {
                foreach (string line in File.ReadLines(filename))
                {
                    string[] parser = line.Split(' ');
                    switch (parser[0])
                    {
                        case "list":
                            Add("list", line);
                            break;
                        case "find":
                            Add("find", line);
                            break;
                        case "add":
                            Add("add", line);
                            break;
                        case "edit":
                            Add("edit", line);
                            break;
                        case "delete":
                            Add("delete", line);
                            break;
                    }
                }
            }
            
        }
    }
}