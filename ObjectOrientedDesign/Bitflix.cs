using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;

namespace ObjectOrientedDesign
{
    public sealed class BitflixDb
    {
        private static BitflixDb _instance;

        private static int AthId;
        private static int SerId;
        private static int EpId;
        private static int MovId;

        public static int SetAthId()
        {
            return AthId++;
        }

        public static int SetSerId()
        {
            return SerId++;
        }

        public static int SetEpId()
        {
            return EpId++;
        }

        public static int SetMovId()
        {
            return MovId++;
        }

        public static BitflixDb Instance => _instance ??= new BitflixDb();

        private BitflixDb()
        {
            AthId = 0;
            SerId = 0;
            EpId = 0;
            MovId = 0;
        }

        private Dictionary<int, IAuthorAdapter> AuthorsDict = new Dictionary<int, IAuthorAdapter>();
        private Dictionary<int, ISeriesAdapter> SeriesDict = new Dictionary<int, ISeriesAdapter>();
        private Dictionary<int, IEpisodeAdapter> EpisodesDict = new Dictionary<int, IEpisodeAdapter>();
        private Dictionary<int, IMovieAdapter> MoviesDict = new Dictionary<int, IMovieAdapter>();

        public List<Episode> GetEpisodes(List<int> ids)
        {
            List<Episode> episodes = new List<Episode>(ids.Count);
            foreach (var id in ids)
            {
                episodes.Add(EpisodesDict[id].ToEpisode());
            }

            return episodes;
        }

        public void Task2(Dictionary<int, IMovieAdapter> dictionary, Dictionary<int, IEpisodeAdapter> dictionary2)
        {
            var res = dictionary.Where(pair =>
            {
                Movie movie;
                if (pair.Value is MovieAdapter movieIndex)
                {
                    movie = movieIndex.ToMovie();
                }
                else
                {
                    if (pair.Value is MovieAdapter2 moviePartial)
                    {
                        movie = moviePartial.ToMovie();
                    }
                    else movie = (Movie)pair.Value;
                }

                return movie.Director.BirthYear >= 1970;
            }).Select(pair => pair.Value);

            foreach (var mov in res)
            {
                Movie movie;
                if (mov is MovieAdapter movieIndex)
                {
                    movie = movieIndex.ToMovie();
                }
                else
                {
                    if (mov is MovieAdapter2 moviePartial)
                    {
                        movie = moviePartial.ToMovie();
                    }
                    else movie = (Movie)mov;
                }

                Console.WriteLine(movie);
            }

            var res2 = dictionary2.Where(pair =>
            {
                Episode episode;
                if (pair.Value is EpisodeAdapter episodeIndex)
                {
                    episode = episodeIndex.ToEpisode();
                }
                else
                {
                    if (pair.Value is EpisodeAdapter2 episodePartial)
                    {
                        episode = episodePartial.ToEpisode();
                    }
                    else episode = (Episode)pair.Value;
                }

                return episode.Director.BirthYear >= 1970;
            }).Select(pair => pair.Value);
            foreach (var eps in res2)
            {
                Episode episode;
                if (eps is EpisodeAdapter episodeIndex)
                {
                    episode = episodeIndex.ToEpisode();
                }
                else
                {
                    if (eps is EpisodeAdapter2 episodePartial)
                    {
                        episode = episodePartial.ToEpisode();
                    }
                    else episode = (Episode)eps;
                }

                Console.WriteLine(episode);
            }
        }



        public void Add(IAuthorAdapter author)
        {
            switch (author)
            {
                case AuthorAdapter adapter:
                    AuthorsDict.Add(adapter.Id, author);
                    AthId = adapter.Id + 1;
                    break;
                case AuthorAdapter2 adapter2:
                    AuthorsDict.Add(adapter2.Id, adapter2);
                    AthId = adapter2.Id + 1;
                    break;
                case Author auth:
                    AuthorsDict.Add(AthId++, auth);
                    break;
            }
        }

        public void Add(IMovieAdapter movie)
        {
            switch (movie)
            {
                case MovieAdapter adapter:
                    MoviesDict.Add(adapter.Id, adapter);
                    MovId = adapter.Id + 1;
                    break;
                case MovieAdapter2 adapter2:
                    MoviesDict.Add(adapter2.Id, adapter2);
                    MovId = adapter2.Id + 1;
                    break;
                case Movie mov:
                    MoviesDict.Add(MovId++, mov);
                    break;
            }

        }

        public void Add(ISeriesAdapter series)
        {
            switch (series)
            {
                case SeriesAdapter adapter:
                    SeriesDict.Add(adapter.Id, adapter);
                    SerId = adapter.Id + 1;
                    break;
                case SeriesAdapter2 adapter2:
                    SeriesDict.Add(adapter2.Id, adapter2);
                    SerId = adapter2.Id + 1;
                    break;
                case Series ser:
                    SeriesDict.Add(SerId++, ser);
                    break;
            }

        }

