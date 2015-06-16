using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.UI.Xaml.Controls;

namespace PiFace
{
    public class MCP23S17 : IDisposable
    {
        internal MCP23S17Register IODIRA { get; private set; }
        internal MCP23S17Register IODIRB { get; private set; }
        internal MCP23S17Register IPOLA { get; private set; }
        internal MCP23S17Register IPOLB { get; private set; }
        internal MCP23S17Register GPINTENA { get; private set; }
        internal MCP23S17Register GPINTENB { get; private set; }
        internal MCP23S17Register DEFVALA { get; private set; }
        internal MCP23S17Register DEFVALB { get; private set; }
        internal MCP23S17Register INTCONA { get; private set; }
        internal MCP23S17Register INTCONB { get; private set; }
        internal MCP23S17Register IOCON { get; private set; }
        internal MCP23S17Register GPPUA { get; private set; }
        internal MCP23S17Register GPPUB { get; private set; }
        internal MCP23S17Register INTFA { get; private set; }
        internal MCP23S17Register INTFB { get; private set; }
        internal MCP23S17Register INTCAPA { get; private set; }
        internal MCP23S17Register INTCAPB { get; private set; }
        internal MCP23S17Register GPIOA { get; private set; }
        internal MCP23S17Register GPIOB { get; private set; }
        internal MCP23S17Register OLATA { get; private set; }
        internal MCP23S17Register OLATB { get; private set; }

        private const int SpiBusSpeed = 10000000;
        private const string UninitializedDeviceInformationMessage = "The SPI device information cannot be null.";
        private const string FailedDeviceInitializationMessage = "The SPI device could not be initialized. Please verify that it is not used anywhere else, as we use Exclusive SharingMode, and verify that it is being disposed of properly.";

        private readonly DeviceInformation _spiDeviceInformation;
        private readonly byte _chipSelectLevel;

        private SpiDevice _spiDevice;
        private byte _hardwareAddress;
        private byte[] readBuffer = new byte[3];
        private byte[] writeBuffer = new byte[3];

        /// <summary>
        /// Create a new instance of the MCP23S17 class, used to communicate with a MCP23S17 controller via SPI.
        /// </summary>
        /// <param name="spiDeviceInformation">The SPI device information.</param>
        /// <param name="chipSelectLevel">The chip select line level to use.</param>
        public MCP23S17(DeviceInformation spiDeviceInformation, byte chipSelectLevel = 0)
        {
            if (spiDeviceInformation == null)
            {
                throw new ArgumentNullException(nameof(spiDeviceInformation), UninitializedDeviceInformationMessage);
            }

            this._chipSelectLevel = chipSelectLevel;
            this._spiDeviceInformation = spiDeviceInformation;

            this.IODIRA = new MCP23S17Register(MCP23S17RegisterAddresses.IODIRA, this);
            this.IODIRB = new MCP23S17Register(MCP23S17RegisterAddresses.IODIRB, this);
            this.IPOLA = new MCP23S17Register(MCP23S17RegisterAddresses.IPOLA, this);
            this.IPOLB = new MCP23S17Register(MCP23S17RegisterAddresses.IPOLB, this);
            this.GPINTENA = new MCP23S17Register(MCP23S17RegisterAddresses.GPINTENA, this);
            this.GPINTENB = new MCP23S17Register(MCP23S17RegisterAddresses.GPINTENB, this);
            this.DEFVALA = new MCP23S17Register(MCP23S17RegisterAddresses.DEFVALA, this);
            this.DEFVALB = new MCP23S17Register(MCP23S17RegisterAddresses.DEFVALB, this);
            this.INTCONA = new MCP23S17Register(MCP23S17RegisterAddresses.INTCONA, this);
            this.INTCONB = new MCP23S17Register(MCP23S17RegisterAddresses.INTCONB, this);
            this.IOCON = new MCP23S17Register(MCP23S17RegisterAddresses.IOCON, this);
            this.GPPUA = new MCP23S17Register(MCP23S17RegisterAddresses.GPPUA, this);
            this.GPPUB = new MCP23S17Register(MCP23S17RegisterAddresses.GPPUB, this);
            this.INTFA = new MCP23S17Register(MCP23S17RegisterAddresses.INTFA, this);
            this.INTFB = new MCP23S17Register(MCP23S17RegisterAddresses.INTFB, this);
            this.INTCAPA = new MCP23S17Register(MCP23S17RegisterAddresses.INTCAPA, this);
            this.INTCAPB = new MCP23S17Register(MCP23S17RegisterAddresses.INTCAPB, this);
            this.GPIOA = new MCP23S17Register(MCP23S17RegisterAddresses.GPIOA, this);
            this.GPIOB = new MCP23S17Register(MCP23S17RegisterAddresses.GPIOB, this);
            this.OLATA = new MCP23S17Register(MCP23S17RegisterAddresses.OLATA, this);
            this.OLATB = new MCP23S17Register(MCP23S17RegisterAddresses.OLATB, this);
        }

