using System.Collections.Generic;
using System.Linq;
using opennlp.tools.namefind;
using opennlp.tools.tokenize;

namespace OTAutocompleteDSL
{
    // See https://cwiki.apache.org/confluence/display/OPENNLP/Introduction+to+using+openNLP+in+.NET+Projects
    // Data files taken from http://opennlp.sourceforge.net/models-1.5/
    // NOT thread safe
    public class NLPLexer : ILexer
    {
        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            // no-op
        }

        public enum EntityType
        {
            Date = 0,
            Location,
            Money,
            Organization,
            Person,
            Time
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var results = new List<Token>();

            //now tokenize the input.
            //"Don Krapohl enjoys warm sunny weather" would tokenize as
            //"Don", "Krapohl", "enjoys", "warm", "sunny", "weather"
            var tokens = Tokenizer.tokenize(source);

            //do the find
            var foundLocations = LocationFinder.find(tokens);
            var foundTimes = TimeFinder.find(tokens);
            var foundNames = NameFinder.find(tokens);

            //important:  clear adaptive data in the feature generators or the detection rate will decrease over time.
            LocationFinder.clearAdaptiveData();
            TimeFinder.clearAdaptiveData();
            NameFinder.clearAdaptiveData();

            results.AddRange(opennlp.tools.util.Span.spansToStrings(foundLocations, tokens).Select(x => new Token { Value = x, Type = "location" }));
            results.AddRange(opennlp.tools.util.Span.spansToStrings(foundTimes, tokens).Select(x => new Token { Value = x, Type = "time" }));
            results.AddRange(opennlp.tools.util.Span.spansToStrings(foundNames, tokens).Select(x => new Token { Value = x, Type = "name" }));

            return results;
        }

        public ILexer InitNow()
        {
            _tokenizer = prepareTokenizer();
            _nameFinder = prepareNameFinder();
            _locationFinder = prepareLocationFinder();
            _timeFinder = prepareTimeFinder();
            return this;
        }

        public TokenizerME Tokenizer
        {
            get { return _tokenizer ?? (_tokenizer = prepareTokenizer()); }
        }
        private TokenizerME _tokenizer;

        public NameFinderME NameFinder
        {
            get { return _nameFinder ?? (_nameFinder = prepareNameFinder()); }
        }
        private NameFinderME _nameFinder;

        public NameFinderME LocationFinder
        {
            get { return _locationFinder ?? (_locationFinder = prepareLocationFinder()); }
        }
        private NameFinderME _locationFinder;

        public NameFinderME TimeFinder
        {
            get { return _timeFinder ?? (_timeFinder = prepareTimeFinder()); }
        }
        private NameFinderME _timeFinder;

        private static TokenizerME prepareTokenizer()
        {
            var tokenInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-token.bin");     //load the token model into a stream
            var tokenModel = new opennlp.tools.tokenize.TokenizerModel(tokenInputStream); //load the token model
            return new opennlp.tools.tokenize.TokenizerME(tokenModel);  //create the tokenizer
        }

        private static NameFinderME prepareLocationFinder()
        {
            var modelInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-ner-location.bin"); //load the name model into a stream
            var model = new opennlp.tools.namefind.TokenNameFinderModel(modelInputStream); //load the model
            return new opennlp.tools.namefind.NameFinderME(model);                   //create the namefinder
        }

        private static opennlp.tools.namefind.NameFinderME prepareNameFinder()
        {
            var modelInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-ner-location.bin"); //load the name model into a stream
            var model = new opennlp.tools.namefind.TokenNameFinderModel(modelInputStream); //load the model
            return new opennlp.tools.namefind.NameFinderME(model);                   //create the namefinder
        }

        private static NameFinderME prepareTimeFinder()
        {
            var modelInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-ner-time.bin"); //load the name model into a stream
            var model = new TokenNameFinderModel(modelInputStream); //load the model
            return new NameFinderME(model);                   //create the namefinder
        }
    }
}
