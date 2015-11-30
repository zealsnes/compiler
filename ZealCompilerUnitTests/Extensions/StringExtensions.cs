using System.IO;
using System.Text;

namespace Zeal.Compiler.UnitTests.Extensions
{
    static class StringExtensions
    {
        public static MemoryStream ToMemoryStream(this string input)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(input));
        }
    }
}
