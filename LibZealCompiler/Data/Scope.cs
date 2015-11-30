using System.Collections.Generic;

namespace Zeal.Compiler.Data
{
    public enum ScopeType
    {
        Scope,
        Procedure,
        Interrupt
    }

    public class Scope
    {
        private List<Statement> _statements = new List<Statement>();

        public string Name { get; set; }
        public ScopeType Type { get; set; }

        public List<Statement> Statements
        {
            get
            {
                return _statements;
            }
        }
    }
}
