using System;

namespace ObjectOrientedDesign
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BitflixDb bitflixDb = BitflixDb.Instance;
            //-------------------------------------Base---------------------------------------------------

            //Console.WriteLine("Base representation");
            Author author1 = new Author("Francis", "Coppola", 1939, 32);
            Author author2 = new Author("Steven", "Spielberg", 1956, 73);
            Author author3 = new Author("Charlie", "Chaplin", 1889, 6);
            Author author4 = new Author("Vince", "Gilligan", 1967, 17);
            Author author5 = new Author("Rian", "Johnson", 1973, 29);
            Author author6 = new Author("Greg", "Daniels", 1963, 5);
            Author author7 = new Author("Troy", "Miller", 1960, 0);
            Author author8 = new Author("Victor", "Nelli, Jr.", 1960, 0);
            Author author9 = new Author("Charles", "McDougall", 1960, 0);


            Episode episode1 = new Episode("Fly", 45, 2010, ref author5);
            Episode episode2 = new Episode("Ozymandias", 50, 2013, ref author5);
            Episode episode3 = new Episode("Pilot", 43, 2008, ref author4);
            Episode episode4 = new Episode("Dwight K. Schrute, (Acting) Manager", 22, 2011, ref author7);
            Episode episode5 = new Episode("The Carpet", 23, 2006, ref author8);
            Episode episode6 = new Episode("Dwight's Speech", 22, 2006, ref author9);


            //bitflixDb.Add(author1);
            //bitflixDb.Add(author2);

            //bitflixDb.Task2(bitflixDb.MoviesDictionary, bitflixDb.EpisodesDictionary);



            //---------------------------------------First---------------------------------------------------

            //Console.WriteLine("First representation");
            /*AuthorIndex athIdx1 = new AuthorIndex("Francis", "Coppola", 1939, 32);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Steven", "Spielberg", 1956, 73);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Charlie", "Chaplin", 1889, 6);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Vince", "Gilligan", 1967, 17);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Rian", "Johnson", 1973, 29);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Greg", "Daniels", 1963, 5);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Troy", "Miller", 1960, 0);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Victor", "Nelli, Jr.", 1960, 0);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
            athIdx1 = new AuthorIndex("Charles", "McDougall", 1960, 0);
            bitflixDb.Add(new AuthorAdapter(athIdx1));
    
    
            MovieIndex mov = new MovieIndex("Apocalypse now", "war film", 0, 147, 1979);
            bitflixDb.Add(new MovieAdapter(mov));
            mov = new MovieIndex("The Godfather", "criminal", 0, 175, 1972);
            bitflixDb.Add(new MovieAdapter(mov));
            mov = new MovieIndex("Raiders of the lost ark", "adventure", 1, 115, 1981);
            bitflixDb.Add(new MovieAdapter(mov));
            mov = new MovieIndex("The Great Dictator", "comedy", 2, 125, 1940);
            bitflixDb.Add(new MovieAdapter(mov));
    
            EpisodeIndex eps = new EpisodeIndex("Fly", 45, 2010, 4);
            bitflixDb.Add(new EpisodeAdapter(eps));
            eps = new EpisodeIndex("Ozymandias", 50, 2013, 4);
            bitflixDb.Add(new EpisodeAdapter(eps));
            eps = new EpisodeIndex("Pilot", 43, 2008, 3);
            bitflixDb.Add(new EpisodeAdapter(eps));
    
            
            
            eps = new EpisodeIndex("Dwight K. Schrute, (Acting) Manager", 22, 2011, 6);
            bitflixDb.Add(new EpisodeAdapter(eps));
            eps = new EpisodeIndex("The Carpet", 23, 2006, 7);
            bitflixDb.Add(new EpisodeAdapter(eps));
            eps = new EpisodeIndex("Dwight's Speech", 22, 2006, 8);
            bitflixDb.Add(new EpisodeAdapter(eps));
    
            SeriesIndex ser = new SeriesIndex("Breaking Bad", "drama", 3, new List<int> { 0, 1, 2 });
            bitflixDb.Add(new SeriesAdapter(ser));
            ser = new SeriesIndex("The Office US", "horror", 5,new List<int> {3,4,5});
            bitflixDb.Add(new SeriesAdapter(ser));
    
            
            //bitflixDb.Task2(bitflixDb.MoviesDictionary, bitflixDb.EpisodesDictionary);*/


            //---------------------------------------Second---------------------------------------------------

            //Console.WriteLine("Second representation");
            AuthorPartial ath = new AuthorPartial("Francisco+Coppola+1939^32^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Steven+Spielberg+1956^73^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Charlie+Chaplin+1889^6^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Vince+Gilligan+1967^17^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Rian+Johnson+1973^29^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Greg+Daniels+1963^5^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Troy+Miller+1960^0^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Victor+Nelli, Jr+1960^0^");
            bitflixDb.Add(new AuthorAdapter2(ath));
            ath = new AuthorPartial("Charles+McDougall+1960^0^");
            bitflixDb.Add(new AuthorAdapter2(ath));



            MoviePartial movie = new MoviePartial("Apocalypse now(1979)", "war film", "0", 147);
            bitflixDb.Add(new MovieAdapter2(movie));
            movie = new MoviePartial("The Godfather(1972)", "criminal", "0", 175);
            bitflixDb.Add(new MovieAdapter2(movie));
            movie = new MoviePartial("Raiders of the lost ark(1981)", "adventure", "1", 115);
            bitflixDb.Add(new MovieAdapter2(movie));
            movie = new MoviePartial("The Great Dictator(1940)", "comedy", "2", 125);
            bitflixDb.Add(new MovieAdapter2(movie));

            EpisodePartial eps1 = new EpisodePartial("Fly", 45, 2010, "4");
            bitflixDb.Add(new EpisodeAdapter2(eps1));
            eps1 = new EpisodePartial("Ozymandias", 50, 2013, "4");
            bitflixDb.Add(new EpisodeAdapter2(eps1));
            eps1 = new EpisodePartial("Pilot", 43, 2008, "3");
            bitflixDb.Add(new EpisodeAdapter2(eps1));



            eps1 = new EpisodePartial("Dwight K. Schrute, (Acting) Manager", 22, 2011, "6");
            bitflixDb.Add(new EpisodeAdapter2(eps1));
            eps1 = new EpisodePartial("The Carpet", 23, 2006, "7");
            bitflixDb.Add(new EpisodeAdapter2(eps1));
            eps1 = new EpisodePartial("Dwight's Speech", 22, 2006, "8");
            bitflixDb.Add(new EpisodeAdapter2(eps1));

            SeriesPartial serial = new SeriesPartial("Breaking Bad/drama", "3", "0,1,2");
            bitflixDb.Add(new SeriesAdapter2(serial));
            serial = new SeriesPartial("The Office US/horror", "5", "3,4,5");
            bitflixDb.Add(new SeriesAdapter2(serial));
            
            //-------------------------------------------------------------------------------------------------------------

            string reader = Console.In.ReadLine();
            CommandQueue commandQueue = new CommandQueue();

            while (reader != "exit")
            {
                if (reader == null)
                {
                    Console.WriteLine("null input");
                    reader = Console.In.ReadLine();
                    continue;
                }

                string[] parser = reader.Split(' ');

                switch (parser[0])
                {
                    case "list":
                        commandQueue.Add("list", reader);
                        break;
                    case "find":
                        commandQueue.Add("find", reader);
                        break;
                    case "add":
                        commandQueue.Add("add", reader);
                        break;
                    case "edit":
                        commandQueue.Add("edit", reader);
                        break;
                    case "delete":
                        commandQueue.Add("delete", reader);
                        break;
                    case "history":
                        commandQueue.History();
                        break;
                    case "export":
                        if (parser.Length > 2)
                        {
                            switch (parser[2])
                            {
                                case "XML":
                                    commandQueue.Serialize(parser[1]);
                                    break;
                                case "plaintext":
                                    commandQueue.PlainText(parser[1]);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong number of parameters");
                        }

                        break;
                    case "load":
                        if (parser.Length > 1)
                        {
                            commandQueue.Load(parser[1]);
                        }

                        break;
                    default:
                        Console.WriteLine("Wrong input");
                        break;
                    case "undo":
                        commandQueue.Undo();
                        break;
                    case "redo":
                        commandQueue.Redo();
                        break;
                }

                reader = Console.In.ReadLine();
            }
        }
    }
}