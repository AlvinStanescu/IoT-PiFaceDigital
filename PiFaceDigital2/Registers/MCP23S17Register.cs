using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// MCP23S17 register class.
    /// </summary>
    public class MCP23S17Register : MCP23S17RegisterBase
    {
        public MCP23S17RegisterNibble LowerNibble { get; protected set; }
        public MCP23S17RegisterNibble UpperNibble { get; protected set; }
        public MCP23S17RegisterBit[] Bits { get; protected set; }

        /// <summary>
        /// Creates a new instance of the MCP23S17Register class.
        /// </summary>
        /// <param name="address">The address of the register.</param>
        /// <param name="chip">The MCP23S17 chip which contains the register.</param>
        internal MCP23S17Register(byte address, MCP23S17 chip) : base(address, chip)
        {
            this.LowerNibble = new MCP23S17RegisterNibble(Nibble.Lower, address, chip);
            this.UpperNibble = new MCP23S17RegisterNibble(Nibble.Upper, address, chip);
            this.Bits = new MCP23S17RegisterBit[8];
            
            for (var i = 0; i < this.Bits.Length; i++)
            {
                this.Bits[i] = new MCP23S17RegisterBit(i, address, chip);
            }
        }
    }
}
