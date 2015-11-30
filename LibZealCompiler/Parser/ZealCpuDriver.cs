using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
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

            errorMessage.Line = line;
            errorMessage.Column = charPositionInLine;
            errorMessage.Message = msg;

            _driver.Errors.Add(errorMessage);
        }
    }
    public class ZealCpuDriver
    {
        private RomHeader _header = new RomHeader();
        private Vectors _vectors = new Vectors();
        private List<Scope> _scopes = new List<Scope>();
        private List<ErrorMessage> _errors = new List<ErrorMessage>();

        private ZealCpuLexer _lexer;
        private CommonTokenStream _tokenStream;
        private ZealCpuParser _parser;

        public RomHeader Header
        {
            get
            {
                return _header;
            }
        }

        public Vectors Vectors
        {
            get
            {
                return _vectors;
            }
        }

        public List<Scope> Scopes
        {
            get
            {
                return _scopes;
            }
        }

        public List<ErrorMessage> Errors
        {
            get
            {
                return _errors;
            }
        }

        public ZealCpuDriver(string inputFile)
            : this(new AntlrFileStream(inputFile))
        {
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
            var rootTree = _parser.root();

            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(new CpuParseInfoPass(this), rootTree);
        }
    }
}
