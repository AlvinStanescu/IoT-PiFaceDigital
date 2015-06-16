using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// An MCP23S17 register nibble.
    /// </summary>
    public class MCP23S17RegisterNibble : MCP23S17RegisterBase
    {
        public MCP23S17RegisterBit[] Bits { get; protected set; }

        private readonly Nibble _nibble;

        internal MCP23S17RegisterNibble(Nibble nibble, byte address, MCP23S17 chip) : base(address, chip)
        {
            this._nibble = nibble;
            this.Bits = new MCP23S17RegisterBit[4];

            var rangeStart = nibble.GetRangeStart();
            var rangeEnd = nibble.GetRangeEnd();

            for (var i = rangeStart; i < rangeEnd; i++)
            {
                this.Bits[i - rangeStart] = new MCP23S17RegisterBit(i, address, chip);
            }
        }

        /// <summary>
        /// The value of the register entity.
        /// </summary>
        public override byte Value
        {
            get
            {
                var readByte = (byte)(this._chip.Read(this.Address) & this._nibble.GetShiftValue());

                // Used to shift back the value into the correct place (if in bytes 7-4, shift them to 3-0)
                var shiftBackValue = ((byte) this._nibble >> 2);

                return (byte)(readByte >> shiftBackValue);
            }
            set
            {
                var registerValue = this._chip.Read(this.Address);
                this._nibble.SetByteNibble(value, ref registerValue);
                this._chip.Write(registerValue, this.Address);
            }
        }

        /// <summary>
        /// Toggles all bits.
        /// </summary>
        public override void Toggle()
        {
            this.Value = (byte)(0xF ^ this.Value);
        }

        /// <summary>
        /// Sets all bits.
        /// </summary>
        public override void SetAll()
        {
            this.Value = 0xF;
        }

        /// <summary>
        /// Clears all bits.
        /// </summary>
        public override void ClearAll()
        {
            this.Value = 0x0;
        }
    }
}
