using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public class InstructionArgument
    {
    }

    public class NumberInstructionArgument : InstructionArgument
    {
        public int Number { get; set; }

        public NumberInstructionArgument(int value)
        {
            Number = value;
        }
    }
}
