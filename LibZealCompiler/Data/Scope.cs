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
        private List<Scope> _children = new List<Scope>();

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

        public Scope Parent
        {
            get;
            internal set;
        }

        public List<Scope> Children
        {
            get
            {
                return _children;
            }
        }

        public void Add(Scope scope)
        {
            scope.Parent = this;

            _children.Add(scope);
        }

        public long AddressFor(string label)
        {
            long result = -1;
            Labels.TryGetValue(label, out result);
            return result;
        }
    }
}
