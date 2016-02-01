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
        private Dictionary<string, long> _labels = new Dictionary<string, long>();

        public string Name { get; set; }
        public ScopeType Type { get; set; }

        public List<Statement> Statements
        {
            get
            {
                return _statements;
            }
        }

        public Dictionary<string, long> Labels
        {
            get
            {
                return _labels;
            }
        }
    }
}
