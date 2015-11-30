using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
