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
        public GraphWindow()
        {
            InitializeComponent();
        }
        private void ChartLoading(object sender, RoutedEventArgs e)
        {

        }

        private void MoreDataButton(object sender, RoutedEventArgs e)
        {


            MoreDataWindow moreDataWindow = new MoreDataWindow(); 

            
            moreDataWindow.DataContext = this;

            
            moreDataWindow.Show();

        }
    }
}
