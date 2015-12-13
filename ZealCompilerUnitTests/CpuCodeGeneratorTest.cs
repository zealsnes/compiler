using System.Collections.Generic;
using System.IO;
using Xunit;
using Zeal.Compiler.CodeGeneration;
using Zeal.Compiler.Data;

namespace Zeal.Compiler.UnitTests
{
    public class CpuCodeGeneratorTest
    {
        [Theory]
        [InlineData(CpuInstructions.asl, 0x0A)]
        [InlineData(CpuInstructions.brk, 0x00)]
        [InlineData(CpuInstructions.clc, 0x18)]
        [InlineData(CpuInstructions.cld, 0xD8)]
        [InlineData(CpuInstructions.cli, 0x58)]
        [InlineData(CpuInstructions.clv, 0xB8)]
        [InlineData(CpuInstructions.dec, 0x3A)]
        [InlineData(CpuInstructions.dex, 0xCA)]
        [InlineData(CpuInstructions.dey, 0x88)]
        [InlineData(CpuInstructions.inc, 0x1A)]
        [InlineData(CpuInstructions.inx, 0xE8)]
        [InlineData(CpuInstructions.iny, 0xC8)]
        [InlineData(CpuInstructions.lsr, 0x4A)]
        [InlineData(CpuInstructions.nop, 0xEA)]
        [InlineData(CpuInstructions.pha, 0x48)]
        [InlineData(CpuInstructions.phb, 0x8B)]
        [InlineData(CpuInstructions.phd, 0x0B)]
        [InlineData(CpuInstructions.phk, 0x4B)]
        [InlineData(CpuInstructions.php, 0x08)]
        [InlineData(CpuInstructions.phx, 0xDA)]
        [InlineData(CpuInstructions.phy, 0x5A)]
        [InlineData(CpuInstructions.pla, 0x68)]
        [InlineData(CpuInstructions.plb, 0xAB)]
        [InlineData(CpuInstructions.pld, 0x2B)]
        [InlineData(CpuInstructions.plp, 0x28)]
        [InlineData(CpuInstructions.plx, 0xFA)]
        [InlineData(CpuInstructions.ply, 0x7A)]
        [InlineData(CpuInstructions.rol, 0x2A)]
        [InlineData(CpuInstructions.ror, 0x6A)]
        [InlineData(CpuInstructions.rti, 0x40)]
        [InlineData(CpuInstructions.rtl, 0x6B)]
        [InlineData(CpuInstructions.rts, 0x60)]
        [InlineData(CpuInstructions.sec, 0x38)]
        [InlineData(CpuInstructions.sed, 0xF8)]
        [InlineData(CpuInstructions.sei, 0x78)]
        [InlineData(CpuInstructions.stp, 0xDB)]
        [InlineData(CpuInstructions.tax, 0xAA)]
        [InlineData(CpuInstructions.tay, 0xA8)]
        [InlineData(CpuInstructions.tcd, 0x5B)]
        [InlineData(CpuInstructions.tcs, 0x1B)]
        [InlineData(CpuInstructions.tdc, 0x7B)]
        [InlineData(CpuInstructions.tsc, 0x3B)]
        [InlineData(CpuInstructions.tsx, 0xBA)]
        [InlineData(CpuInstructions.txa, 0x8A)]
        [InlineData(CpuInstructions.txs, 0x9A)]
        [InlineData(CpuInstructions.txy, 0x9B)]
        [InlineData(CpuInstructions.tya, 0x98)]
        [InlineData(CpuInstructions.tyx, 0xBB)]
        [InlineData(CpuInstructions.wai, 0xCB)]
        [InlineData(CpuInstructions.xba, 0xEB)]
        [InlineData(CpuInstructions.xce, 0xFB)]
        public void ShouldGenerateImpliedInstruction(CpuInstructions opcodeEnum, byte finalOpcode)
        {
            CpuInstructionStatement instruction = new CpuInstructionStatement();
            instruction.AddressingMode = CpuAddressingMode.Implied;
            instruction.Opcode = opcodeEnum;

            List<CpuInstructionStatement> instructions = new List<CpuInstructionStatement>();
            instructions.Add(instruction);

            MemoryStream memoryStream = new MemoryStream(8);
            CpuCodeGenerator generator = new CpuCodeGenerator(memoryStream);
            generator.Instructions = instructions;
            generator.Generate();

            Assert.Equal(finalOpcode, memoryStream.GetBuffer()[0]);
        }

