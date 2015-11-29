using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeal.Compiler.Data;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Zeal.Compiler.Pass;

namespace Zeal.Compiler.Parser
{
    public class ZealCpuDriver
    {
        private RomHeader _header = new RomHeader();
        private Vectors _vectors = new Vectors();

        private AntlrInputStream _inputStream;
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

        public ZealCpuDriver(Stream stream)
        {
            _inputStream = new AntlrInputStream(stream);
            _lexer = new ZealCpuLexer(_inputStream);
            _tokenStream = new CommonTokenStream(_lexer);
            _parser = new ZealCpuParser(_tokenStream);
        }

        public void Parse()
        {
            var rootTree = _parser.root();

            ParseTreeWalker walker = new ParseTreeWalker();
            walker.Walk(new CpuParseInfoPass(this), rootTree);
        }
    }
}
