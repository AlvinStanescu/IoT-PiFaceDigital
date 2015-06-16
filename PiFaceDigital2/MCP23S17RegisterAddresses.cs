using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    internal class MCP23S17RegisterAddresses
    {
        // Register addresses
        internal static byte IODIRA = 0x0; // I/O direction A
        internal static byte IODIRB = 0x1; // I/O direction B
        internal static byte IPOLA = 0x2; // I/O polarity A
        internal static byte IPOLB = 0x3; // I/O polarity B
        internal static byte GPINTENA = 0x4; // interrupt enable A
        internal static byte GPINTENB = 0x5; // interrupt enable B
        internal static byte DEFVALA = 0x6; // register default value A (interrupts)
        internal static byte DEFVALB = 0x7; // register default value B (interrupts)
        internal static byte INTCONA = 0x8; // interupt control A
        internal static byte INTCONB = 0x9; // interupt control B
        internal static byte IOCON = 0xA; // I/O config (also 0xB)
        internal static byte GPPUA = 0xC; // port A pullups
        internal static byte GPPUB = 0xD; // port B pullups
        internal static byte INTFA = 0xE; // interrupt flag A (where the interrupt came from)
        internal static byte INTFB = 0xF; // interrupt flag B
        internal static byte INTCAPA = 0x10; // interrupt capture A (value at interrupt is saved here)
        internal static byte INTCAPB = 0x11; // interrupt capture B
        internal static byte GPIOA = 0x12; // port A
        internal static byte GPIOB = 0x13; // port B
        internal static byte OLATA = 0x14; // output latch A
        internal static byte OLATB = 0x15; // output latch B
    }
}
