using Zeal.Compiler.CodeGeneration;

namespace Zeal.Compiler.Data
{
    public enum CpuInstructions
    {
        [Opcode(CpuAddressingMode.Immediate, 0x69)]
        [Opcode(CpuAddressingMode.Direct, 0x65)]
        adc,

        [Opcode(CpuAddressingMode.Immediate, 0x29)]
        [Opcode(CpuAddressingMode.Direct, 0x25)]
        and,

        [Opcode(0x0A)]
        [Opcode(CpuAddressingMode.Direct, 0x06)]
        asl,

        bcc,
        bcs,
        beq,

        [Opcode(CpuAddressingMode.Immediate, 0x89)]
        [Opcode(CpuAddressingMode.Direct, 0x24)]
        bit,

        bmi,
        bne,
        bpl,
        bra,

        [Opcode(0x00)]
        brk,

        brl,
        bvc,
        bvs,

        [Opcode(0x18)]
        clc,

        [Opcode(0xD8)]
        cld,

        [Opcode(0x58)]
        cli,

        [Opcode(0xB8)]
        clv,

        [Opcode(CpuAddressingMode.Immediate, 0xC9)]
        [Opcode(CpuAddressingMode.Direct, 0xC5)]
        cmp,

        cop,

        [Opcode(CpuAddressingMode.Immediate, 0xE0)]
        [Opcode(CpuAddressingMode.Direct, 0xE4)]
        cpx,

        [Opcode(CpuAddressingMode.Immediate, 0xC0)]
        [Opcode(CpuAddressingMode.Direct, 0xC4)]
        cpy,

        [Opcode(0x3A)]
        [Opcode(CpuAddressingMode.Direct, 0xC6)]
        dec,

        [Opcode(0xCA)]
        dex,

        [Opcode(0x88)]
        dey,

        [Opcode(CpuAddressingMode.Immediate, 0x49)]
        [Opcode(CpuAddressingMode.Direct, 0x45)]
        eor,

        [Opcode(0x1A)]
        [Opcode(CpuAddressingMode.Direct, 0xE6)]
        inc,

        [Opcode(0xE8)]
        inx,

        [Opcode(0xC8)]
        iny,

        jmp,
        jml,
        jsr,
        jsl,

        [Opcode(CpuAddressingMode.Immediate, 0xA9)]
        [Opcode(CpuAddressingMode.Direct, 0xA5)]
        lda,

        [Opcode(CpuAddressingMode.Immediate, 0xA2)]
        [Opcode(CpuAddressingMode.Direct, 0xA6)]
        ldx,

        [Opcode(CpuAddressingMode.Immediate, 0xA0)]
        [Opcode(CpuAddressingMode.Direct, 0xA4)]
        ldy,

        [Opcode(0x4A)]
        [Opcode(CpuAddressingMode.Direct, 0x46)]
        lsr,

        mvn,
        mvp,

        [Opcode(0xEA)]
        nop,

        [Opcode(CpuAddressingMode.Immediate, 0x09)]
        [Opcode(CpuAddressingMode.Direct, 0x05)]
        ora,

        pea,
        per,

        [Opcode(0x48)]
        pha,

        [Opcode(0x8B)]
        phb,

        [Opcode(0x0B)]
        phd,

        [Opcode(0x4B)]
        phk,

        [Opcode(0x08)]
        php,

        [Opcode(0xDA)]
        phx,

        [Opcode(0x5A)]
        phy,

        [Opcode(0x68)]
        pla,

        [Opcode(0xAB)]
        plb,

        [Opcode(0x2B)]
        pld,

        [Opcode(0x28)]
        plp,

        [Opcode(0xFA)]
        plx,

        [Opcode(0x7A)]
        ply,

        [Opcode(CpuAddressingMode.Immediate, 0xC2)]
        rep,

        [Opcode(0x2A)]
        [Opcode(CpuAddressingMode.Direct, 0x26)]
        rol,

        [Opcode(0x6A)]
        [Opcode(CpuAddressingMode.Direct, 0x66)]
        ror,

        [Opcode(0x40)]
        rti,

        [Opcode(0x6B)]
        rtl,

        [Opcode(0x60)]
        rts,

        [Opcode(CpuAddressingMode.Immediate, 0xE9)]
        [Opcode(CpuAddressingMode.Direct, 0xE5)]
        sbc,

        [Opcode(0x38)]
        sec,

        [Opcode(0xF8)]
        sed,

        [Opcode(0x78)]
        sei,

        [Opcode(CpuAddressingMode.Immediate, 0xE2)]
        sep,

        [Opcode(CpuAddressingMode.Direct, 0x85)]
        sta,

        [Opcode(0xDB)]
        stp,

        [Opcode(CpuAddressingMode.Direct, 0x86)]
        stx,

        [Opcode(CpuAddressingMode.Direct, 0x84)]
        sty,

        [Opcode(CpuAddressingMode.Direct, 0x64)]
        stz,

        [Opcode(0xAA)]
        tax,

        [Opcode(0xA8)]
        tay,

        [Opcode(0x5B)]
        tcd,

        [Opcode(0x1B)]
        tcs,

        [Opcode(0x7B)]
        tdc,

        [Opcode(CpuAddressingMode.Direct, 0x14)]
        trb,

        [Opcode(CpuAddressingMode.Direct, 0x04)]
        tsb,

        [Opcode(0x3B)]
        tsc,

        [Opcode(0xBA)]
        tsx,

        [Opcode(0x8A)]
        txa,

        [Opcode(0x9A)]
        txs,

        [Opcode(0x9B)]
        txy,

        [Opcode(0x98)]
        tya,

        [Opcode(0xBB)]
        tyx,

        [Opcode(0xCB)]
        wai,

        [Opcode(0xEB)]
        xba,

        [Opcode(0xFB)]
        xce
    }
}
