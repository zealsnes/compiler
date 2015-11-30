using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Globalization;
using Zeal.Compiler.Data;
using Zeal.Compiler.Parser;

namespace Zeal.Compiler.Pass
{
    class CpuParseInfoPass : ZealCpuBaseListener
    {
        private ZealCpuDriver _driver;
        private Scope _currentScope;
        private CpuInstructionStatement _currentInstruction;

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

        public override void EnterProcedureDeclaration([NotNull] ZealCpuParser.ProcedureDeclarationContext context)
        {
            _currentScope = new Scope();
            _currentScope.Name = context.name.Text;
            _currentScope.Type = ScopeType.Procedure;
        }

        public override void ExitProcedureDeclaration([NotNull] ZealCpuParser.ProcedureDeclarationContext context)
        {
            _driver.Scopes.Add(_currentScope);
            _currentScope = null;
        }

        public override void EnterInterruptDeclaration([NotNull] ZealCpuParser.InterruptDeclarationContext context)
        {
            _currentScope = new Scope();
            _currentScope.Name = context.name.Text;
            _currentScope.Type = ScopeType.Interrupt;
        }

        public override void ExitInterruptDeclaration([NotNull] ZealCpuParser.InterruptDeclarationContext context)
        {
            _driver.Scopes.Add(_currentScope);
            _currentScope = null;
        }

        public override void EnterInstructionStatement([NotNull] ZealCpuParser.InstructionStatementContext context)
        {
            _currentInstruction = new CpuInstructionStatement();

            CpuInstructions opcode;
            if (Enum.TryParse<CpuInstructions>(context.opcode.Text, out opcode))
            {
                _currentInstruction.Opcode = opcode;
            }
        }

        public override void ExitAddress([NotNull] ZealCpuParser.AddressContext context)
        {
            var argument = new NumberInstructionArgument(parseNumberLiteral(context.numberLiteral()));

            if (argument.Number > byte.MaxValue)
            {
                _currentInstruction.AddressingMode = CpuAddressingMode.Absolute;
            }
            else
            {
                _currentInstruction.AddressingMode = CpuAddressingMode.Direct;
            }

            _currentInstruction.Arguments.Add(argument);
        }

        public override void ExitImmediate([NotNull] ZealCpuParser.ImmediateContext context)
        {
            var argument = new NumberInstructionArgument(parseNumberLiteral(context.numberLiteral()));
            _currentInstruction.Arguments.Add(argument);

            _currentInstruction.AddressingMode = CpuAddressingMode.Immediate;
        }

        public override void ExitInstructionStatement([NotNull] ZealCpuParser.InstructionStatementContext context)
        {
            if (_currentInstruction.Arguments.Count == 0)
            {
                _currentInstruction.AddressingMode = CpuAddressingMode.Implied;
            }

            _currentScope.Statements.Add(_currentInstruction);
            _currentInstruction = null;
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
