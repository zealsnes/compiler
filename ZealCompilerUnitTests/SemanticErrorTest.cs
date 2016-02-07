using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zeal.Compiler.Parser;
using Zeal.Compiler.UnitTests.Extensions;

namespace Zeal.Compiler.UnitTests
{
    public class SemanticErrorTest
    {
        [Fact]
        public void ShouldFailOnInvalidHeaderEntryName()
        {
            const string input = @"
header
{
    CartName = ""HELLO WORLD""
}

vectors
{

}

procedure Main
{
    sei
    clc
    xce
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("Slowrom")]
        [InlineData("Fastrom")]
        [InlineData("ExHirom")]
        [InlineData("SlowROM")]
        public void MapModeShouldBeValid(string mapMode)
        {
            const string inputTemplate = @"
header
{{
    MapMode = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, mapMode);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("slowrom")]
        [InlineData("fastrom")]
        [InlineData("LoROM")]
        [InlineData("HiROM")]
        public void RomSpeedShouldBeValid(string romSpeed)
        {
            const string inputTemplate = @"
header
{{
    RomSpeed = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, romSpeed);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("Canadia")]
        [InlineData("USA")]
        [InlineData("Deuchland")]
        [InlineData("Kebec")]
        public void CountryShouldBeValid(string country)
        {
            const string inputTemplate = @"
header
{{
    Country = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, country);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("2")]
        [InlineData("HelloWorld")]
        [InlineData("$2A")]
        [InlineData("%010101011")]
        public void CartridgeNameExceptsAStringLiteral(string invalidCartridgeName)
        {
            const string inputTemplate = @"
header
{{
    CartridgeName = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, invalidCartridgeName);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("HelloWorld")]
        [InlineData("\"Hi\"")]
        public void SramSizeExceptsANumberiteral(string invalidSramSize)
        {
            const string inputTemplate = @"
header
{{
    SramSize = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, invalidSramSize);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("HelloWorld")]
        [InlineData("\"Hi\"")]
        public void DeveloperExceptsANumberiteral(string invalidDeveloper)
        {
            const string inputTemplate = @"
header
{{
    Developer = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, invalidDeveloper);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("HelloWorld")]
        [InlineData("\"Hi\"")]
        public void VersionExceptsANumberiteral(string invalidVersion)
        {
            const string inputTemplate = @"
header
{{
    Version = {0}
}}

vectors
{{

}}

procedure Main
{{
    sei
    clc
    xce
}}
";
            string input = String.Format(inputTemplate, invalidVersion);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Fact]
        public void VectorsIsRequired()
        {
            const string input = @"
header
{
    CartridgeName = ""HELLO WORLD SNES""
    RomSpeed = SlowROM
    MapMode = LoROM
    SramSize = 0
    Country = Japan
    Developer = 0
    Version = 0
}

procedure Main
{
    sei
    clc
    xce
}
"
;
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();
            Assert.Throws<CompilerErrorException>(() => driver.SecondPass());
            Assert.Equal(1, driver.Errors.Count);
        }

        [Fact]
        public void ShouldFailOnInvalidVectorsEntryName()
        {
            const string input = @"
vectors
{
    NormalNMI = Main
}

procedure Main
{
    sei
    clc
    xce
}
"
;
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Fact]
        public void VectorsShouldBeFullyPopulated()
        {
            const string input = @"
vectors
{
    Reset = Main
}

procedure Main
{
    sei
    clc
    xce
}
"
;
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Fact]
        public void ShouldFailOnInvalidVectorsLabel()
        {
            const string input = @"
vectors
{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = NotValidReset
}

procedure Main
{
    sei
    clc
    xce
}

interrupt EmptyVector
{
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();
            Assert.Throws<CompilerErrorException>(() => driver.SecondPass());
        }

        [Fact]
        public void ShouldFailOnInvalidInstructionLabel()
        {
            const string input = @"
vectors
{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = Main
}

procedure Main
{
    sei
    clc
    xce
mainLoop:
    jmp infinite
}

interrupt EmptyVector
{
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();
            Assert.Throws<CompilerErrorException>(() => driver.SecondPass());
        }

        [Fact]
        public void ImpliedInstructionsShouldNotHaveArguments()
        {
            const string input = @"
vectors
{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = Main
}

procedure Main
{
    sei #$1A
}

interrupt EmptyVector
{
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Fact]
        public void InstructionsShouldHaveArguments()
        {
            const string input = @"
vectors
{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = Main
}

procedure Main
{
    lda
}

interrupt EmptyVector
{
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Theory]
        [InlineData("stz $7E2345")]
        public void CheckInvalidAddressingMode(string instruction)
        {
            const string inputTemplate = @"
vectors
{{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = Main
}}

procedure Main
{{
    {0}
}}

interrupt EmptyVector
{{
}}
";
            string input = String.Format(inputTemplate, instruction);

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
        }

        [Fact]
        public void ShouldFailOnBranchTooLong()
        {
            const string input = @"
vectors
{
    BRK = EmptyVector
    NMI = EmptyVector
    IRQ = EmptyVector
    Reset = Main
}

procedure Main
{
    bra forwardLabel
backwardLabel:
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
    sta $7E0000
forwardLabel:
    bra backwardLabel
}

interrupt EmptyVector
{
}
";

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            Assert.Throws<CompilerErrorException>(() => driver.Parse());
            Assert.Equal(2, driver.Errors.Count);
        }
    }
}
