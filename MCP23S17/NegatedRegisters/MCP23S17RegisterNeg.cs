using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// MCP23S17 negated register class.
    /// </summary>
    public class MCP23S17RegisterNeg : MCP23S17RegisterBaseNeg
    {
        public MCP23S17RegisterNibbleNeg LowerNibble { get; protected set; }
        public MCP23S17RegisterNibbleNeg UpperNibble { get; protected set; }
        public MCP23S17RegisterBitNeg[] Bits { get; protected set; }

        /// <summary>
        /// Creates a new instance of the MCP23S17Register class.
        /// </summary>
        /// <param name="address">The address of the register.</param>
        /// <param name="chip">The MCP23S17 chip which contains the register.</param>
        internal MCP23S17RegisterNeg(byte address, MCP23S17 chip) : base(address, chip)
        {
            this.LowerNibble = new MCP23S17RegisterNibbleNeg(Nibble.Lower, address, chip);
            this.UpperNibble = new MCP23S17RegisterNibbleNeg(Nibble.Upper, address, chip);
            this.Bits = new MCP23S17RegisterBitNeg[8];

            for (var i = 0; i < this.Bits.Length; i++)
            {
                this.Bits[i] = new MCP23S17RegisterBitNeg(i, address, chip);
            }
        }
    }
}
