using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeal.Compiler.Parser;
using Zeal.Compiler.Helper;
using Zeal.Compiler.Data;

namespace Zeal.Compiler.CodeGeneration
{
    public static class SfcRomHeader
    {
        public const int CatridgeHeader = 0x00FFC0;
        public const int Checksum = 0x00FFDC;

        public const int NativeVectors = 0x00FFE4;

        public const int EmulationCopVector = 0x00FFF4;
        public const int EmulationVectors = 0x00FFF8;
    }

    public class SfcRomWriter
    {
        private Stream _writer;

        public ZealCpuDriver Driver { get; set; }

        private Scope GlobalScope
        {
            get
            {
                return Driver.GlobalScope;
            }
        }

        public SfcRomWriter(Stream stream)
        {
            _writer = stream;
        }

        public void Write()
        {
            writeCatridgeHeader();
            writeVectors();
        }

        public void ComputeChecksum()
        {
            _writer.Seek(0, SeekOrigin.End);

            long fileSize = _writer.Position;

            byte[] allBytes = new byte[fileSize];

            _writer.Seek(0, SeekOrigin.Begin);

            _writer.Read(allBytes, 0, allBytes.Length);

            uint checksum = 0;

            for(int i=0; i<allBytes.Length; ++i)
            {
                checksum += (uint)allBytes[i];
            }

            uint checksumComplement = checksum ^ uint.MaxValue;

            _writer.Seek(CpuAddressConverter.RAMToPhysical(SfcRomHeader.Checksum, Driver.Header.MapMode), SeekOrigin.Begin);

            using (BinaryWriter binWriter = new BinaryWriter(_writer))
            {
                binWriter.Write((ushort)checksumComplement);
                binWriter.Write((ushort)checksum);
            }
        }

        private void writeCatridgeHeader()
        {
            MapMode map = Driver.Header.MapMode;

            _writer.Seek(CpuAddressConverter.RAMToPhysical(SfcRomHeader.CatridgeHeader, map), SeekOrigin.Begin);

            // Write cartridge title
            byte[] cartridgeTitle = new byte[21];
            for (int i = 0; i < cartridgeTitle.Length; ++i)
            {
                cartridgeTitle[i] = (byte)' ';
            }
            Array.Copy(Encoding.ASCII.GetBytes(Driver.Header.CatridgeName), cartridgeTitle, Encoding.ASCII.GetByteCount(Driver.Header.CatridgeName));

            _writer.Write(cartridgeTitle, 0, cartridgeTitle.Length);

            // Write ROM Makeup / ROM Speed and map mode
            int romMakeup = 0;
            romMakeup = (byte)Driver.Header.MapMode
                | ((byte)Driver.Header.RomSpeed << 4)
                | 1 << 5
                ;

            _writer.WriteByte((byte)romMakeup);

            // Write Chipset
            byte chipset = 0;
            if (Driver.Header.SramSize > 0)
            {
                chipset = 0x2;
            }

            _writer.WriteByte(chipset);

            // Write ROM size
            byte romSize = (byte)Math.Log(32, 2);
            _writer.WriteByte(romSize);

            // Write RAM size
            _writer.WriteByte((byte)Math.Log(Driver.Header.SramSize, 2));

            // Write Country
            _writer.WriteByte((byte)Driver.Header.Country);

            // Write Developer ID
            _writer.WriteByte((byte)Driver.Header.Developer);

            // Write ROM version
            _writer.WriteByte((byte)Driver.Header.Version);

            // Write initial checksum complement
            _writer.WriteByte(0xFF);
            _writer.WriteByte(0xFF);

            // Write initial checksum
            _writer.WriteByte(0);
            _writer.WriteByte(0);
        }

        private void writeVectors()
        {
            MapMode map = Driver.Header.MapMode;

            _writer.Seek(CpuAddressConverter.RAMToPhysical(SfcRomHeader.NativeVectors, map), SeekOrigin.Begin);

            using (BinaryWriter binWriter = new BinaryWriter(_writer))
            {
                Vectors vectors = Driver.Vectors;
                RomSpeed speed = Driver.Header.RomSpeed;

                // ==========================
                // = Native (65816 vectors) =
                // ==========================

                // COP vector
                int cop = (int)GlobalScope.AddressFor(vectors.COP);
                cop = CpuAddressConverter.PhysicalToRAM(cop, map, speed);

                binWriter.Write((ushort)cop);

                // BRK vector
                int brk = (int)GlobalScope.AddressFor(vectors.BRK);
                brk = CpuAddressConverter.PhysicalToRAM(brk, map, speed);

                binWriter.Write((ushort)brk);

                // ABORT vector
                binWriter.Write((ushort)brk);

                // NMI vector
                int nmi = (int)GlobalScope.AddressFor(vectors.NMI);
                nmi = CpuAddressConverter.PhysicalToRAM(nmi, map, speed);

                binWriter.Write((ushort)nmi);

                // RESET vector
                int reset = (int)GlobalScope.AddressFor(vectors.Reset);
                reset = CpuAddressConverter.PhysicalToRAM(reset, map, speed);

                binWriter.Write((ushort)reset);

                // IRQ vector
                int irq = (int)GlobalScope.AddressFor(vectors.IRQ);
                irq = CpuAddressConverter.PhysicalToRAM(irq, map, speed);

                binWriter.Write((ushort)irq);

                // ============================
                // = Emulation (6502 vectors) =
                // ============================

                // COP vector
                binWriter.Seek(CpuAddressConverter.RAMToPhysical(SfcRomHeader.EmulationCopVector, map), SeekOrigin.Begin);

                binWriter.Write((ushort)cop);

                // Rest of 6502 vectors
                binWriter.Seek(CpuAddressConverter.RAMToPhysical(SfcRomHeader.EmulationVectors, map), SeekOrigin.Begin);

                // ABORT
                binWriter.Write((ushort)brk);

                // NMI
                binWriter.Write((ushort)nmi);

                // RESET
                binWriter.Write((ushort)reset);

                // IRQ
                binWriter.Write((ushort)irq);
            }
        }
    }
}