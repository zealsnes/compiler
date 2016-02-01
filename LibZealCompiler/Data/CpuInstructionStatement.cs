using System.Collections.Generic;

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

        public CpuInstructionStatement()
            : base()
        {
        }

        public override long ComputeSize()
        {
            long totalSize = 1;

            foreach (var argument in Arguments)
            {
                long argSize = argument.ComputeSize();
                if (argSize == -1)
                {
                    if (AddressingMode == CpuAddressingMode.Relative)
                    {
                        totalSize += 1;
                    }
                    else if (AddressingMode == CpuAddressingMode.Absolute)
                    {
                        totalSize += 2;
                    }
                    else if (AddressingMode == CpuAddressingMode.AbsoluteLong)
                    {
                        totalSize += 3;
                    }
                }
                else
                {
                    totalSize += argSize;
                }
            }

            return totalSize;
        }
    }
}
