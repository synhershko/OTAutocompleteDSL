using System.Collections.Generic;

namespace OTAutocompleteDSL
{
    public interface ILexer
    {
        void AddDefinition(TokenDefinition tokenDefinition);
        IEnumerable<Token> Tokenize(string source);
    }
}
