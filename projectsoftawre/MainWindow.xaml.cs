using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using MessageBox = System.Windows.Forms.MessageBox;
namespace WeatherApp
{

    public partial class MainWindow : Window
    {
        private Main Main;

        public SeriesCollection ChartSeriesCollection { get; set; } // for the graphs

        public MainWindow() // constructor
        {
            InitializeComponent();
            Main = new Main();

            // Initialize the graph properties
            ChartSeriesCollection = new SeriesCollection();
            DataContext = this;

            
            datePicker.SelectedDate = DateTime.Today;

        }
        private List<string> GetSelectedDevices()
        {
            var selectedDevices = new List<string>();

            if (checkBoxSaxion.IsChecked == true)
                selectedDevices.Add("lht-saxion");
            if (checkBoxGronau.IsChecked == true)
                selectedDevices.Add("lht-gronau");
            if (checkBoxWierden.IsChecked == true)
                selectedDevices.Add("lht-wierden");
            if (checkBoxLocal.IsChecked == true)
                selectedDevices.Add("saxion-group5-2023");
            return selectedDevices;
        }

        private void DisplayButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDevices = GetSelectedDevices();

                if (selectedDevices.Count == 0 || datePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please make sure that at least one city or the date is selected! Select help for more information.", "Error");
                }
                else
                {

                  
                    var selectedSensorType = (sensorTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    var selectedDate = datePicker.SelectedDate ?? DateTime.Today;

                    var sensorData = Main.sensor_data
                        .Where(sensor => selectedDevices.Contains(sensor.device_id) && sensor.date_time.Date == selectedDate)
                        .OrderBy(sensor => sensor.date_time);

                    ChartSeriesCollection.Clear();



                    foreach (var selectedDevice in selectedDevices)
                    {
                        LineSeries series = new LineSeries
                        {
                            Title = $"{selectedDevice} - {selectedSensorType}",
                            Values = new ChartValues<double>()
                        };


                        string unitLabel = ""; // Variable to hold the unit label

                        switch (selectedSensorType)
                        {
                            case "Temperature In":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == "lht-saxion" || sensor.device_id == "saxion-group5-2023")
                                    .Select(sensor => sensor.temperature_in ?? 0));
                                unitLabel = "°C"; // Celsius
                                break;
                            case "Temperature Out":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.temperature_out));
                                unitLabel = "°C"; // Celsius
                                break;
                            case "Humidity":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.humidity));
                                unitLabel = "%"; // Percentage
                                break;
                            case "Ambient Light":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.ambient_light));
                                unitLabel = "lux"; // Lux 
                                break;
                            case "Barometric Pressure":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.barometric_pressure ?? 0));
                                unitLabel = "hPa"; // Hectopascal
                                break;
                            default:
                                break;
                        }


                        series.LabelPoint = point => $"{selectedDevice}: {point.Y} {unitLabel} \n Date: {selectedDate.Date}";


                        ChartSeriesCollection.Add(series);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void ChartLoading(object sender, RoutedEventArgs e)
        {

        }
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Select the appropriate city to display the information of the set location. Information to be displayed as well can be changed accordingly. To proceed " +
                "it is necessary to select at least one city and a date.", "Help");
        }
        private void SensorDataButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDevices = GetSelectedDevices();

                if (selectedDevices.Count > 0)
                {
                    MoreDataWindow moreDataWindow = new MoreDataWindow(selectedDevices);
                    moreDataWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select at least one city to view sensor data.", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }


}