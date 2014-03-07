using System.Collections.Generic;

namespace OTAutocompleteDSL
{
    public class Lexer : ILexer
    {
        private readonly List<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();

        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var currentIndex = 0;

            while (currentIndex < source.Length)
            {
                TokenDefinition matchedDefinition = null;
                int matchLength = 0;

                foreach (var rule in tokenDefinitions)
                {
                    var match = rule.Regex.Match(source, currentIndex);

                    if (match.Success && (match.Index - currentIndex) == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    yield return new UnknownToken {Position = currentIndex, Type = "(unknown)"};
                    //throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
                    currentIndex = source.Length;
                    continue;
                }

                var value = source.Substring(currentIndex, matchLength);

                if (!matchedDefinition.IsIgnored)
                    yield return
                        new Token
                        {
                            Type = matchedDefinition.Type,
                            Value = value,
                            Position = currentIndex,
                        };

                currentIndex += matchLength;
            }

            yield return
                new Token
                {
                    Type = "(end)",
                    Position = currentIndex,
                };
        }
    }
}
