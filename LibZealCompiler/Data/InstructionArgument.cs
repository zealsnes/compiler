using System.IO;

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

    public class LabelInstructionArgument : InstructionArgument
    {
        public string Label { get; set; }

        public LabelInstructionArgument(string label)
        {
            Label = label;
        }
    }
}
