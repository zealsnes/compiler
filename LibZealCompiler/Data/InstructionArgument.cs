using System.IO;

namespace Zeal.Compiler.Data
{
    public class InstructionArgument
    {
        public virtual long ComputeSize()
        {
            return -1;
        }
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

        public override long ComputeSize()
        {
            switch (Size)
            {
                case ArgumentSize.Byte:
                    return 1;
                case ArgumentSize.Word:
                    return 2;
                case ArgumentSize.LongWord:
                    return 3;
            }

            return -1;
        }
    }

    public class LabelInstructionArgument : InstructionArgument
    {
        public string Label { get; set; }

        public LabelInstructionArgument(string label)
        {
            Label = label;
        }

        public override long ComputeSize()
        {
            return base.ComputeSize();
        }
    }
}
