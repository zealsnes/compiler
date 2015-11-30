using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public enum ScopeType
    {
        Scope,
        Procedure,
        Function,
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
