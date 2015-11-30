using System;
using Zeal.Compiler.Data;

namespace Zeal.Compiler.CodeGeneration
{
    public class OpcodeAttribute : Attribute
    {
        public byte Opcode { get; private set; }
        public CpuAddressingMode AddressingMode { get; private set; }

        public OpcodeAttribute(byte opcode)
        {
            AddressingMode = CpuAddressingMode.Implied;
            Opcode = opcode;
        }

        public OpcodeAttribute(CpuAddressingMode addressing, byte opcode)
        {
            AddressingMode = addressing;
            Opcode = opcode;
        }
    }
}