        /// <summary>
        /// Reads a byte from the MCP23S17 via SPI.
        /// </summary>
        /// <param name="address">The address to read from.</param>
        /// <returns>The read value.</returns>
        public byte Read(byte address)
        {
            var controlByte = this.GetSpiControlByte(this._spiDevice, MCP23S17Command.Read);
            writeBuffer[0] = controlByte;
            writeBuffer[1] = address;
            writeBuffer[2] = 0;
            this._spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
            return readBuffer[2]; // data
        }

        /// <summary>
        /// Writes to the MCP23S17 via SPI.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="address">The address to write to.</param>
        public void Write(byte value, byte address)
        {
            var controlByte = this.GetSpiControlByte(this._spiDevice, MCP23S17Command.Write);
            writeBuffer[0] = controlByte;
            writeBuffer[1] = address;
            writeBuffer[2] = value;
            this._spiDevice.Write(writeBuffer);
        }

        public async Task Init()
        {
            var spiSettings = new SpiConnectionSettings(this._chipSelectLevel)
            {
                ClockFrequency = SpiBusSpeed
            };

            try
            {
                this._spiDevice = await SpiDevice.FromIdAsync(this._spiDeviceInformation.Id, spiSettings);
            }
            catch (Exception)
            {
            }

            if (this._spiDevice == null)
            {
                throw new SpiDeviceException(FailedDeviceInitializationMessage);
            }

            var match = Regex.Match(this._spiDevice.DeviceId, @"SPI(\d)");
            this._hardwareAddress = Byte.Parse(match.Groups[1].Value);
        }

        /// <summary>
        /// Returns an SPI control byte.
        /// The MCP23S17 is a slave SPI device.The slave address contains four fixed bits and three user - defined hardware address bits 
        /// (if enabled via IOCON.HAEN)(pins A2, A1 and A0) with the read / write bit filling out the control byte:
        ///      + --------------------+
        ///      | 0 | 1 | 0 | 0 | A2 | A1 | A0 | R / W |
        ///      +--------------------+
        ///        7 6 5 4 3  2  1   0
        /// </summary>
        /// <param name="device">The SPI device.</param>
        /// <param name="command">The command to issue.</param>
        /// <returns>The SPI control byte.</returns>
        private byte GetSpiControlByte(SpiDevice device, MCP23S17Command command)
        {
            var boardAddressPattern = (this._hardwareAddress << 1) & 0xE;
            return (byte)(0x40 | boardAddressPattern | ((byte)command));
        }

        /// <summary>
        /// Private Read/Write command enumeration.
        /// </summary>
        private enum MCP23S17Command
        {
            Write = 0,
            Read = 1
        }

        /// <summary>
        /// Disposes of the MCP23S17 object.
        /// </summary>
        public void Dispose()
        {
            if (this._spiDevice != null)
            {
                _spiDevice.Dispose();
            }
        }
    }

    public class SpiDeviceException : Exception
    {
        public SpiDeviceException(string failedDeviceInitializationMessage)
        {
            this.Message = failedDeviceInitializationMessage;
        }

        public override string Message { get; }
    }
}
