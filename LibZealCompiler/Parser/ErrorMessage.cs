using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Parser
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
