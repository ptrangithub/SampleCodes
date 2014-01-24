using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FTD2XX_NET;

namespace ListVIDPID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The <see cref="VIDPIDList" /> dependency property's name.
        /// </summary>
        public const string VIDPIDListPropertyName = "VIDPIDList";

        /// <summary>
        /// Gets or sets the value of the <see cref="VIDPIDList" />
        /// property. This is a dependency property.
        /// </summary>
        public string VIDPIDList
        {
            get
            {
                return (string)GetValue(VIDPIDListProperty);
            }
            set
            {
                SetValue(VIDPIDListProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="VIDPIDList" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty VIDPIDListProperty = DependencyProperty.Register(
            VIDPIDListPropertyName,
            typeof(string),
            typeof(MainWindow),
            new UIPropertyMetadata(string.Empty));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            FTD2XX_NET.FTDI dev = new FTDI();
            UInt32 numDevices = 0;
            dev.GetNumberOfDevices(ref numDevices);

            var pDest = new FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[numDevices];
            dev.GetDeviceList(pDest);

            VIDPIDList = string.Empty;
            for (int i = 0; i < numDevices; i++ )
            {
                VIDPIDList += string.Format("{0} \n", pDest[i].ID);
            }
        }
    }
}
