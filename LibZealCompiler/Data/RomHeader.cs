namespace Zeal.Compiler.Data
{
    public enum RomSpeed
    {
        SlowROM,
        FastROM
    }

    public enum MapMode
    {
        LoROM,
        HiROM
    }

    public enum Country
    {
        Japan,
        NorthAmerica,
        Europe,
        Sweden,
        Finland,
        Denmark,
        France,
        Holland,
        Spain,
        Germany,
        Italy,
        China,
        Indonesia,
        SouthKorea,
        Common,
        Canada,
        Brazil,
        Australia
    }

    public class RomHeader
    {
        public string CartridgeName { get; set; }
        public RomSpeed RomSpeed { get; set; }
        public MapMode MapMode { get; set; }
        public uint SramSize { get; set; }
        public Country Country { get; set; }
        public uint Developer { get; set; }
        public uint Version { get; set; }
    }
}
