using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Mamemaki.EventFlow.HealthReporters.StdOutput.Tokenization
{
    internal class FormatTokenizer
    {
        public IEnumerable<FormatToken> Tokenize(string text)
        {
            var regex = new Regex(@"\$(\{[a-zA-Z0-9_-]+\})");
            var tokens = Tokenize<FormatToken>(regex, 
                text,
                (s, i, len) =>
                {
                    return new FormatToken(FormatTokenType.String, s.Substring(i, len));
                },
                m =>
                {
                    if (Enum.TryParse(m.Value.Substring(2, m.Value.Length - 3), out FormatTokenType type))
                        return new FormatToken(type, m.Value);
                    else
                        return new FormatToken(FormatTokenType.String, m.Value);
                });
            return tokens;
        }

        /// <summary>
        /// Regex to turn matches into a stream of tokens
        /// <see cref="https://gist.github.com/atifaziz/8016e8cf8c89d39ad719057c2299eb94"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        /// <param name="textSelector"></param>
        /// <param name="matchSelector"></param>
        /// <returns></returns>
        private IEnumerable<T> Tokenize<T>(Regex regex, string input,
            Func<string, int, int, T> textSelector, Func<Match, T> matchSelector)
        {
            var i = 0;
            foreach (Match m in regex.Matches(input))
            {
                if (m.Index > i)
                    yield return textSelector(input, i, m.Index - i);
                yield return matchSelector(m);
                i = m.Index + m.Length;
            }
            if (i < input.Length)
                yield return textSelector(input, i, input.Length - i);
        }
    }
}
