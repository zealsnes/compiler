using System;
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

        public Scope Scope
        {
            private get;
            set;
        }

        public RomHeader Header
        {
            private get;
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
                    case CpuAddressingMode.Direct:
                    case CpuAddressingMode.Absolute:
                    case CpuAddressingMode.Relative:
                        {
                            _stream.WriteByte(opcode.Opcode);

                            if (instruction.Arguments[0] is NumberInstructionArgument)
                            {
                                var numberArgument = instruction.Arguments[0] as NumberInstructionArgument;

                                if (numberArgument.Size == ArgumentSize.Word
                                    || numberArgument.Size == ArgumentSize.LongWord)
                                {
                                    writeWord(numberArgument.Number);
                                }
                                else
                                {
                                    _stream.WriteByte((byte)numberArgument.Number);
                                }
                            }
                            else if (instruction.Arguments[0] is LabelInstructionArgument)
                            {
                                var labelArgument = instruction.Arguments[0] as LabelInstructionArgument;
                                long labelPhysicalAddress = Scope.AddressFor(labelArgument.Label);

                                if (instruction.AddressingMode == CpuAddressingMode.Relative)
                                {
                                    byte relativeAddress = Convert.ToByte((labelPhysicalAddress - (_stream.Position + 1)) & 0xFF);
                                    _stream.WriteByte(relativeAddress);
                                }
                                else if (instruction.AddressingMode == CpuAddressingMode.Absolute)
                                {
                                    int ramAddress = CpuAddressConverter.PhysicalToRAM((int)labelPhysicalAddress, Header.MapMode, Header.RomSpeed);
                                    writeWord(ramAddress & 0xFFFF);
                                }
                            }
                            break;
                        }
                }
            }
        }

        private void writeWord(int number)
        {
            _stream.WriteByte((byte)(number & 0xFF));
            _stream.WriteByte((byte)(number >> 8));
        }
    }
}