        public void Add(IEpisodeAdapter episode)
        {
            switch (episode)
            {
                case EpisodeAdapter adapter:
                    EpisodesDict.Add(adapter.Id, adapter);
                    EpId = adapter.Id + 1;
                    break;
                case EpisodeAdapter2 adapter2:
                    EpisodesDict.Add(adapter2.Id, adapter2);
                    EpId = adapter2.Id + 1;
                    break;
                case Episode ep:
                    EpisodesDict.Add(EpId++, ep);
                    break;
            }
        }

        public Author GetAuthor(int id)
        {
            return AuthorsDict.TryGetValue(id, out var value) ? value.ToAuthor() : null;
        }



        public Series GetSeries(int id)
        {
            return SeriesDict.TryGetValue(id, out var value) ? value.ToSeries() : null;
        }


        public Episode GetEpisode(int id)
        {
            return EpisodesDict.TryGetValue(id, out var value) ? value.ToEpisode() : null;
        }


        public Movie GetMovie(int id)
        {
            return MoviesDict.TryGetValue(id, out var value) ? value.ToMovie() : null;
        }


        public List<IAuthorAdapter> GetAuthors()
        {
            List<IAuthorAdapter> res = new List<IAuthorAdapter>();
            foreach (var author in AuthorsDict)
            {
                res.Add(author.Value);
            }

            return res;
        }

        public List<IMovieAdapter> GetMovies()
        {
            List<IMovieAdapter> res = new List<IMovieAdapter>();
            foreach (var mov in MoviesDict)
            {
                res.Add(mov.Value);
            }

            return res;
        }

        public List<IEpisodeAdapter> GetEpisodes()
        {
            List<IEpisodeAdapter> res = new List<IEpisodeAdapter>();
            foreach (var ep in EpisodesDict)
            {
                res.Add(ep.Value);
            }

            return res;
        }

        public List<ISeriesAdapter> GetSeries()
        {
            List<ISeriesAdapter> res = new List<ISeriesAdapter>();
            foreach (var ser in SeriesDict)
            {
                res.Add(ser.Value);
            }

            return res;
        }

        public void Remove(IAuthorAdapter author)
        {
            switch (author)
            {
                case AuthorAdapter2 adapter2:
                    int id = -1;
                    foreach (var aut in AuthorsDict)
                    {
                        if (aut.Value.ToAuthor().Equals(adapter2.ToAuthor()))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        AuthorsDict.Remove(id);
                    break;
                case AuthorAdapter adapter:
                    id = -1;
                    foreach (var aut in AuthorsDict)
                    {
                        if (aut.Value.ToAuthor().Equals(adapter.ToAuthor()))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        AuthorsDict.Remove(id);
                    break;
                case Author ath:
                    id = -1;
                    foreach (var aut in AuthorsDict)
                    {
                        if (aut.Value.Equals(ath))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        AuthorsDict.Remove(id);
                    break;
            }
        }

        public void Remove(IMovieAdapter movies)
        {
            switch (movies)
            {
                case MovieAdapter2 adapter2:
                    int id = -1;
                    foreach (var aut in MoviesDict)
                    {
                        if (aut.Value.ToMovie().Equals(adapter2.ToMovie()))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        MoviesDict.Remove(id);
                    break;
                case MovieAdapter adapter:
                    MoviesDict.Remove(adapter.Id);
                    break;
                case Movie mov:
                    id = -1;
                    foreach (var pair in MoviesDict)
                    {
                        if (pair.Value.Equals(mov))
                        {
                            id = pair.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        MoviesDict.Remove(id);
                    break;
            }
        }

        public void Remove(IEpisodeAdapter episode)
        {
            switch (episode)
            {
                case EpisodeAdapter2 adapter2:
                    int id = -1;
                    foreach (var aut in EpisodesDict)
                    {
                        if (aut.Value.ToEpisode().Equals(adapter2.ToEpisode()))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        EpisodesDict.Remove(id);
                    break;
                case EpisodeAdapter adapter:
                    EpisodesDict.Remove(adapter.Id);
                    break;
                case Episode eps:
                    id = -1;
                    foreach (var pair in EpisodesDict)
                    {
                        if (pair.Value.Equals(eps))
                        {
                            id = pair.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        EpisodesDict.Remove(id);
                    break;
            }
        }

        public void Remove(ISeriesAdapter series)
        {
            switch (series)
            {
                case SeriesAdapter2 adapter2:
                    int id = -1;
                    foreach (var aut in SeriesDict)
                    {
                        if (aut.Value.ToSeries().Equals(adapter2.ToSeries()))
                        {
                            id = aut.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        SeriesDict.Remove(id);
                    break;
                case SeriesAdapter adapter:
                    SeriesDict.Remove(adapter.Id);
                    break;
                case Series ser:
                    id = -1;
                    foreach (var pair in SeriesDict)
                    {
                        if (pair.Value.Equals(ser))
                        {
                            id = pair.Key;
                            break;
                        }
                    }

                    if (id != -1)
                        EpisodesDict.Remove(id);
                    break;
            }
        }

        public Dictionary<int, IAuthorAdapter> CloneAuthor()
        {
            return new Dictionary<int, IAuthorAdapter>(AuthorsDict);
        }

    }
}