using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public class CpuInstructionStatement : Statement
    {
        private List<InstructionArgument> _arguments = new List<InstructionArgument>();

        public CpuInstructions Opcode { get; set; }
        public CpuAddressingMode AddressingMode { get; set; }

        public List<InstructionArgument> Arguments
        {
            get
            {
                return _arguments;
            }
        }
    }
}
