using Zeal.Compiler.CodeGeneration;

namespace Zeal.Compiler.Data
{
    public enum CpuInstructions
    {
        [Opcode(CpuAddressingMode.Immediate, 0x69)]
        adc,

        [Opcode(CpuAddressingMode.Immediate, 0x29)]
        and,

        [Opcode(0x0A)]
        asl,

        bcc,
        bcs,
        beq,

        [Opcode(CpuAddressingMode.Immediate, 0x89)]
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
        cmp,

        cop,

        [Opcode(CpuAddressingMode.Immediate, 0xE0)]
        cpx,

        [Opcode(CpuAddressingMode.Immediate, 0xC0)]
        cpy,

        [Opcode(0x3A)]
        dec,

        [Opcode(0xCA)]
        dex,

        [Opcode(0x88)]
        dey,

        [Opcode(CpuAddressingMode.Immediate, 0x49)]
        eor,

        [Opcode(0x1A)]
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
        lda,

        [Opcode(CpuAddressingMode.Immediate, 0xA2)]
        ldx,

        [Opcode(CpuAddressingMode.Immediate, 0xA0)]
        ldy,

        [Opcode(0x4A)]
        lsr,

        mvn,
        mvp,

        [Opcode(0xEA)]
        nop,

        [Opcode(CpuAddressingMode.Immediate, 0x09)]
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
        rol,

        [Opcode(0x6A)]
        ror,

        [Opcode(0x40)]
        rti,

        [Opcode(0x6B)]
        rtl,

        [Opcode(0x60)]
        rts,

        [Opcode(CpuAddressingMode.Immediate, 0xE9)]
        sbc,

        [Opcode(0x38)]
        sec,

        [Opcode(0xF8)]
        sed,

        [Opcode(0x78)]
        sei,

        [Opcode(CpuAddressingMode.Immediate, 0xE2)]
        sep,

        sta,

        [Opcode(0xDB)]
        stp,

        stx,
        sty,
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

        trb,
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
