using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Parser;

namespace Funcmd.Scripting
{
    internal abstract class Element
    {
        public Lexer<TokenType>.Token TokenPosition { get; set; }
    }

    internal abstract class Expression : Element
    {
        public virtual RuntimeValue Execute(RuntimeContext context)
        {
            throw new NotSupportedException();
        }
    }

    internal class Program
    {
        public List<Expression> Definitions { get; set; }
    }
}
