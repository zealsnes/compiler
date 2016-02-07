using System;
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

        internal Scope GetScope(string text)
        {
            foreach(var child in _children)
            {
                if (child.Name == text)
                {
                    return child;
                }
            }

            return null;
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

        public bool IsLabelValid(string label)
        {
            return Labels.ContainsKey(label);
        }
    }
}
