using System;
using System.IO;
using Zeal.Compiler.Parser;

namespace ZealCompiler
{
    class Program
    {
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
            driver.Parse();

            if (driver.Errors.Count > 0)
            {
                string fullPath = Path.GetFullPath(args[0]);

                foreach(var error in driver.Errors)
                {
                    Console.Error.WriteLine("{0}({1},{2}): error: {3}", fullPath, error.Line, error.Column, error.Message);
                }

                Console.Read();
                return 1;
            }

            return 0;
        }
    }
}
