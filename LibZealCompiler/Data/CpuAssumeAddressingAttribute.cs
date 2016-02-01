using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public class CpuAssumeAddressingAttribute : Attribute
    {
        public CpuAddressingMode Addressing { get; private set; }

        public CpuAssumeAddressingAttribute(CpuAddressingMode addressing)
        {
            Addressing = addressing;
        }
    }
}
