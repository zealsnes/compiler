namespace Zeal.Compiler.Data
{
    public class Statement
    {
        public string AssociatedLabel { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }

        public Statement()
        {
            AssociatedLabel = null;
        }

        public virtual long ComputeSize()
        {
            return 0;
        }
    }
}
