using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WeatherApp
{

    public partial class MainWindow : Window
    {
        private Main Main;

        public SeriesCollection ChartSeriesCollection { get; set; } // for the graphs
        public string[] Labels { get; set; } // to create labels 

        public MainWindow() // constructor
        {
            InitializeComponent();
            Main = new Main();

            // Initialize the graph properties
            ChartSeriesCollection = new SeriesCollection();


        }

        private void DisplayButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkBoxSaxion.IsChecked == false && checkBoxGronau.IsChecked == false && checkBoxWierden.IsChecked == false || datePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please make sure that at least one city " +
                        "or the date are selected! Select help for more information.",
                        "Error");
                }
                else
                {

                    var selectedDevices = new List<string>();

                    if (checkBoxSaxion.IsChecked == true)
                        selectedDevices.Add("lht-saxion");
                    if (checkBoxGronau.IsChecked == true)
                        selectedDevices.Add("lht-gronau");
                    if (checkBoxWierden.IsChecked == true)
                        selectedDevices.Add("lht-wierden");

                    var selectedSensorType = (sensorTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    var selectedDate = datePicker.SelectedDate ?? DateTime.Today;

                    var sensorData = Main.sensor_data
                        .Where(sensor => selectedDevices.Contains(sensor.device_id) && sensor.date_time.Date == selectedDate)
                        .OrderBy(sensor => sensor.date_time);

                    ChartSeriesCollection.Clear();

                    GraphWindow graphWindow = new GraphWindow(selectedDevices);

                    // Set the DataContext for the GraphWindow
                    graphWindow.DataContext = this;

                    // Show the GraphWindow
                    graphWindow.Show();


                    foreach (var selectedDevice in selectedDevices)
                    {
                        LineSeries series = new LineSeries
                        {
                            Title = $"{selectedDevice} - {selectedSensorType}",
                            Values = new ChartValues<double>()
                        };


                        switch (selectedSensorType)
                        {
                            case "Temperature In":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == "lht-saxion")
                                    .Select(sensor => sensor.temperature_in ?? 0));
                                break;
                            case "Temperature Out":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.temperature_out));
                                break;
                            case "Humidity":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.humidity));
                                break;
                            case "Ambient Light":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.ambient_light));
                                break;
                            case "Barometric Pressure":
                                series.Values = new ChartValues<double>(sensorData
                                    .Where(sensor => sensor.device_id == selectedDevice)
                                    .Select(sensor => sensor.barometric_pressure ?? 0));
                                break;
                            default:
                                break;
                        }

                        series.LabelPoint = point => $"{selectedDevice}: {point.Y} \n Date: {selectedDate}";



                        ChartSeriesCollection.Add(series);
                        var moreDataWindow = new MoreDataWindow(selectedDevices);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void UpdateDisplay(object sender, SelectionChangedEventArgs e)
        {
            DisplayButton(sender, e);
        }


        private void ChartLoading(object sender, RoutedEventArgs e)
        {

        }
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Select the appropriate city to display the information of the set location. Information to be displayed as well can be changed accordingly. To proceed " +
                "it is necessary to select at least one city and a date.", "Help");
        }
    }


}

