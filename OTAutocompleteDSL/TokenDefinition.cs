using System.Text.RegularExpressions;

namespace OTAutocompleteDSL
{
    public class TokenDefinition
    {
        public bool IsIgnored { get; set; }
        public Regex Regex { get; set; }
        public string Type { get; set; }
    }
}
