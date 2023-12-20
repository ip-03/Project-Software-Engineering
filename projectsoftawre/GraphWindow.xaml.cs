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
using System.Windows.Shapes;

namespace WeatherApp
{
    public partial class GraphWindow : Window
    {
        private List<string> selectedDevices; // Declare selectedDevices as a field

        public GraphWindow(List<string> selectedDevices)
        {
            InitializeComponent();

            // Assign the selectedDevices passed to the constructor to the field
            this.selectedDevices = selectedDevices;
        }
        private void ChartLoading(object sender, RoutedEventArgs e)
        {

        }

        private void MoreDataButton(object sender, RoutedEventArgs e)
        {


            MoreDataWindow moreDataWindow = new MoreDataWindow(selectedDevices); 

            
            moreDataWindow.DataContext = this;

            
            moreDataWindow.Show();

        }
    }
}
