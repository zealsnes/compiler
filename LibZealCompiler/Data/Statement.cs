namespace Zeal.Compiler.Data
{
    public class Statement
    {
        public string AssociatedLabel { get; set; }

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
