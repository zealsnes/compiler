using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeal.Compiler.Data
{
    public enum CpuAddressingMode
    {
        Implied,
        Immediate,
        Relative,
        RelativeLong,
        Direct,
        DirectIndexedX,
        DirectIndexedY,
        DirectIndirect,
        DirectIndexedIndirect,
        DirectIndirectLong,
        DirectIndirectIndexedLong,
        Absolute,
        AbsoluteIndexedX,
        AbsoluteIndexedY,
        AbsoluteLong,
        AbsoluteIndexedLong,
        StackRelative,
        StackRelativeIndirectIndexed,
        AbsoluteIndirect,
        AbsoluteIndirectLong,
        AbsoluteIndexedIndirect,
        BlockMove
    }
}
