using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Zeal.Compiler.Parser;
using Antlr4.Runtime.Tree;
using Zeal.Compiler.Data;
using System.Globalization;

namespace Zeal.Compiler.Pass
{
    class CpuParseInfoPass : ZealCpuBaseListener
    {
        private ZealCpuDriver _driver;

        public CpuParseInfoPass(ZealCpuDriver driver)
        {
            _driver = driver;
        }

        public override void ExitHeaderDeclaration([NotNull] ZealCpuParser.HeaderDeclarationContext context)
        {
            foreach (var info in context.headerInfo())
            {
                switch (info.headerType.Text)
                {
                    case "CatridgeName":
                        _driver.Header.CatridgeName = parseStringLiteral(info.headerValue.STRING_LITERAL());
                        break;
                    case "RomSpeed":
                        {
                            RomSpeed speed = RomSpeed.SlowROM;
                            if (Enum.TryParse<RomSpeed>(info.headerValue.IDENTIFIER().GetText(), out speed))
                            {
                                _driver.Header.RomSpeed = speed;
                            }
                            break;
                        }
                    case "MapMode":
                        {
                            MapMode mode = MapMode.LoROM;
                            if (Enum.TryParse<MapMode>(info.headerValue.IDENTIFIER().GetText(), out mode))
                            {
                                _driver.Header.MapMode = mode;
                            }
                            break;
                        }
                    case "SramSize":
                        _driver.Header.SramSize = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                        break;
                    case "Country":
                        {
                            Country country = Country.Japan;
                            if (Enum.TryParse<Country>(info.headerValue.IDENTIFIER().GetText(), out country))
                            {
                                _driver.Header.Country = country;
                            }
                            break;
                        }
                    case "Developer":
                        {
                            _driver.Header.Developer = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                            break;
                        }
                    case "Version":
                        {
                            _driver.Header.Version = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public override void ExitVectorsDeclaration([NotNull] ZealCpuParser.VectorsDeclarationContext context)
        {
            foreach(var info in context.vectorInfo())
            {
                switch(info.vectorType.Text)
                {
                    case "COP":
                        _driver.Vectors.COP = info.labelName.Text;
                        break;
                    case "BRK":
                        _driver.Vectors.BRK = info.labelName.Text;
                        break;
                    case "IRQ":
                        _driver.Vectors.IRQ = info.labelName.Text;
                        break;
                    case "NMI":
                        _driver.Vectors.NMI = info.labelName.Text;
                        break;
                    case "Reset":
                        _driver.Vectors.Reset = info.labelName.Text;
                        break;
                    default:
                        break;
                }
            }
        }

        private string parseStringLiteral(ITerminalNode node)
        {
            return node.GetText().Trim('"');
        }

        private int parseNumberLiteral(ZealCpuParser.NumberLiteralContext context)
        {
            int result = 0;

            switch (context.Start.Type)
            {
                case ZealCpuParser.HEX_LITERAL:
                    Int32.TryParse(context.HEX_LITERAL().GetText().Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
                    break;
                case ZealCpuParser.INTEGER_LITERAL:
                    Int32.TryParse(context.INTEGER_LITERAL().GetText(), out result);
                    break;
                case ZealCpuParser.BINARY_LITERAL:
                    {
                        string binaryLiteral = context.BINARY_LITERAL().GetText().Substring(1);
                        int stringLength = binaryLiteral.Length - 1;
                        for (int i = 0; i <= stringLength; ++i)
                        {
                            if (binaryLiteral[stringLength - i] == '1')
                            {
                                result |= (1 << i);
                            }
                        }
                        break;
                    }
            }

            return result;
        }
    }
}
