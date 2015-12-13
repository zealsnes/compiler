using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zeal.Compiler.Data;
using Zeal.Compiler.Helper;

namespace Zeal.Compiler.CodeGeneration
{
    public class CpuCodeGenerator
    {
        private Stream _stream;
        
        public List<CpuInstructionStatement> Instructions
        {
            get;
            set;
        }

        public CpuCodeGenerator(Stream stream)
        {
            _stream = stream;
        }

        public void Generate()
        {
            foreach(var instruction in Instructions)
            {
                var opcodes = instruction.Opcode.GetAttributes<OpcodeAttribute>();

                var opcode = opcodes.Where(x => x.AddressingMode == instruction.AddressingMode).First();

                switch(instruction.AddressingMode)
                {
                    // Instruction size of 1
                    case CpuAddressingMode.Implied:
                        _stream.WriteByte(opcode.Opcode);
                        break;
                    case CpuAddressingMode.Immediate:
                        {
                            var numberArgument = instruction.Arguments[0] as NumberInstructionArgument;
                            _stream.WriteByte(opcode.Opcode);
                            if (numberArgument.Size == ArgumentSize.Word
                                || numberArgument.Size == ArgumentSize.LongWord)
                            {
                                _stream.WriteByte((byte)(numberArgument.Number & 0xFF));
                                _stream.WriteByte((byte)(numberArgument.Number >> 8));
                            }
                            else
                            {
                                _stream.WriteByte((byte)numberArgument.Number);
                            }
                            break;
                        }
                }
            }
        }
    }
}
