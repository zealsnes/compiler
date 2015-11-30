using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public class InstructionStatement : Statement
    {
        public CpuInstructions Opcode { get; set; }
    }
}
