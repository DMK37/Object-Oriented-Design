namespace ObjectOrientedDesign
{

    public class Episode: IEpisodeAdapter
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }
        
        public Episode(string title, int duration, int releaseYear, ref Author director)
        {

            Title = title;
            Duration = duration;
            ReleaseYear = releaseYear;
            Director = director;
        }
        
        protected Episode(string title, int duration, int releaseYear, Author director)
        {

            Title = title;
            Duration = duration;
            ReleaseYear = releaseYear;
            Director = director;
        }

        private bool Equals(Episode other)
        {
            if (other == null)
                return false;

            return Title == other.Title 
                   && Duration == other.Duration 
                   && ReleaseYear == other.ReleaseYear && Director.Equals(other.Director);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Episode);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + ReleaseYear.GetHashCode();
            hash = hash * 31 + (Title?.GetHashCode() ?? 0);
            return hash;
        }
        
        public override string ToString()
        {
            return
                $"Episode: {Title}, {Duration}, {ReleaseYear}, {Director.Name} " +
                $"{Director.Surname}";
        }

        public Episode ToEpisode()
        {
            return this;
        }
    }
    
    public class EpisodeIndex
    {

        public int Id { get; }
        public string Title { get; }
        public int Duration { get; }
        public int ReleaseYear { get; }
        public int DirectorId { get; }

        public EpisodeIndex(string title, int duration, int releaseYear, int directorId)
        {
            Id = BitflixDb.SetEpId();
            Title = title;
            Duration = duration;
            ReleaseYear = releaseYear;
            DirectorId = directorId;
        }
    }

    public interface IEpisodeAdapter
    {
        public string ToString();
        public Episode ToEpisode();
    }

    public class EpisodeAdapter : IEpisodeAdapter
    {
        private static BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;
        private readonly EpisodeIndex _episodeIndex;

        public EpisodeAdapter(EpisodeIndex episodeIndex)
        {
            _episodeIndex = episodeIndex;
            Id = episodeIndex.Id;
        }

        public override string ToString()
        {
            //var director = new AuthorAdapter(_bitflixDb.GetAuthor(_episodeIndex.DirectorId)).ToAuthor();
            Author author = _bitflixDb.GetAuthor(_episodeIndex.DirectorId);
            return
                $"Episode: {_episodeIndex.Title}, {_episodeIndex.Duration}, {_episodeIndex.ReleaseYear}," +
                $" {author.Name} {author.Surname}";
        }

        public Episode ToEpisode()
        {
            Author author = _bitflixDb.GetAuthor(_episodeIndex.DirectorId);
            return new Episode(_episodeIndex.Title,_episodeIndex.Duration,_episodeIndex.ReleaseYear,ref author);
        }
    }

    public class EpisodePartial
    {
        public int Id { get; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }


        public EpisodePartial(string title, int duration, int releaseYear, string director)
        {
            Id = BitflixDb.SetEpId();
            Title = title;
            Duration = duration;
            ReleaseYear = releaseYear;
            Director = director;
        }
        
        
    }

    public class EpisodeAdapter2 : IEpisodeAdapter
    {
        private static BitflixDb _bitflixDb = BitflixDb.Instance;
        public readonly int Id;

        public EpisodePartial _episodePartial;

        public EpisodeAdapter2(EpisodePartial episodePartial)
        {
            Id = episodePartial.Id;
            _episodePartial = episodePartial;
        }

        public override string ToString()
        {
            Author author = _bitflixDb.GetAuthor(int.Parse(_episodePartial.Director));
            return
                $"Episode: {_episodePartial.Title}, {_episodePartial.Duration}, {_episodePartial.ReleaseYear}," +
                $" {author.Name} {author.Surname}";
        }

        public Episode ToEpisode()
        {
            Author author = _bitflixDb.GetAuthor(int.Parse(_episodePartial.Director));
            return new Episode(_episodePartial.Title, _episodePartial.Duration, _episodePartial.ReleaseYear,
                ref author);
        }
    }
    


}