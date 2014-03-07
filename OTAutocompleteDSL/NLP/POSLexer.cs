using System;
using System.Collections.Generic;
using opennlp.tools.postag;
using opennlp.tools.tokenize;

namespace OTAutocompleteDSL.NLP
{
    // See https://cwiki.apache.org/confluence/display/OPENNLP/Introduction+to+using+openNLP+in+.NET+Projects
    // Data files taken from http://opennlp.sourceforge.net/models-1.5/
    // NOT thread safe
    public class POSLexer : ILexer
    {
        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            // no-op
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var results = new List<Token>();

            var tokens = Tokenizer.tokenize(source);
            var tags = PosTagger.tag(tokens);
            var probs = PosTagger.probs();

            // POS tags explained here: http://blog.dpdearing.com/2011/12/opennlp-part-of-speech-pos-tags-penn-english-treebank/

            return results;
        }

        public ILexer InitNow()
        {
            Console.WriteLine("Loading...");
            _tokenizer = prepareTokenizer();
            _posTagger = preparePOSTagger();
            return this;
        }

        public TokenizerME Tokenizer
        {
            get { return _tokenizer ?? (_tokenizer = prepareTokenizer()); }
        }
        private TokenizerME _tokenizer;

        public POSTaggerME PosTagger
        {
            get { return _posTagger ?? (_posTagger = preparePOSTagger()); }
        }
        private POSTaggerME _posTagger;

        private static TokenizerME prepareTokenizer()
        {
            using (var tokenInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-token.bin"))
            {
                var tokenModel = new opennlp.tools.tokenize.TokenizerModel(tokenInputStream); //load the token model
                return new opennlp.tools.tokenize.TokenizerME(tokenModel);  //create the tokenizer
            }
        }

        private static POSTaggerME preparePOSTagger()
        {
            using (var modelInputStream = new java.io.FileInputStream(@"c:\projects\OTAutocompleteDSL\dep\en-pos-maxent.bin"))
            {
                return new POSTaggerME(new POSModel(modelInputStream)); //load the model
            }
        }
    }
}
