using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ObjectOrientedDesign
{
    [Serializable]
    public class Author: IAuthorAdapter
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Awards { get; set; }
        public Author(string name, string surname, int birthYear, int awards)
        {
            Name = name;
            Surname = surname;
            BirthYear = birthYear;
            Awards = awards;
        }

        public override string ToString()
        {
            return $"Author: {Name} {Surname}, {BirthYear}, {Awards}";
        }
        
        private bool Equals(Author other)
        {
            if (other == null)
                return false;

            return Name == other.Name 
                   && Surname == other.Surname 
                   && BirthYear == other.BirthYear && Awards == other.Awards;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Author);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + BirthYear.GetHashCode();
            hash = hash * 31 + (Name?.GetHashCode() ?? 0);
            return hash;
        }
        
        public Author ToAuthor()
        {
            return this;
        }
    }
    
    public class AuthorIndex
    {
        public int Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public int BirthYear { get; }
        public int Awards { get; }

        public AuthorIndex(string name, string surname, int birthYear, int awards)
        {
            Id = BitflixDb.SetAthId();
            Name = name;
            Surname = surname;
            BirthYear = birthYear;
            Awards = awards;
        }

        
    }

    
    public interface IAuthorAdapter
    {
        public string ToString();
        public Author ToAuthor();
    }


    [Serializable]
    public class AuthorAdapter : IAuthorAdapter
    {
        private readonly AuthorIndex _authorIndex;
        public AuthorAdapter(AuthorIndex authorIndex)
        {
            _authorIndex = authorIndex;
            Id = authorIndex.Id;
        }

        public override string ToString()
        {
            return
                $"Author: {_authorIndex.Name} {_authorIndex.Surname}, {_authorIndex.BirthYear}, {_authorIndex.Awards}";
        }

        public readonly int Id;
        
        public Author ToAuthor()
        {
            return new Author(_authorIndex.Name, _authorIndex.Surname, _authorIndex.BirthYear, _authorIndex.Awards);
        }
    }
    
    
    public class AuthorPartial
    {
        public string Info { get; set; }
        public int Id { get; }

        public AuthorPartial(string info)
        {
            Info = info;
            Id = BitflixDb.SetAthId();
        }
    }

    [Serializable]
    public class AuthorAdapter2 : IAuthorAdapter
    {
        public AuthorPartial _authorPartial;
        public readonly int Id;
        public static string[] Parse(string info)
        {
            string[] res = new string[4];
            int i = 0;
            int n = info.Length;
            for (; i < n; i++)
            {
                if (info[i] == '+')
                {
                    res[0] = info.Substring(0, i);
                    break;
                }
            }

            int tmp = ++i;
            for (; i < n; i++)
            {
                if (info[i] == '+')
                {
                    res[1] = info.Substring(tmp, i - tmp);
                    break;
                }
            }

            tmp = ++i;
            for (; i < n; i++)
            {
                if (info[i] == '^')
                {
                    res[2] = info.Substring(tmp, i - tmp);
                    break;
                }
            }

            tmp = ++i;
            res[3] = info.Substring(tmp, n - tmp - 1);
            return res;
        }
        
        public AuthorAdapter2(AuthorPartial authorPartial)
        {
            _authorPartial = authorPartial;
            Id = authorPartial.Id;
        }
        
        public override string ToString()
        {
            var info = Parse(_authorPartial.Info);
            return
                $"Author: {info[0]} {info[1]}, {info[2]}, {info[3]}";
        }

        public Author ToAuthor()
        {
            var info = Parse(_authorPartial.Info);
            return new Author(info[0],
                info[1], int.Parse(info[2]),
                int.Parse(info[3]));
        }
    }

}