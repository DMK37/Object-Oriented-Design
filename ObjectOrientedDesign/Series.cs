using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectOrientedDesign
{

    public class Series : ISeriesAdapter
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author Showrunner { get; set; }
        public List<Episode> Episodes { get; set; }

        public Series(string title, string genre, ref Author showrunner, List<Episode> episodes)
        {
            Title = title;
            Genre = genre;
            Showrunner = showrunner;
            Episodes = episodes;
        }

        protected Series(string title, string genre, Author showrunner, List<Episode> episodes)
        {
            Title = title;
            Genre = genre;
            Showrunner = showrunner;
            Episodes = episodes;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(
                $"Series: {Title}, {Genre}, {Showrunner.Name} " +
                $"{Showrunner.Surname},\n Episodes: \n");

            foreach (var episode in Episodes)
            {
                stringBuilder.Append("\t");
                stringBuilder.Append(episode);
                stringBuilder.Append("\n");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        private bool Equals(Series other)
        {
            if (other == null)
                return false;

            return Title == other.Title
                   && Genre == other.Genre
                   && Episodes.Equals(other.Episodes)
                   && Showrunner.Equals(other.Showrunner);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Series);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + Genre.GetHashCode();
            hash = hash * 31 + (Title?.GetHashCode() ?? 0);
            return hash;
        }

        public Series ToSeries()
        {
            return this;
        }
    }

    public interface ISeriesAdapter
    {
        public string ToString();
        public Series ToSeries();
    }

    public class SeriesIndex
    {
        public int Id { get; }
        public string Title { get; }
        public string Genre { get; }
        public int ShowrunnerId { get; }
        public List<int> EpisodesId { get; }

        public SeriesIndex(string title, string genre, int showrunnerId, List<int> episodes)
        {
            Id = BitflixDb.SetSerId();
            Title = title;
            Genre = genre;
            ShowrunnerId = showrunnerId;
            EpisodesId = episodes;
        }
    }

    public class SeriesAdapter : ISeriesAdapter
    {
        private static BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;

        private readonly SeriesIndex _seriesIndex;

        public SeriesAdapter(SeriesIndex seriesIndex)
        {
            _seriesIndex = seriesIndex;
            Id = seriesIndex.Id;
        }

        public override string ToString()
        {
            Author author = _bitflixDb.GetAuthor(_seriesIndex.ShowrunnerId);
            StringBuilder stringBuilder = new StringBuilder(
                $"Series: {_seriesIndex.Title}, {_seriesIndex.Genre}, {author.Name} " +
                $"{author.Surname},\n Episodes: \n");

            var episodes = _bitflixDb.GetEpisodes(_seriesIndex.EpisodesId);
            foreach (var episode in episodes)
            {
                stringBuilder.Append("\t");
                stringBuilder.Append(episode);
                stringBuilder.Append("\n");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        public Series ToSeries()
        {
            Author author = _bitflixDb.GetAuthor(_seriesIndex.ShowrunnerId);
            return new Series(_seriesIndex.Title, _seriesIndex.Genre, ref author,
                _bitflixDb.GetEpisodes(_seriesIndex.EpisodesId));
        }
    }

    public class SeriesPartial
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Showrunner { get; set; }
        public string Episodes { get; set; }

        public SeriesPartial(string title, string showrunner, string episodes)
        {
            Id = BitflixDb.SetSerId();
            Title = title;
            Showrunner = showrunner;
            Episodes = episodes;
        }
    }

    public class SeriesAdapter2 : ISeriesAdapter
    {
        private static BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;

        public SeriesPartial _seriesPartial;


        public SeriesAdapter2(SeriesPartial seriesPartial)
        {
            _seriesPartial = seriesPartial;
            Id = seriesPartial.Id;
        }

        public static string[] ParseTitle(string title)
        {
            string[] res = new string[2];
            int i = 0;
            for (; i < title.Length; i++)
            {
                if (title[i] == '/')
                {
                    res[0] = title.Substring(0, i);
                    break;
                }
            }

            res[1] = title.Substring(++i, title.Length - i);

            return res;
        }

        private static List<int> ParseEpisodes(string episodes)
        {
            var ls = new List<int>();
            int prev = 0;
            for (int i = 0; i < episodes.Length; i++)
            {
                if (i == episodes.Length - 1)
                {
                    ls.Add(int.Parse(episodes.Substring(prev, i - prev + 1)));
                }

                if (episodes[i] == ',')
                {
                    ls.Add(int.Parse(episodes.Substring(prev, i - prev)));
                    prev = i + 1;
                }
            }

            return ls;
        }


        public override string ToString()
        {
            Author author = _bitflixDb.GetAuthor(int.Parse(_seriesPartial.Showrunner));
            var title = ParseTitle(_seriesPartial.Title);
            StringBuilder stringBuilder = new StringBuilder(
                $"Series: {title[0]}, {title[1]}, {author.Name} " +
                $"{author.Surname},\n Episodes: \n");

            var episodes = _bitflixDb.GetEpisodes(ParseEpisodes(_seriesPartial.Episodes));
            foreach (var episode in episodes)
            {
                stringBuilder.Append("\t");
                stringBuilder.Append(episode);
                stringBuilder.Append("\n");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }

        public Series ToSeries()
        {
            Author author = _bitflixDb.GetAuthor(int.Parse(_seriesPartial.Showrunner));
            var title = ParseTitle(_seriesPartial.Title);
            return new Series(title[0], title[1], ref author,
                _bitflixDb.GetEpisodes(ParseEpisodes(_seriesPartial.Episodes)));
        }
    }
}