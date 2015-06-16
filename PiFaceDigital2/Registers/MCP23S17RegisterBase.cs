using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// Base class for MCP23S17 register entities.
    /// </summary>
    public class MCP23S17RegisterBase
    {
        /// <summary>
        /// The address of the entity.
        /// </summary>
        public byte Address { get; protected set; }

        /// <summary>
        /// The MCP23S17 chip which contains the register.
        /// </summary>
        protected readonly MCP23S17 _chip;

        /// <summary>
        /// Creates a new instance of the MCP23S17RegisterBase class.
        /// </summary>
        /// <param name="address">The address of the entity.</param>
        /// <param name="chip">The MCP23S17 chip which contains the register.</param>
        internal MCP23S17RegisterBase(byte address, MCP23S17 chip)
        {
            this.Address = address;
            this._chip = chip;
        }

        /// <summary>
        /// The value of the register entity.
        /// </summary>
        public virtual byte Value
        {
            get
            {
                return this._chip.Read(this.Address);
            }
            set
            {
                this._chip.Write(value, this.Address);
            }
        }

        /// <summary>
        /// Toggles all bits.
        /// </summary>
        public virtual void Toggle()
        {
            this.Value = (byte)(0xFF ^ this.Value);
        }

        /// <summary>
        /// Sets all bits.
        /// </summary>
        public virtual void SetAll()
        {
            this.Value = 0xFF;
        }

        /// <summary>
        /// Clears all bits.
        /// </summary>
        public virtual void ClearAll()
        {
            this.Value = 0x00;
        }
    }
}
