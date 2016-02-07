using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeal.Compiler.Parser;

namespace Zeal.Compiler.Pass
{
    class CpuPass : ZealCpuBaseListener
    {
        protected ZealCpuDriver _driver;

        public CpuPass(ZealCpuDriver driver)
        {
            _driver = driver;
        }

        protected void addErrorMesage(string message, IToken offendingToken)
        {
            ErrorMessage error = new ErrorMessage();
            error.SourceFile = _driver.SourceFilePath;
            error.Message = message;

            if (offendingToken != null)
            {
                error.Context = offendingToken.InputStream.ToString().Split('\n')[offendingToken.Line - 1].Replace('\t', ' ').TrimEnd();
                error.Line = offendingToken.Line;
                error.Column = offendingToken.Column;
                error.StartToken = offendingToken.StartIndex;
                error.EndToken = offendingToken.StopIndex;
            }

            _driver.Errors.Add(error);
        }
    }
}
