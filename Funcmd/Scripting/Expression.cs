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
        public virtual void BuildContext(RuntimeContext context)
        {
        }

        public virtual RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return RuntimeValueWrapper.CreateValue(new object());
        }

        public virtual bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            return false;
        }
    }

    internal class Program
    {
        public List<Expression> Definitions { get; set; }

        public RuntimeContext BuildContext()
        {
            RuntimeContext context = new RuntimeContext();
            foreach (Expression e in Definitions)
            {
                e.BuildContext(context);
            }
            return context;
        }
    }
}
