using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using Zeal.Compiler.Data;
using Zeal.Compiler.Pass;

namespace Zeal.Compiler.Parser
{
    class CpuErrorListener : BaseErrorListener
    {
        private ZealCpuDriver _driver;

        public CpuErrorListener(ZealCpuDriver driver)
        {
            _driver = driver;
        }

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            ErrorMessage errorMessage = new ErrorMessage();

            CommonTokenStream tokens = (CommonTokenStream)recognizer.InputStream;

            errorMessage.SourceFile = _driver.SourceFilePath;
            errorMessage.Context = tokens.TokenSource.InputStream.ToString().Split('\n')[line - 1].Replace('\t', ' ').TrimEnd();
            errorMessage.Line = line;
            errorMessage.Column = charPositionInLine;
            errorMessage.Message = msg;
            errorMessage.StartToken = offendingSymbol.StartIndex;
            errorMessage.EndToken = offendingSymbol.StopIndex;

            _driver.Errors.Add(errorMessage);
        }
    }

    public class ZealCpuDriver
    {
        private RomHeader _header = new RomHeader();
        private List<Scope> _scopes = new List<Scope>();
        private Scope _globalScope = new Scope();
        private List<ErrorMessage> _errors = new List<ErrorMessage>();

        private ZealCpuLexer _lexer;
        private CommonTokenStream _tokenStream;
        private ZealCpuParser _parser;
        private ZealCpuParser.RootContext _rootTree;

        public RomHeader Header
        {
            get
            {
                return _header;
            }
        }

        public Vectors Vectors
        {
            get;
            set;
        }

        public Scope GlobalScope
        {
            get
            {
                return _globalScope;
            }
        }

        public List<ErrorMessage> Errors
        {
            get
            {
                return _errors;
            }
        }

        internal string SourceFilePath { get; set; }

        public ZealCpuDriver(string inputFile)
            : this(new AntlrFileStream(inputFile))
        {
            SourceFilePath = Path.GetFullPath(inputFile);
        }

        public ZealCpuDriver(Stream stream)
            : this(new AntlrInputStream(stream))
        {
        }

        private ZealCpuDriver(ICharStream antlrInputStream)
        {
            _lexer = new ZealCpuLexer(antlrInputStream);
            _tokenStream = new CommonTokenStream(_lexer);
            _parser = new ZealCpuParser(_tokenStream);
            _parser.RemoveErrorListeners();
            _parser.AddErrorListener(new CpuErrorListener(this));
        }

        public void Parse()
        {
            _rootTree = _parser.root();

            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(new ParseCpuPass(this), _rootTree);

            if (Errors.Count > 0)
            {
                throw new CompilerErrorException();
            }

            resolveLabels();
            verifyBranchLength();

            if (Errors.Count > 0)
            {
                throw new CompilerErrorException();
            }
        }

        public void SecondPass()
        {
            if (Vectors == null)
            {
                ErrorMessage error = new ErrorMessage();
                error.SourceFile = SourceFilePath;
                error.Message = "Required vectors statement not found.";
                Errors.Add(error);
            }

            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(new SecondCpuPass(this), _rootTree);

            if (Errors.Count > 0)
            {
                throw new CompilerErrorException();
            }
        }

        private void resolveLabels()
        {
            long physicalAddress = 0;

            Scope parentScope = _globalScope;
            foreach (var scope in parentScope.Children)
            {
                parentScope.Labels[scope.Name] = physicalAddress;

                foreach (var instruction in scope.Statements)
                {
                    if (!String.IsNullOrEmpty(instruction.AssociatedLabel))
                    {
                        scope.Labels[instruction.AssociatedLabel] = physicalAddress;
                    }

                    physicalAddress += instruction.ComputeSize();
                }
            }
        }

        private void verifyBranchLength()
        {
            long physicalAddress = 0;

            Scope parentScope = _globalScope;
            foreach (var scope in parentScope.Children)
            {
                foreach (var instruction in scope.Statements)
                {
                    if (instruction is CpuInstructionStatement)
                    {
                        var cpuInstruction = instruction as CpuInstructionStatement;
                        if (cpuInstruction.AddressingMode == CpuAddressingMode.Relative)
                        {
                            int instructionSize = (int)instruction.ComputeSize();

                            if (cpuInstruction.Arguments[0] is LabelInstructionArgument)
                            {
                                var labelArgument = cpuInstruction.Arguments[0] as LabelInstructionArgument;
                                int labelAddress = (int)scope.AddressFor(labelArgument.Label);

                                int branchLength = labelAddress - ((int)physicalAddress + instructionSize);
                                if (branchLength > sbyte.MaxValue || branchLength < sbyte.MinValue)
                                {
                                    ErrorMessage error = new ErrorMessage();
                                    error.SourceFile = SourceFilePath;
                                    error.Message = String.Format("Branch label '{0}' is too far away. Consider reducing the distance or use a jmp.", labelArgument.Label);
                                    error.Line = instruction.Line;
                                    error.Column = instruction.Column;

                                    Errors.Add(error);
                                }
                            }
                        }
                    }

                    physicalAddress += instruction.ComputeSize();
                }
            }
        }
    }
}
