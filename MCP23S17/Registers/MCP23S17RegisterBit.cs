using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// A bit of an MCP23S17 register.
    /// </summary>
    public class MCP23S17RegisterBit : MCP23S17RegisterBase
    {
        private readonly int _bitOffset;

        /// <summary>
        /// Instantiate a new MCP23S17 register bit.
        /// </summary>
        /// <param name="bitOffset"></param>
        /// <param name="address"></param>
        /// <param name="chip"></param>
        internal MCP23S17RegisterBit(int bitOffset, byte address, MCP23S17 chip) : base(address, chip)
        {
            this._bitOffset = bitOffset;
        }

        /// <summary>
        /// The value of the register entity.
        /// </summary>
        public override byte Value
        {
            get
            {
                var readBit = (byte)(this._chip.Read(this.Address) & (0x01 << _bitOffset));

                // The read bit must be correctly shifted back to the first bit of the byte.
                return (byte)(readBit >> _bitOffset);
            }
            set
            {
                var registerValue = this._chip.Read(this.Address) & (~(0x01 << _bitOffset));
                this._chip.Write((byte)(registerValue | (value << _bitOffset)), this.Address);
            }
        }

        /// <summary>
        /// Toggles the bit.
        /// </summary>
        public override void Toggle()
        {
            this.Value = (byte)(0x1 ^ this.Value);
        }

        /// <summary>
        /// Sets the bit.
        /// </summary>
        public override void SetAllBits()
        {
            this.Value = 0x1;
        }

        /// <summary>
        /// Clears the bit.
        /// </summary>
        public override void ClearAllBits()
        {
            this.Value = 0x0;
        }
    }
}
