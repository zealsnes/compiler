namespace Zeal.Compiler.Parser
{
    public class ErrorMessage
    {
        public string SourceFile { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public int StartToken { get; set; }
        public int EndToken { get; set; }
        public string Context { get; set; }
    }
}
