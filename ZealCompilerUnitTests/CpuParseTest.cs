using System;
using Xunit;
using Zeal.Compiler.Data;
using Zeal.Compiler.Parser;
using Zeal.Compiler.UnitTests.Extensions;

namespace Zeal.Compiler.UnitTests
{
    public class CpuParseTest
    {
        const string ProcedureTemplate = @"procedure Test
{{
    {0}
}}";

        [Fact]
        public void ShouldParseHeaderInfo()
        {
            string input = @"
header
{
    CatridgeName = ""HELLO WORLD SNES""
    RomSpeed = SlowROW
    MapMode = HiROM
    SramSize = 32
    Country = NorthAmerica
    Developer = $1A
    Version = %1010
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            Assert.Equal("HELLO WORLD SNES", driver.Header.CatridgeName);
            Assert.Equal(RomSpeed.SlowROM, driver.Header.RomSpeed);
            Assert.Equal(MapMode.HiROM, driver.Header.MapMode);
            Assert.Equal(32u, driver.Header.SramSize);
            Assert.Equal(Country.NorthAmerica, driver.Header.Country);
            Assert.Equal(0x1Au, driver.Header.Developer);
            Assert.Equal(10u, driver.Header.Version);
        }

        [Fact]
        public void ShouldParseVectorsInfo()
        {
            string input = @"
vectors
{
    COP = CopVector
    BRK = BrkVector
    IRQ = IrqVector
    NMI = NmiVector
    Reset = Main
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            Assert.Equal("CopVector", driver.Vectors.COP);
            Assert.Equal("BrkVector", driver.Vectors.BRK);
            Assert.Equal("IrqVector", driver.Vectors.IRQ);
            Assert.Equal("NmiVector", driver.Vectors.NMI);
            Assert.Equal("Main", driver.Vectors.Reset);
        }

        [Fact]
        public void ShouldParseProcedure()
        {
            string input = @"procedure Test
{
}";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            Assert.Equal("Test", driver.Scopes[0].Name);
            Assert.Equal(ScopeType.Procedure, driver.Scopes[0].Type);
        }

        [Fact]
        public void ShouldParseInterrupt()
        {
            string input = @"interrupt EmptyVector
{
}";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            Assert.Equal("EmptyVector", driver.Scopes[0].Name);
            Assert.Equal(ScopeType.Interrupt, driver.Scopes[0].Type);
        }

        [Fact]
        public void ShouldAddRTIWhenParsingInterrupt()
        {
            string input = @"interrupt NMI
{
    php
    pha

    pla
    plp
}";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            var cpuInstruction = driver.Scopes[0].Statements[driver.Scopes[0].Statements.Count - 1] as CpuInstructionStatement;

            Assert.Equal("NMI", driver.Scopes[0].Name);
            Assert.Equal(ScopeType.Interrupt, driver.Scopes[0].Type);
            Assert.Equal(CpuInstructions.rti, cpuInstruction.Opcode);
            Assert.Equal(CpuAddressingMode.Implied, cpuInstruction.AddressingMode);
        }

        [Fact]
        public void ShouldParseLabel()
        {
            string input = @"procedure Test
{
    php
    pha

mainLoop:
    lda $2007
    jmp mainLoop

exit:
    rts
}";

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            var thirdInstruction = driver.Scopes[0].Statements[2];
            var fiveInstruction = driver.Scopes[0].Statements[4];

            Assert.Equal("mainLoop", thirdInstruction.AssociatedLabel);
            Assert.Equal("exit", fiveInstruction.AssociatedLabel);
        }

        [Fact]
        public void ShouldParseLabelArgument()
        {
            string input = @"procedure Test
{
    jmp mainLoop
}";

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            var instruction = driver.Scopes[0].Statements[0] as CpuInstructionStatement;
            var argument = instruction.Arguments[0] as LabelInstructionArgument;

            Assert.Equal("mainLoop", argument.Label);
        }

        [Theory]
        [InlineData("jmp mainLoop")]
        [InlineData("jsr SomeFunction")]
        public void ShouldMarkAbsoluteLabelArgument(string instructionText)
        {
            string input = String.Format(ProcedureTemplate, instructionText);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            var instruction = driver.Scopes[0].Statements[0] as CpuInstructionStatement;

            Assert.Equal(CpuAddressingMode.Absolute, instruction.AddressingMode);
        }

        [Theory]
        [InlineData("bcc Label")]
        [InlineData("bcs Label")]
        [InlineData("beq Label")]
        [InlineData("bmi Label")]
        [InlineData("bne Label")]
        [InlineData("bpl Label")]
        [InlineData("bra Label")]
        [InlineData("bvc Label")]
        [InlineData("bvs Label")]
        public void ShouldMarkRelativeLabelArgument(string instructionText)
        {
            string input = String.Format(ProcedureTemplate, instructionText);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            var instruction = driver.Scopes[0].Statements[0] as CpuInstructionStatement;

            Assert.Equal(CpuAddressingMode.Relative, instruction.AddressingMode);
        }

        [Theory]
        [InlineData("clc", CpuInstructions.clc)]
        [InlineData("cld", CpuInstructions.cld)]
        [InlineData("cli", CpuInstructions.cli)]
        [InlineData("clv", CpuInstructions.clv)]
        [InlineData("dex", CpuInstructions.dex)]
        [InlineData("dey", CpuInstructions.dey)]
        [InlineData("inc", CpuInstructions.inc)]
        [InlineData("inx", CpuInstructions.inx)]
        [InlineData("iny", CpuInstructions.iny)]
        [InlineData("nop", CpuInstructions.nop)]
        [InlineData("sec", CpuInstructions.sec)]
        [InlineData("sed", CpuInstructions.sed)]
        [InlineData("sei", CpuInstructions.sei)]
        [InlineData("stp", CpuInstructions.stp)]
        [InlineData("tax", CpuInstructions.tax)]
        [InlineData("tay", CpuInstructions.tay)]
        [InlineData("tcd", CpuInstructions.tcd)]
        [InlineData("tcs", CpuInstructions.tcs)]
        [InlineData("tdc", CpuInstructions.tdc)]
        [InlineData("tsc", CpuInstructions.tsc)]
        [InlineData("tsx", CpuInstructions.tsx)]
        [InlineData("txa", CpuInstructions.txa)]
        [InlineData("txs", CpuInstructions.txs)]
        [InlineData("txy", CpuInstructions.txy)]
        [InlineData("tya", CpuInstructions.tya)]
        [InlineData("tyx", CpuInstructions.tyx)]
        [InlineData("wai", CpuInstructions.wai)]
        [InlineData("xba", CpuInstructions.xba)]
        [InlineData("xce", CpuInstructions.xce)]
        public void ShouldParseImpliedInstructions(string opcodeText, CpuInstructions opcodeEnum)
        {
            string input = String.Format(ProcedureTemplate, opcodeText);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            CpuInstructionStatement instructionStatement = driver.Scopes[0].Statements[0] as CpuInstructionStatement;
            Assert.Equal(opcodeEnum, instructionStatement.Opcode);
            Assert.Equal(CpuAddressingMode.Implied, instructionStatement.AddressingMode);
        }

        [Theory]
        [InlineData("rep #$38", CpuInstructions.rep, 0x38)]
        [InlineData("ldx #$1FFF", CpuInstructions.ldx, 0x1FFF)]
        public void ShouldParseImmediateInstructions(string instruction, CpuInstructions opcodeEnum, int value)
        {
            string input = String.Format(ProcedureTemplate, instruction);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            CpuInstructionStatement instructionStatement = driver.Scopes[0].Statements[0] as CpuInstructionStatement;
            Assert.Equal(opcodeEnum, instructionStatement.Opcode);
            Assert.Equal(CpuAddressingMode.Immediate, instructionStatement.AddressingMode);

            var numberArgument = instructionStatement.Arguments[0] as NumberInstructionArgument;
            Assert.Equal(value, numberArgument.Number);
        }

        [Theory]
        [InlineData("sta $00", CpuInstructions.sta, 0)]
        [InlineData("lda $02", CpuInstructions.lda, 2)]
        public void ShouldParseDirectInstructions(string instruction, CpuInstructions opcodeEnum, int value)
        {
            string input = String.Format(ProcedureTemplate, instruction);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            CpuInstructionStatement instructionStatement = driver.Scopes[0].Statements[0] as CpuInstructionStatement;
            Assert.Equal(opcodeEnum, instructionStatement.Opcode);
            Assert.Equal(CpuAddressingMode.Direct, instructionStatement.AddressingMode);

            var numberArgument = instructionStatement.Arguments[0] as NumberInstructionArgument;
            Assert.Equal(value, numberArgument.Number);
        }

        [Theory]
        [InlineData("sta $2100", CpuInstructions.sta, 0x2100)]
        public void ShouldParseAbsoluteInstructions(string instruction, CpuInstructions opcodeEnum, int value)
        {
            string input = String.Format(ProcedureTemplate, instruction);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            CpuInstructionStatement instructionStatement = driver.Scopes[0].Statements[0] as CpuInstructionStatement;
            Assert.Equal(opcodeEnum, instructionStatement.Opcode);
            Assert.Equal(CpuAddressingMode.Absolute, instructionStatement.AddressingMode);

            var numberArgument = instructionStatement.Arguments[0] as NumberInstructionArgument;
            Assert.Equal(value, numberArgument.Number);
        }

        [Theory]
        [InlineData("lda #$1", 0x1, ArgumentSize.Byte)]
        [InlineData("lda #$01", 0x1, ArgumentSize.Byte)]
        [InlineData("lda #1", 0x1, ArgumentSize.Byte)]
        [InlineData("lda #255", 255, ArgumentSize.Byte)]
        [InlineData("lda #%0001", 1, ArgumentSize.Byte)]
        [InlineData("lda #$0001", 0x1, ArgumentSize.Word)]
        [InlineData("lda #$100", 0x100, ArgumentSize.Word)]
        [InlineData("lda #$00FF", 0xFF, ArgumentSize.Word)]
        [InlineData("lda #256", 256, ArgumentSize.Word)]
        [InlineData("lda #%000000000001", 1, ArgumentSize.Word)]
        [InlineData("sta $000001", 1, ArgumentSize.LongWord)]
        [InlineData("sta 65537", 65537, ArgumentSize.LongWord)]
        [InlineData("sta %100000000000000000000000", 0x800000, ArgumentSize.LongWord)]
        [InlineData("sta %000000000000000000000001", 1, ArgumentSize.LongWord)]
        public void ShouldParseArgumentSizeCorrectly(string instruction, int value, ArgumentSize size)
        {
            string input = String.Format(ProcedureTemplate, instruction);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            CpuInstructionStatement instructionStatement = driver.Scopes[0].Statements[0] as CpuInstructionStatement;

            var argument = instructionStatement.Arguments[0] as NumberInstructionArgument;

            Assert.Equal(value, argument.Number);
            Assert.Equal(size, argument.Size);
        }
    }
}
