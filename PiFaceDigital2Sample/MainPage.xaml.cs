using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
        private PiFaceDigital piFaceDigital;

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeSystem();
        }

        public async void InitializeSystem()
        {
            try
            {
                var deviceSelector = SpiDevice.GetDeviceSelector();
                var spiControllers = await DeviceInformation.FindAllAsync(deviceSelector);

                piFaceDigital = new PiFaceDigital(spiControllers[0]);
                await piFaceDigital.Init();
                this.TitleText.Text = "Use the outputs to control the LEDs.";
                piFaceDigital?.LEDs[7].SetAll();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
                piFaceDigital?.Dispose();
                this.TitleText.Text = "Initialization failed.";
                piFaceDigital = null;
            }
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var toggleSwitch = (ToggleSwitch) e.OriginalSource;
            var headerText = toggleSwitch.Header.ToString();
            var portNumber = Byte.Parse((Regex.Match(headerText, "\\d").Value));

            if (toggleSwitch.IsOn)
            {
                piFaceDigital?.LEDs[portNumber].SetAll();
            }
            else
            {
                piFaceDigital?.LEDs[portNumber].ClearAll();
            }
        }
    }
}
