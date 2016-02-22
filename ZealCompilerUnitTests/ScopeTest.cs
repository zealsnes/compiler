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
    public class ScopeTest
    {
        [Fact]
        public void ShoulResolveLabelsToAddress()
        {
            string input = @"procedure Test
{
    php
    pha

mainLoop:
    lda $2007
    jmp mainLoop
    bra exit

exit:
    rts
}

interrupt EmptyVector
{
}
";

            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());
            driver.Parse();

            Assert.Equal(0, driver.GlobalScope.AddressFor("Test"));
            Assert.Equal(11, driver.GlobalScope.AddressFor("EmptyVector"));

            var testScope = driver.GlobalScope.Children[0];
            Assert.Equal(2, testScope.AddressFor("mainLoop"));
            Assert.Equal(10, testScope.AddressFor("exit"));
        }

        [Fact]
        public void ShouldResolveLabelsOutsideScope()
        {
            const string input =
@"
vectors
{
    BRK = EmptyVector
    IRQ = EmptyVector
    NMI = EmptyVector
    Reset = Main
}

procedure Main
{
    jsr Test
}

procedure Test
{
    php
    rep #$30
    pha

    lda #$03

    pla
    plp
    rts
}

interrupt EmptyVector
{
}
";
            ZealCpuDriver driver = new ZealCpuDriver(input.ToMemoryStream());

            driver.Parse();
            driver.SecondPass();

            var mainProcedure = driver.GlobalScope.Children[0];
            Assert.Equal(true, mainProcedure.IsLabelValid("Test"));
            Assert.Equal(3, mainProcedure.AddressFor("Test"));
        }
    }
}
