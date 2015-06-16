using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    internal static class MCP23S17Config
    {
        // I/O config
        internal static byte BANK_OFF = 0x00; // addressing mode
        internal static byte BANK_ON = 0x80;
        internal static byte INT_MIRROR_ON = 0x40; // interupt mirror (INTa|INTb)
        internal static byte INT_MIRROR_OFF = 0x00;
        internal static byte SEQOP_OFF = 0x20; // incrementing address pointer
        internal static byte SEQOP_ON = 0x00;
        internal static byte DISSLW_ON = 0x10; // slew rate
        internal static byte DISSLW_OFF = 0x00;
        internal static byte HAEN_ON = 0x08; // hardware addressing
        internal static byte HAEN_OFF = 0x00;
        internal static byte ODR_ON = 0x04; // open drain for interupts
        internal static byte ODR_OFF = 0x00;
        internal static byte INTPOL_HIGH = 0x02; // interupt polarity
        internal static byte INTPOL_LOW = 0x00;

    }
}
