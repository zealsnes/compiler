using System;
using Zeal.Compiler.Data;

namespace Zeal.Compiler.Helper
{
    public static class CpuAddressConverter
    {
        public static int RAMToPhysical(int address, MapMode mode)
        {
            switch(mode)
            {
                case MapMode.LoROM:
                    return ((address & 0x7F0000) >> 1) | (address & 0x7FFF);
                case MapMode.HiROM:
                    return address & 0x3FFFFF;
            }

            return address;
        }

        public static int PhysicalToRAM(int address, MapMode mode, RomSpeed romSpeed)
        {
            int bank = 0;
            switch(mode)
            {
                case MapMode.LoROM:
                    {
                        switch (romSpeed)
                        {
                            case RomSpeed.SlowROM:
                                bank = 0x00;
                                break;
                            case RomSpeed.FastROM:
                                bank = 0x80;
                                break;
                        }

                        int result = (bank << 16)
                            | ((address & 0xFF8000) << 1)
                            | (address & 0xFFFF)
                            | 0x8000
                            ;

                        return result;
                    }
                case MapMode.HiROM:
                    {
                        switch (romSpeed)
                        {
                            case RomSpeed.SlowROM:
                            case RomSpeed.FastROM:
                                bank = 0xC0;
                                break;
                        }

                        return (bank << 16) | (address & 0xFFFFFF);
                    }
            }

            return address;
        }
    }
}
