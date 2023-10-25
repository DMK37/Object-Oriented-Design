using System;
using System.Runtime.CompilerServices;

namespace ObjectOrientedDesign
{
    public class Movie: IMovieAdapter
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author Director { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }

        public Movie(string title, string genre,ref Author author, int duration, int releaseYear)
        {
            
            Title = title;
            Genre = genre;
            Director = author;
            ReleaseYear = releaseYear;
            Duration = duration;
        }
        
        protected Movie(string title, string genre, Author author, int duration, int releaseYear)
        {
            Title = title;
            Genre = genre;
            Director = author;
            ReleaseYear = releaseYear;
            Duration = duration;
        }

        public override string ToString()
        {
            return
                $"Movie: {Title}, {Genre}, {Director.Name} " +
                $"{Director.Surname}, {ReleaseYear}, {Duration}";
        }
        
        private bool Equals(Movie other)
        {
            if (other == null)
                return false;

            return Title == other.Title 
                   && Genre == other.Genre
                   && Duration == other.Duration 
                   && ReleaseYear == other.ReleaseYear && Director.Equals(other.Director);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Movie);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + ReleaseYear.GetHashCode();
            hash = hash * 31 + (Title?.GetHashCode() ?? 0);
            return hash;
        }

        public Movie ToMovie()
        {
            return this;
        }
    }

    public class MovieIndex
    {
        public int Id { get; }
        public string Title { get; }
        public string Genre { get; }
        public int DirectorId { get; }
        public int ReleaseYear { get; }
        public int Duration { get; }
        
        public MovieIndex(string title, string genre, int directorId, int duration, int releaseYear)
        {
            Id = BitflixDb.SetMovId();
            Title = title;
            Genre = genre;
            DirectorId = directorId;
            ReleaseYear = releaseYear;
            Duration = duration;
        }
    }

    public interface IMovieAdapter
    {
        public string ToString();
        public Movie ToMovie();
    }

    public class MovieAdapter : IMovieAdapter
    {
        private static BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;
        private readonly MovieIndex _movieIndex;

        public MovieAdapter(MovieIndex movieIndex)
        {
            _movieIndex = movieIndex;
            Id = movieIndex.Id;
        }

        public override string ToString()
        {
            Author author = _bitflixDb.GetAuthor(_movieIndex.DirectorId);

            return
                $"Movie: {_movieIndex.Title}, {_movieIndex.Genre}, {author.Name} " +
                $"{author.Surname}, {_movieIndex.ReleaseYear}, {_movieIndex.Duration}";
        }

        public Movie ToMovie()
        {
            Author author = _bitflixDb.GetAuthor(_movieIndex.DirectorId);
            return new Movie(_movieIndex.Title, _movieIndex.Genre, ref author, _movieIndex.Duration,
                _movieIndex.ReleaseYear);
        }
        
        
    }


    public class MoviePartial
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public int Duration { get; set; }

        public MoviePartial(string title, string genre, string director, int duration)
        {
            Id = BitflixDb.SetMovId();
            Title = title;
            Genre = genre;
            Director = director;
            Duration = duration;
        }
    }

    public class MovieAdapter2 : IMovieAdapter
    {
        private static readonly BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;
        public MoviePartial _moviePartial;

        public MovieAdapter2(MoviePartial moviePartial)
        {
            _moviePartial = moviePartial;
            Id = moviePartial.Id;
        }
        public static string[]ParseTitle(string title)
        {
            string[] res = new string[2];
            int i = 0;
            for (; i < title.Length; i++)
            {
                if (title[i] == '(')
                {
                    res[0] = title.Substring(0, i);
                    break;
                }
            }

            res[1] = title.Substring(++i, title.Length - i - 1);
            return res;
        }

        public override string ToString()
        {
            var title = ParseTitle(_moviePartial.Title);
            Author author = _bitflixDb.GetAuthor(int.Parse(_moviePartial.Director));
            return $"Movie: {title[0]}, {_moviePartial.Genre}, {author.Name} " +
                   $"{author.Surname}, {title[1]}, {_moviePartial.Duration}";
        }

        public Movie ToMovie()
        {
            var title = ParseTitle(_moviePartial.Title);
            Author author = _bitflixDb.GetAuthor(int.Parse(_moviePartial.Director));
            return new Movie(title[0], _moviePartial.Genre, ref author, _moviePartial.Duration,
                int.Parse(title[1]));
        }
    }

}