        [Theory]
        // Enum, opcode, value
        [InlineData(CpuInstructions.adc, 0x69, 34)]
        [InlineData(CpuInstructions.adc, 0x69, 1024)]
        [InlineData(CpuInstructions.and, 0x29, 34)]
        [InlineData(CpuInstructions.and, 0x29, 1024)]
        [InlineData(CpuInstructions.bit, 0x89, 34)]
        [InlineData(CpuInstructions.bit, 0x89, 1024)]
        [InlineData(CpuInstructions.cmp, 0xC9, 34)]
        [InlineData(CpuInstructions.cmp, 0xC9, 1024)]
        [InlineData(CpuInstructions.cpx, 0xE0, 34)]
        [InlineData(CpuInstructions.cpx, 0xE0, 1024)]
        [InlineData(CpuInstructions.cpy, 0xC0, 34)]
        [InlineData(CpuInstructions.cpy, 0xC0, 1024)]
        [InlineData(CpuInstructions.eor, 0x49, 34)]
        [InlineData(CpuInstructions.eor, 0x49, 1024)]
        [InlineData(CpuInstructions.lda, 0xA9, 34)]
        [InlineData(CpuInstructions.lda, 0xA9, 1024)]
        [InlineData(CpuInstructions.ldx, 0xA2, 34)]
        [InlineData(CpuInstructions.ldx, 0xA2, 1024)]
        [InlineData(CpuInstructions.ldy, 0xA0, 34)]
        [InlineData(CpuInstructions.ldy, 0xA0, 1024)]
        [InlineData(CpuInstructions.ora, 0x09, 34)]
        [InlineData(CpuInstructions.ora, 0x09, 1024)]
        [InlineData(CpuInstructions.rep, 0xC2, 34)]
        [InlineData(CpuInstructions.sbc, 0xE9, 34)]
        [InlineData(CpuInstructions.sbc, 0xE9, 1024)]
        [InlineData(CpuInstructions.sep, 0xE2, 34)]
        public void ShouldGenerateImmediateInstruction(CpuInstructions opcodeEnum, byte finalOpcode, int value)
        {
            CpuInstructionStatement instruction = new CpuInstructionStatement();
            instruction.AddressingMode = CpuAddressingMode.Immediate;
            instruction.Opcode = opcodeEnum;
            if (value > byte.MaxValue)
            {
                instruction.Arguments.Add(new NumberInstructionArgument(value, ArgumentSize.Word));
            }
            else
            {
                instruction.Arguments.Add(new NumberInstructionArgument(value, ArgumentSize.Byte));
            }

            List<CpuInstructionStatement> instructions = new List<CpuInstructionStatement>();
            instructions.Add(instruction);

            MemoryStream memoryStream = new MemoryStream(8);
            CpuCodeGenerator generator = new CpuCodeGenerator(memoryStream);
            generator.Instructions = instructions;
            generator.Generate();

            Assert.Equal(finalOpcode, memoryStream.GetBuffer()[0]);
            Assert.Equal((byte)(value & 0xFF), memoryStream.GetBuffer()[1]);
            Assert.Equal((byte)(value >> 8), memoryStream.GetBuffer()[2]);
        }

        [Theory]
        // Enum, opcode, value
        [InlineData(CpuInstructions.adc, 0x65, 0x34)]
        [InlineData(CpuInstructions.and, 0x25, 0x34)]
        [InlineData(CpuInstructions.asl, 0x06, 0x34)]
        [InlineData(CpuInstructions.bit, 0x24, 0x34)]
        [InlineData(CpuInstructions.cmp, 0xC5, 0x34)]
        [InlineData(CpuInstructions.cpx, 0xE4, 0x34)]
        [InlineData(CpuInstructions.cpy, 0xC4, 0x34)]
        [InlineData(CpuInstructions.dec, 0xC6, 0x34)]
        [InlineData(CpuInstructions.eor, 0x45, 0x34)]
        [InlineData(CpuInstructions.inc, 0xE6, 0x34)]
        [InlineData(CpuInstructions.lda, 0xA5, 0x34)]
        [InlineData(CpuInstructions.ldx, 0xA6, 0x34)]
        [InlineData(CpuInstructions.ldy, 0xA4, 0x34)]
        [InlineData(CpuInstructions.lsr, 0x46, 0x34)]
        [InlineData(CpuInstructions.ora, 0x05, 0x34)]
        [InlineData(CpuInstructions.rol, 0x26, 0x34)]
        [InlineData(CpuInstructions.ror, 0x66, 0x34)]
        [InlineData(CpuInstructions.sbc, 0xE5, 0x34)]
        [InlineData(CpuInstructions.sta, 0x85, 0x34)]
        [InlineData(CpuInstructions.stx, 0x86, 0x34)]
        [InlineData(CpuInstructions.sty, 0x84, 0x34)]
        [InlineData(CpuInstructions.stz, 0x64, 0x34)]
        [InlineData(CpuInstructions.trb, 0x14, 0x34)]
        [InlineData(CpuInstructions.tsb, 0x04, 0x34)]
        public void ShouldGenerateDirectInstruction(CpuInstructions opcodeEnum, byte finalOpcode, int value)
        {
            CpuInstructionStatement instruction = new CpuInstructionStatement();
            instruction.AddressingMode = CpuAddressingMode.Direct;
            instruction.Opcode = opcodeEnum;
            instruction.Arguments.Add(new NumberInstructionArgument(value, ArgumentSize.Byte));

            List<CpuInstructionStatement> instructions = new List<CpuInstructionStatement>();
            instructions.Add(instruction);

            MemoryStream memoryStream = new MemoryStream(8);
            CpuCodeGenerator generator = new CpuCodeGenerator(memoryStream);
            generator.Instructions = instructions;
            generator.Generate();

            Assert.Equal(finalOpcode, memoryStream.GetBuffer()[0]);
            Assert.Equal((byte)(value & 0xFF), memoryStream.GetBuffer()[1]);
        }
    }
}
