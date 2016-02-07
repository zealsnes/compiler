using System;
using System.Collections.Generic;
using System.IO;
using Zeal.Compiler.CodeGeneration;
using Zeal.Compiler.Data;
using Zeal.Compiler.Parser;

namespace ZealCompiler
{
    class Program
    {
        static void printErrorMessage(ErrorMessage error)
        {
            Console.Error.WriteLine("{0}({1},{2}): error: {3}", error.SourceFile, error.Line, error.Column, error.Message);
            if (!String.IsNullOrEmpty(error.Context))
            {
                Console.Error.WriteLine(error.Context);
                for (int i = 0; i < error.Column; ++i)
                {
                    Console.Error.Write(' ');
                }
                for(int i=error.StartToken; i<=error.EndToken; ++i)
                {
                    Console.Error.Write("^");
                }
                Console.Error.WriteLine();
            }
        }

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("No source file provided.");
                return 1;
            }

            if (Path.GetExtension(args[0]) != ".zcpu")
            {
                Console.Error.WriteLine("The source file is in the wrong format. The source file must ends with .zcpu.");
                return 1;
            }

            ZealCpuDriver driver = new ZealCpuDriver(args[0]);
            try
            {
                driver.Parse();
                driver.SecondPass();
            }
            catch(CompilerErrorException)
            {
                foreach (var error in driver.Errors)
                {
                    printErrorMessage(error);
                }

#if DEBUG
                Console.Read();
#endif

                return 1;
            }

            FileStream outputRom = new FileStream(Path.ChangeExtension(args[0], ".sfc"), FileMode.Create);

            CpuCodeGenerator codeGenerator = new CpuCodeGenerator(outputRom);
            codeGenerator.Header = driver.Header;

            foreach (var scope in driver.GlobalScope.Children)
            {
                codeGenerator.Scope = scope;

                List<CpuInstructionStatement> instructions = new List<CpuInstructionStatement>();

                foreach (var statement in scope.Statements)
                {
                    if (statement is CpuInstructionStatement)
                    {
                        instructions.Add((CpuInstructionStatement)statement);
                    }
                }

                codeGenerator.Instructions = instructions;
                codeGenerator.Generate();
            }

            SfcRomWriter romWriter = new SfcRomWriter(outputRom);
            romWriter.Driver = driver;
            romWriter.Write();

            outputRom.Close();

            using (FileStream newRom = new FileStream(Path.ChangeExtension(args[0], ".sfc"), FileMode.Open))
            {
                SfcRomWriter checksumWriter = new SfcRomWriter(newRom);
                checksumWriter.Driver = driver;
                checksumWriter.ComputeChecksum();
            }

            return 0;
        }
    }
}
