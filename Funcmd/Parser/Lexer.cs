using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Funcmd.Parser
{
    public class Lexer<T>
    {
        public class TokenValue
        {
            public int Position { get; internal set; }
            public string Value { get; internal set; }
        }

        public class Token : TokenValue
        {
            public T Tag { get; internal set; }
        }

        private List<Regex> tokenRules = new List<Regex>();
        private List<Func<TokenValue, T>> tokenHandlers = new List<Func<TokenValue, T>>();

        public Lexer()
        {
            tokenRules.Add(null);
            tokenHandlers.Add(null);
        }

        public void AddToken(string rule, Func<TokenValue, T> handler)
        {
            tokenRules.Add(new Regex(rule));
            tokenHandlers.Add(handler);
        }

        public void AddToken(string rule, T tag)
        {
            tokenRules.Add(new Regex(rule));
            tokenHandlers.Add(t => tag);
        }

        public IEnumerable<Token> Parse(string input)
        {
            int position = 0;
            while (true)
            {
                var match = tokenRules
                    .Select(r => r == null ? null : new { Source = r, Match = r.Match(input, position) })
                    .Where(m => m == null || m.Match.Success && m.Match.Index == position)
                    .Aggregate((a, b) =>
                        a == null ? b :
                        a.Match.Length >= b.Match.Length ? a :
                        b);
                if (match != null)
                {
                    Token token = new Token()
                    {
                        Position = position,
                        Value = match.Match.Value
                    };
                    token.Tag = tokenHandlers[tokenRules.IndexOf(match.Source)](token);
                    yield return token;
                    position += match.Match.Length;
                }
                else if (position == input.Length)
                {
                    break;
                }
                else
                {
                    throw new LexerException<T>("遇到无法解析的字符串。", new Token()
                    {
                        Position = position
                    });
                }
            }
        }
    }

    public class LexerException<T> : Exception
    {
        public Lexer<T>.Token Token { get; private set; }

        public LexerException(string message, Lexer<T>.Token token)
            : base(message)
        {
            this.Token = token;
        }
    }
}
