using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Linq;
using System.Globalization;
using Zeal.Compiler.Data;
using Zeal.Compiler.Parser;
using Zeal.Compiler.Helper;
using System.ComponentModel;

namespace Zeal.Compiler.Pass
{
    class ParseCpuPass : CpuPass
    {
        private Scope _currentScope;
        private CpuInstructionStatement _currentInstruction;

        public ParseCpuPass(ZealCpuDriver driver)
            : base(driver)
        {
        }

        public override void ExitHeaderDeclaration([NotNull] ZealCpuParser.HeaderDeclarationContext context)
        {
            foreach (var info in context.headerInfo())
            {
                switch (info.headerType.Text)
                {
                    case "CartridgeName":
                        if (info.headerValue.STRING_LITERAL() == null)
                        {
                            addErrorMesage("CartridgeName excepts a string literal.", info.headerValue.Start);
                        }
                        else
                        {
                            _driver.Header.CartridgeName = parseStringLiteral(info.headerValue.STRING_LITERAL());
                        }
                        break;
                    case "RomSpeed":
                        {
                            RomSpeed speed = RomSpeed.SlowROM;
                            if (Enum.TryParse<RomSpeed>(info.headerValue.IDENTIFIER().GetText(), out speed))
                            {
                                _driver.Header.RomSpeed = speed;
                            }
                            else
                            {
                                string validInput = String.Join(", ", Enum.GetNames(typeof(RomSpeed)));
                                addErrorMesage(String.Format("'{0}' is not a valid RomSpeed. Valid inputs are: {1}", info.headerValue.IDENTIFIER().GetText(), validInput), info.headerValue.IDENTIFIER().Symbol);
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
                            else
                            {
                                string validInput = String.Join(", ", Enum.GetNames(typeof(MapMode)));
                                addErrorMesage(String.Format("'{0}' is not a valid MapMode. Valid inputs are: {1}", info.headerValue.IDENTIFIER().GetText(), validInput), info.headerValue.IDENTIFIER().Symbol);
                            }
                            break;
                        }
                    case "SramSize":
                        if (info.headerValue.numberLiteral() == null)
                        {
                            addErrorMesage("SramSize expects a number literal.", info.headerValue.Start);
                        }
                        else
                        {
                            _driver.Header.SramSize = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                        }
                        break;
                    case "Country":
                        {
                            Country country = Country.Japan;
                            if (Enum.TryParse<Country>(info.headerValue.IDENTIFIER().GetText(), out country))
                            {
                                _driver.Header.Country = country;
                            }
                            else
                            {
                                string validInput = String.Join(", ", Enum.GetNames(typeof(Country)));
                                addErrorMesage(String.Format("'{0}' is not a valid Country. Valid inputs are: {1}", info.headerValue.IDENTIFIER().GetText(), validInput), info.headerValue.IDENTIFIER().Symbol);
                            }
                            break;
                        }
                    case "Developer":
                        {
                            if (info.headerValue.numberLiteral() == null)
                            {
                                addErrorMesage("Developer expects a number literal.", info.headerValue.Start);
                            }
                            else
                            {
                                _driver.Header.Developer = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                            }
                            break;
                        }
                    case "Version":
                        {
                            if (info.headerValue.numberLiteral() == null)
                            {
                                addErrorMesage("Version expects a number literal.", info.headerValue.Start);
                            }
                            else
                            {
                                _driver.Header.Version = (uint)parseNumberLiteral(info.headerValue.numberLiteral());
                            }
                            break;
                        }
                    default:
                        addErrorMesage(String.Format("'{0}' is not a valid entry name for the header statement.", info.headerType.Text), info.headerType);
                        break;
                }
            }
        }

        public override void ExitVectorsDeclaration([NotNull] ZealCpuParser.VectorsDeclarationContext context)
        {
            _driver.Vectors = new Vectors();

            foreach (var info in context.vectorInfo())
            {
                switch (info.vectorType.Text)
                {
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
                        addErrorMesage(String.Format("'{0}' is not a valid entry name for the vectors statement.", info.vectorType.Text), info.vectorType);
                        break;
                }
            }

            if (String.IsNullOrEmpty(_driver.Vectors.BRK))
            {
                addErrorMesage("BRK vector should be defined.", context.Start);
            }
            if (String.IsNullOrEmpty(_driver.Vectors.IRQ))
            {
                addErrorMesage("IRQ vector should be defined.", context.Start);
            }
            if (String.IsNullOrEmpty(_driver.Vectors.NMI))
            {
                addErrorMesage("NMI vector should be defined.", context.Start);
            }
            if (String.IsNullOrEmpty(_driver.Vectors.Reset))
            {
                addErrorMesage("Reset vector should be defined.", context.Start);
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
            _driver.GlobalScope.Add(_currentScope);
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
            var rtiInstruction = new CpuInstructionStatement();
            rtiInstruction.Opcode = CpuInstructions.rti;
            rtiInstruction.AddressingMode = CpuAddressingMode.Implied;

            _currentScope.Statements.Add(rtiInstruction);

            _driver.GlobalScope.Add(_currentScope);
            _currentScope = null;
        }

        public override void EnterInstructionStatement([NotNull] ZealCpuParser.InstructionStatementContext context)
        {
            _currentInstruction = new CpuInstructionStatement();

            _currentInstruction.Line = context.Start.Line;
            _currentInstruction.Column = context.Start.Column;

            CpuInstructions opcode;
            if (Enum.TryParse<CpuInstructions>(context.opcode().GetText(), out opcode))
            {
                _currentInstruction.Opcode = opcode;
            }
        }

        public override void ExitAddress([NotNull] ZealCpuParser.AddressContext context)
        {
            var numberLiteral = context.argumentLiteral().numberLiteral();
            InstructionArgument argument = null;

            if (numberLiteral != null)
            {
                argument = parseNumberArgument(numberLiteral);

                if (((NumberInstructionArgument)argument).Size == ArgumentSize.Word)
                {
                    _currentInstruction.AddressingMode = CpuAddressingMode.Absolute;
                }
                else if (((NumberInstructionArgument)argument).Size == ArgumentSize.LongWord)
                {
                    _currentInstruction.AddressingMode = CpuAddressingMode.AbsoluteLong;
                }
                else
                {
                    _currentInstruction.AddressingMode = CpuAddressingMode.Direct;
                }
            }

            var identifierLitteral = context.argumentLiteral().IDENTIFIER();
            if (identifierLitteral != null)
            {
                argument = new LabelInstructionArgument(identifierLitteral.GetText());
                _currentInstruction.AddressingMode = CpuAddressingMode.Absolute;
            }

            if (argument != null)
            {
                _currentInstruction.Arguments.Add(argument);
            }
        }

        public override void ExitImmediate([NotNull] ZealCpuParser.ImmediateContext context)
        {
            var numberLiteral = context.numberLiteral();
            if (numberLiteral != null)
            {
                _currentInstruction.Arguments.Add(parseNumberArgument(numberLiteral));

                _currentInstruction.AddressingMode = CpuAddressingMode.Immediate;
            }
        }

        public override void ExitInstructionStatement([NotNull] ZealCpuParser.InstructionStatementContext context)
        {
            if (_currentInstruction.Arguments.Count == 0)
            {
                _currentInstruction.AddressingMode = CpuAddressingMode.Implied;
            }

            var assumeAddressingAttribute = _currentInstruction.Opcode.GetAttribute<CpuAssumeAddressingAttribute>();
            if (assumeAddressingAttribute != null)
            {
                _currentInstruction.AddressingMode = assumeAddressingAttribute.Addressing;
            }

            var labelContext = context.label();
            if (labelContext != null)
            {
                _currentInstruction.AssociatedLabel = labelContext.IDENTIFIER().GetText();
            }

            var opcodeAttributes = EnumHelper.GetAttributes<OpcodeAttribute>(_currentInstruction.Opcode).Where(x => x.AddressingMode == _currentInstruction.AddressingMode).ToArray();
            if (opcodeAttributes == null || (opcodeAttributes != null && opcodeAttributes.Length == 0))
            {
                string descriptionAddressingMode = EnumHelper.GetAttribute<DescriptionAttribute>(_currentInstruction.AddressingMode).Description;
                addErrorMesage(String.Format("opcode '{0}' does not support {1} addressing mode.", _currentInstruction.Opcode, descriptionAddressingMode), context.opcode().Start);
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
            return parseNumberArgument(context).Number;
        }

        private NumberInstructionArgument parseNumberArgument(ZealCpuParser.NumberLiteralContext context)
        {
            int result = 0;
            ArgumentSize size = ArgumentSize.Byte;

            switch (context.Start.Type)
            {
                case ZealCpuParser.HEX_LITERAL:
                    {
                        string hexText = context.HEX_LITERAL().GetText().Substring(1);
                        if (Int32.TryParse(hexText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
                        {
                            if (hexText.Length > 4 && hexText.Length <= 6)
                            {
                                size = ArgumentSize.LongWord;
                            }
                            else if (hexText.Length > 2 && hexText.Length <= 4)
                            {
                                size = ArgumentSize.Word;
                            }
                        }
                        break;
                    }
                case ZealCpuParser.INTEGER_LITERAL:
                    if (Int32.TryParse(context.INTEGER_LITERAL().GetText(), out result))
                    {
                        if (result > ushort.MaxValue)
                        {
                            size = ArgumentSize.LongWord;
                        }
                        else if (result > byte.MaxValue)
                        {
                            size = ArgumentSize.Word;
                        }
                    }
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
                        if (binaryLiteral.Length > 16 && binaryLiteral.Length <= 24)
                        {
                            size = ArgumentSize.LongWord;
                        }
                        else if (binaryLiteral.Length > 8 && binaryLiteral.Length <= 16)
                        {
                            size = ArgumentSize.Word;
                        }
                        break;
                    }
            }

            return new NumberInstructionArgument(result, size);
        }
    }
}
