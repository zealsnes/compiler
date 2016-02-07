using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Zeal.Compiler.Parser;
using Zeal.Compiler.Data;

namespace Zeal.Compiler.Pass
{
    class SecondCpuPass : CpuPass
    {
        private Scope _currentScope;

        public SecondCpuPass(ZealCpuDriver driver)
            : base(driver)
        {
        }

        public override void EnterProcedureDeclaration([NotNull] ZealCpuParser.ProcedureDeclarationContext context)
        {
            _currentScope = _driver.GlobalScope.GetScope(context.name.Text);
        }

        public override void EnterInterruptDeclaration([NotNull] ZealCpuParser.InterruptDeclarationContext context)
        {
            _currentScope = _driver.GlobalScope.GetScope(context.name.Text);
        }

        public override void ExitVectorInfo([NotNull] ZealCpuParser.VectorInfoContext context)
        {
            if (!_driver.GlobalScope.IsLabelValid(context.labelName.Text))
            {
                addErrorMesage(String.Format("Label '{0}' not found for vector {1}.", context.labelName.Text, context.vectorType.Text), context.labelName);
            }
        }

        public override void ExitArgumentLiteral([NotNull] ZealCpuParser.ArgumentLiteralContext context)
        {
            if (_currentScope != null && context.IDENTIFIER() != null)
            {
                string labelName = context.IDENTIFIER().GetText();
                if (!_currentScope.IsLabelValid(labelName))
                {
                    addErrorMesage(String.Format("Label '{0}' not found.", labelName), context.IDENTIFIER().Symbol);
                }
            }
        }
    }
}
