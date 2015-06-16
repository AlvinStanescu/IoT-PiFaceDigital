using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace PiFace
{
    public class PiFaceDigital : IDisposable
    {
        /// <summary>
        /// The input pins of the PiFaceDigital.
        /// </summary>
        public MCP23S17RegisterBitNeg[] InputPins { get; private set; }

        /// <summary>
        /// The input port of the PiFaceDigital, containing 8 separate pins.
        /// </summary>
        public MCP23S17RegisterNeg InputPort { get; private set; }

        /// <summary>
        /// The output pins of the PiFaceDigital.
        /// </summary>
        public MCP23S17RegisterBit[] OutputPins { get; private set; }

        /// <summary>
        /// The output port of the PiFaceDigital, containing 8 separate pins.
        /// </summary>
        public MCP23S17Register OutputPort { get; private set; }

        /// <summary>
        /// The LEDs of the PiFaceDigital.
        /// </summary>
        public MCP23S17RegisterBit[] LEDs { get; private set; }

        /// <summary>
        /// The relays of the PiFaceDigital.
        /// </summary>
        public MCP23S17RegisterBit[] Relays { get; private set; }

        /// <summary>
        /// The switches of the PiFaceDigital.
        /// </summary>
        public MCP23S17RegisterBitNeg[] Switches { get; private set; }

        private readonly MCP23S17 _chip;

        /// <summary>
        /// Creates a new instance of the PiFaceDigital class, used to communicate with a PiFaceDigital or PiFaceDigital2 extension board.
        /// </summary>
        /// <param name="spiDeviceInformation">The SPI device information.</param>
        /// <param name="chipSelectLevel">The chip select line level to use.</param>
        public PiFaceDigital(DeviceInformation spiDeviceInformation, byte chipSelectLevel = 0)
        {
            this._chip = new MCP23S17(spiDeviceInformation, chipSelectLevel);

            this.InputPort = new MCP23S17RegisterNeg(MCP23S17RegisterAddresses.GPIOB, this._chip);
            this.InputPins = new MCP23S17RegisterBitNeg[8];

            this.OutputPort = new MCP23S17Register(MCP23S17RegisterAddresses.GPIOA, this._chip);
            this.OutputPins = new MCP23S17RegisterBit[8];
            this.LEDs = new MCP23S17RegisterBit[8];

            this.Relays = new MCP23S17RegisterBit[2];
            this.Switches = new MCP23S17RegisterBitNeg[4];

            for (var i = 0; i < 8; i++)
            {
                if (i < 2)
                {
                    this.Relays[i] = new MCP23S17RegisterBit(i, MCP23S17RegisterAddresses.GPIOA, this._chip);
                }

                if (i < 4)
                {
                    this.Switches[i] = new MCP23S17RegisterBitNeg(i, MCP23S17RegisterAddresses.GPIOB, this._chip);
                }

                this.InputPins[i] = new MCP23S17RegisterBitNeg(i, MCP23S17RegisterAddresses.GPIOB, this._chip);
                this.OutputPins[i] = new MCP23S17RegisterBit(i, MCP23S17RegisterAddresses.GPIOB, this._chip);
                this.LEDs[i] = new MCP23S17RegisterBit(i, MCP23S17RegisterAddresses.GPIOA, this._chip);
            }
        }

        /// <summary>
        /// Initializes communication with the PiFaceDigital2 board.
        /// </summary>
        /// <param name="initBoard">A flag indicating whether to also initialize the board.</param>
        public async Task Init(bool initBoard = true)
        {
            await this._chip.Init();
            if (initBoard)
            {
                this.InitBoard();
            }
        }

        /// <summary>
        /// Initializes the PiFaceDigital board.
        /// </summary>
        private void InitBoard()
        {
            var ioConfig = (byte)(MCP23S17Config.BANK_OFF |
                           MCP23S17Config.INT_MIRROR_OFF |
                           MCP23S17Config.SEQOP_OFF |
                           MCP23S17Config.DISSLW_OFF |
                           MCP23S17Config.HAEN_ON |
                           MCP23S17Config.ODR_OFF |
                           MCP23S17Config.INTPOL_LOW);
            this._chip.IOCON.Value = ioConfig;

            if (this._chip.IOCON.Value != ioConfig)
            {
                throw new InvalidOperationException("Cannot initialize a board which does not exist. Please check that the provided SpiDevice exists.");
            }

            this._chip.GPIOA.Value = 0;
            this._chip.IODIRA.Value = 0; // GPIOA as outputs
            this._chip.GPIOA.Value = 0;

            this._chip.IODIRB.Value = 0xFF; // GPIOB as inputs
            this._chip.GPPUB.Value = 0xFF; // enable GPIOB input pull-ups
            this.EnableInterrupts();
        }

        /// <summary>
        /// Deinitializes the board
        /// </summary>
        private void DeInitBoard()
        {
            this._chip.GPIOA.Value = 0;
            if (this._chip.GPINTENB.Value != 0x00)
            {
                this.DisableInterrupts();
            }
        }

        /// <summary>
        /// Enables all interrupts.
        /// </summary>
        public void EnableInterrupts()
        {
            //TODO: Implement   
        }

        /// <summary>
        /// Disables all interrupts.
        /// </summary>
        public void DisableInterrupts()
        {
            //TODO: Implement
        }

        public void Dispose()
        {
            if (this._chip != null)
            {
                DeInitBoard();
                this._chip.Dispose();
            }
        }
    }
}
