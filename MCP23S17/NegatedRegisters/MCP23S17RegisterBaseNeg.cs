using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// Base class for MCP23S17 negated register entities.
    /// </summary>
    public class MCP23S17RegisterBaseNeg : MCP23S17RegisterBase
    {
        /// <summary>
        /// Creates a new instance of the MCP23S17RegisterBaseNeg class.
        /// </summary>
        /// <param name="address">The address of the entity.</param>
        /// <param name="chip">The MCP23S17 chip which contains the register.</param>
        internal  MCP23S17RegisterBaseNeg(byte address, MCP23S17 chip) : base(address, chip)
        {
        }

        /// <summary>
        /// The value of the register entity.
        /// </summary>
        public override byte Value
        {
            get
            {
                return (byte)(0xFF ^ this._chip.Read(this.Address));
            }
            set
            {
                this._chip.Write((byte)(value ^ 0xFF), this.Address);
            }
        }       
    }
}
