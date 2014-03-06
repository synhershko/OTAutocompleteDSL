using System.Collections.Generic;

namespace OTAutocompleteDSL
{
    public class DeletingLexer : ILexer
    {
        private readonly List<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();

        public void AddDefinition(TokenDefinition tokenDefinition)
        {
            tokenDefinitions.Add(tokenDefinition);
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var currentIndex = 0;

            while (!string.IsNullOrWhiteSpace(source))
            {
                TokenDefinition matchedDefinition = null;
                var matchLength = 0;
                string value = "";

                foreach (var rule in tokenDefinitions)
                {
                    var match = rule.Regex.Match(source);

                    if (match.Success)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        value = source.Substring(match.Index, matchLength);
                        source = source.Remove(match.Index, matchLength).Trim();
                        break;
                    }
                }

                if (matchedDefinition == null)
                {
                    yield return new UnknownToken {Position = currentIndex, Type = "(unknown)", Value = source};
                    //throw new Exception(string.Format("Unrecognized symbol '{0}' at index {1} (line {2}, column {3}).", source[currentIndex], currentIndex, currentLine, currentColumn));
                    source = string.Empty;
                    continue;
                }

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
