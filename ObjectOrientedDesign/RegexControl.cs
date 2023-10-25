using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace ObjectOrientedDesign
{
    public static class RegexControl
    {
        private static string classes = "Author|Episode|Series|Movie";

        public static readonly Dictionary<string, HashSet<string>> fieldsOfClasses =
            new Dictionary<string, HashSet<string>>
            {
                { "Author", new HashSet<string> { "Name", "Surname", "BirthYear", "Awards" } },
                { "Episode", new HashSet<string> { "Title", "Duration", "ReleaseYear", "AuthorID" } },
                { "Series", new HashSet<string> { "Title", "Genre", "AuthorID" } },
                { "Movie", new HashSet<string> { "Title", "Genre", "Duration", "ReleaseYear", "AuthorID" } }
            };

        public static (bool, List<string>) ListCheck(string input)
        {
            Regex pat = new Regex(@"^(list)(\s+)");
            List<string> res = new List<string>();
            Match mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            int i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);
            int k = mat.Value.IndexOf(' ');
            string tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@$"^({classes})\s*$");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value;
            if (k != -1)
                tmp = mat.Value.Remove(k);
            res.Add(tmp);
            return (true, res);
        }

        public static (bool, List<string>) FindCheck(string input)
        {
            List<string> res = new List<string>();
            Regex pat = new Regex(@"^(find)(\s+)");
            Match mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            int i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            int k = mat.Value.IndexOf(' ');
            string tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@$"^({classes})(\s+)");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value.Remove(k);
            res.Add(tmp);

            string fields = "";
            foreach (var field in fieldsOfClasses[res[res.Count - 1]])
            {
                fields += field + "|";
            }

            if (fields.Length > 0)
            {
                fields = fields.Remove(fields.Length - 1);
            }

            pat = new Regex(@$"(^(?:({fields})[\<\>\=])((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$))");
            mat = pat.Match(input);
            while (mat.Success)
            {
                i = input.IndexOf(mat.Value, StringComparison.Ordinal);
                input = input.Remove(i, mat.Length);
                int t = mat.Value.LastIndexOf('\"');
                k = mat.Value.IndexOf(' ');
                tmp = mat.Value;
                if (t != -1 && t < tmp.Length - 1)
                    tmp = mat.Value.Remove(t + 1);
                else
                {
                    if (k != -1 && t == -1)
                        tmp = mat.Value.Remove(k);
                }

                res.Add(tmp);
                mat = pat.Match(input);
            }

            if (input == "")
            {
                return (true, res);
            }

            Console.WriteLine("Wrong input");
            return (false, null);

        }

        public static (bool, string, string, ObjectBuildFactory) AddCheck(string input)
        {
            List<string> res = new List<string>();
            Regex pat = new Regex(@"^(add)(\s+)");
            Match mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null, null, null);
            }

            int i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);
            int k = mat.Value.IndexOf(' ');
            string tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@$"^({classes})(\s+)");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null, null, null);
            }

            i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@"^(base|secondary)(\s*$)");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null, null, null);
            }

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value;
            if (k != -1)
                tmp = mat.Value.Remove(k);
            res.Add(tmp);
            //-----------------------------



            ObjectBuildFactory builder = null;

            switch (res[2])
            {
                case "base":
                    switch (res[1])
                    {
                        case "Author":
                            builder = new AuthorBuilderBase();
                            break;
                        case "Movie":
                            builder = new MovieBuilderBase();
                            break;
                        case "Episode":
                            builder = new EpisodeBuilderBase();
                            break;
                        case "Series":
                            builder = new SeriesBuilderBase();
                            break;
                    }

                    break;
                case "secondary":
                    switch (res[1])
                    {
                        case "Author":
                            builder = new AuthorBuilderSecondary();
                            break;
                        case "Movie":
                            builder = new MovieBuilderSecondary();
                            break;
                        case "Episode":
                            builder = new EpisodeBuilderSecondary();
                            break;
                        case "Series":
                            builder = new SeriesBuilderSecondary();
                            break;
                    }

                    break;
            }

            return (true, res[1], res[2], builder);
        }

        public static (bool, List<string>) EditCheck(string input)
        {
            List<string> res = new List<string>();
            Regex pat = new Regex(@"^(edit)(\s+)");
            Match mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            int i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            int k = mat.Value.IndexOf(' ');
            string tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@$"^({classes})(\s+)");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value.Remove(k);
            res.Add(tmp);

            string fields = "";
            foreach (var field in fieldsOfClasses[res[res.Count - 1]])
            {
                fields += field + "|";
            }

            if (fields.Length > 0)
            {
                fields = fields.Remove(fields.Length - 1);
            }

            pat = new Regex(@$"(^(?:({fields})[\<\>\=])((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$))");
            mat = pat.Match(input);
            while (mat.Success)
            {
                i = input.IndexOf(mat.Value, StringComparison.Ordinal);
                input = input.Remove(i, mat.Length);
                int t = mat.Value.LastIndexOf('\"');
                k = mat.Value.IndexOf(' ');
                tmp = mat.Value;
                if (t != -1 && t < tmp.Length - 1)
                    tmp = mat.Value.Remove(t + 1);
                else
                {
                    if (k != -1 && t == -1)
                        tmp = mat.Value.Remove(k);
                }

                res.Add(tmp);
                mat = pat.Match(input);
            }

            if (input == "")
            {
                return (true, res);
            }

            Console.WriteLine("Wrong input");
            return (false, null);

        }
        
        public static (bool, List<string>) DeleteCheck(string input)
        {
            List<string> res = new List<string>();
            Regex pat = new Regex(@"^(delete)(\s+)");
            Match mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            int i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            int k = mat.Value.IndexOf(' ');
            string tmp = mat.Value.Remove(k);
            res.Add(tmp);

            pat = new Regex(@$"^({classes})(\s+)");
            mat = pat.Match(input);
            if (!mat.Success)
            {
                Console.WriteLine("Wrong input");
                return (false, null);
            }

            i = input.IndexOf(mat.Value, StringComparison.Ordinal);
            input = input.Remove(i, mat.Length);

            k = mat.Value.IndexOf(' ');
            tmp = mat.Value.Remove(k);
            res.Add(tmp);

            string fields = "";
            foreach (var field in fieldsOfClasses[res[res.Count - 1]])
            {
                fields += field + "|";
            }

            if (fields.Length > 0)
            {
                fields = fields.Remove(fields.Length - 1);
            }

            pat = new Regex(@$"(^(?:({fields})[\<\>\=])((\""([^\""]+)\"")|(?:\d+(?:\.\d+)?))(\s+|$))");
            mat = pat.Match(input);
            while (mat.Success)
            {
                i = input.IndexOf(mat.Value, StringComparison.Ordinal);
                input = input.Remove(i, mat.Length);
                int t = mat.Value.LastIndexOf('\"');
                k = mat.Value.IndexOf(' ');
                tmp = mat.Value;
                if (t != -1 && t < tmp.Length - 1)
                    tmp = mat.Value.Remove(t + 1);
                else
                {
                    if (k != -1 && t == -1)
                        tmp = mat.Value.Remove(k);
                }

                res.Add(tmp);
                mat = pat.Match(input);
            }

            if (input == "")
            {
                return (true, res);
            }

            Console.WriteLine("Wrong input");
            return (false, null);

        }
    }
}