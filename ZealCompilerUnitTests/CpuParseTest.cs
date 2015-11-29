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
            ZealCpuDriver parser = new ZealCpuDriver(input.ToMemoryStream());
            parser.Parse();

            Assert.Equal("HELLO WORLD SNES", parser.Header.CatridgeName);
            Assert.Equal(RomSpeed.SlowROM, parser.Header.RomSpeed);
            Assert.Equal(MapMode.HiROM, parser.Header.MapMode);
            Assert.Equal(32u, parser.Header.SramSize);
            Assert.Equal(Country.NorthAmerica, parser.Header.Country);
            Assert.Equal(0x1Au, parser.Header.Developer);
            Assert.Equal(10u, parser.Header.Version);
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
            ZealCpuDriver parser = new ZealCpuDriver(input.ToMemoryStream());
            parser.Parse();

            Assert.Equal("CopVector", parser.Vectors.COP);
            Assert.Equal("BrkVector", parser.Vectors.BRK);
            Assert.Equal("IrqVector", parser.Vectors.IRQ);
            Assert.Equal("NmiVector", parser.Vectors.NMI);
            Assert.Equal("Main", parser.Vectors.Reset);
        }
    }
}
