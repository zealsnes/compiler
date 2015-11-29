using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
