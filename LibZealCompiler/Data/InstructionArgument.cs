namespace Zeal.Compiler.Data
{
    public class InstructionArgument
    {
    }

    public class NumberInstructionArgument : InstructionArgument
    {
        public int Number { get; set; }
        public ArgumentSize Size { get; set; }

        public NumberInstructionArgument(int value, ArgumentSize size)
        {
            Number = value;
            Size = size;
        }
    }
}
