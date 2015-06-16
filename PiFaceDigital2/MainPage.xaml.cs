using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PiFace;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PiFaceDigital2Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeSystem();
        }

        public async void InitializeSystem()
        {
            PiFaceDigital piFaceDigital = null;

            try
            {
                var deviceSelector = SpiDevice.GetDeviceSelector();
                var spiControllers = await DeviceInformation.FindAllAsync(deviceSelector);

                piFaceDigital = new PiFaceDigital(spiControllers[0]);
                await piFaceDigital.Init();
                piFaceDigital.LEDs[0].SetAllBits();
                piFaceDigital.LEDs[7].SetAllBits();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                piFaceDigital?.Dispose();
            }
        }
    }
}
