using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiFace
{
    /// <summary>
    /// The nibble of a byte. 
    /// The high nibble contains the bits from 7 to 4, the low nibble contains the bits from 3 to 0.
    /// </summary>
    internal enum Nibble
    {
        Lower = 0,
        Upper = 1
    }

    /// <summary>
    /// Extensions to ease the use of nibbles.
    /// </summary>
    internal static class NibbleExtensions
    {
        /// <summary>
        /// Gets the range start from the nibble.
        /// 0 (Nibble.Lower) => 0
        /// 1 (Nibble.Upper) => 4
        /// </summary>
        /// <param name="nibble">The nibble.</param>
        /// <returns>The beginning of the range, with a length of 4.</returns>

        internal static int GetRangeStart(this Nibble nibble)
        {
            return (int)nibble << 2;
        }

        /// <summary>
        /// Gets the range start from the nibble.
        /// 0 (Nibble.Lower) => 4
        /// 1 (Nibble.Upper) => 8
        /// </summary>
        /// <param name="nibble">The nibble.</param>
        /// <returns>The beginning of the range, with a length of 4.</returns>

        internal static int GetRangeEnd(this Nibble nibble)
        {
            return (int)(++nibble) << 2;
        }

        /// <summary>
        /// Gets the shift left value needed to access the nibble.
        /// </summary>
        /// <param name="nibble">The nibble.</param>
        /// <returns>The beginning of the range, with a length of 4.</returns>

        internal static byte GetShiftValue(this Nibble nibble)
        {
            return (byte)(0x0F << ((byte)(nibble) << 4));
        }

        /// <summary>
        /// Sets the part of a byte corresponding to the nibble and returns the byte.
        /// </summary>
        /// <param name="nibble">The nibble.</param>
        /// <param name="value">The value of the nibble (4-bit).</param>
        /// <param name="byteValue">The value of the byte on which to set the nibble.</param>
        internal static void SetByteNibble(this Nibble nibble, byte value, ref byte byteValue)
        {
            var byteWithClearedNibble = (byte) (nibble == Nibble.Lower ? byteValue & 0xF0 : byteValue & 0x0F);
            byteValue ^= (byte)(value & GetShiftValue(nibble));
        }

        /// <summary>
        /// Sets the part of a negated byte corresponding to the nibble and returns the negated byte.
        /// </summary>
        /// <param name="nibble">The nibble.</param>
        /// <param name="value">The value of the nibble (4-bit).</param>
        /// <param name="byteValue">The value of the negated byte on which to set the nibble.</param>
        internal static void SetByteNibbleNeg(this Nibble nibble, byte value, ref byte byteValue)
        {
            var byteWithClearedNibble = (byte)(nibble == Nibble.Lower ? byteValue & 0xF0 : byteValue & 0x0F);
            byteValue ^= (byte)(value & GetShiftValue(nibble) ^ (0xF << ((int)nibble) << 2));
        }
    }
}
