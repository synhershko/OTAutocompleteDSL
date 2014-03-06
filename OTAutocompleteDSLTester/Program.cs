using System;
using System.Text.RegularExpressions;
using OTAutocompleteDSL;

namespace OTAutocompleteDSLTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new DeletingLexer();

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"in (?<location>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "location_in",
                TakeGroup = "location",
            });

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"(near|by|around) (?<location>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "location_near",
                TakeGroup = "location",
            });

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"best", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "best",
            });

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"(the|restaurant(s?))", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "stop",
            });

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"(tonight|tomorrow)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "time",
            });

            lexer.AddDefinition(new TokenDefinition
            {
                Regex = new Regex(@"(?<anything>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                Type = "anything",
            });

            var queries = new[]
            {
                "tonight in london",
                "in london tonight",
                "the best italian in London",
                "restaurants around Birmington",
                "cheap italian restaurant",
                "special offers in london",
                "romantic place in manchester",
                "restaurants in London tonight",
                "mango tree",
                "Reading Spice and Shampan",
                "1 lombard street",
                "1 lombard st",
            };
            foreach (var query in queries)
            {
                RunQuery(query, lexer);
                Console.WriteLine("=================");
            }
        }

        static void RunQuery(string query, ILexer lexer)
        {
            Console.WriteLine(query);
            foreach (var token in lexer.Tokenize(query))
            {
                Console.WriteLine(token);
            }
        }
    }
}
