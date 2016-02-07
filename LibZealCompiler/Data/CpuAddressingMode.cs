using System.ComponentModel;

namespace Zeal.Compiler.Data
{
    public enum CpuAddressingMode
    {
        [Description("implied")]
        Implied,
        [Description("immediate")]
        Immediate,
        [Description("relative")]
        Relative,
        [Description("relative long")]
        RelativeLong,
        [Description("direct")]
        Direct,
        [Description("direct indexed X")]
        DirectIndexedX,
        [Description("direct indexed Y")]
        DirectIndexedY,
        [Description("direct indirect")]
        DirectIndirect,
        [Description("direct indexed indirect")]
        DirectIndexedIndirect,
        [Description("direct indirect long")]
        DirectIndirectLong,
        [Description("direct indirect indexed long")]
        DirectIndirectIndexedLong,
        [Description("absolute")]
        Absolute,
        [Description("absolute indexed X")]
        AbsoluteIndexedX,
        [Description("absolute indexed Y")]
        AbsoluteIndexedY,
        [Description("absolute long")]
        AbsoluteLong,
        [Description("absolute indexed long")]
        AbsoluteIndexedLong,
        [Description("stack relative")]
        StackRelative,
        [Description("stack relative indirect indexed")]
        StackRelativeIndirectIndexed,
        [Description("absolute indirect")]
        AbsoluteIndirect,
        [Description("absolute indirect")]
        AbsoluteIndirectLong,
        [Description("absolute indexed indirect")]
        AbsoluteIndexedIndirect,
        [Description("block move")]
        BlockMove
    }
}
