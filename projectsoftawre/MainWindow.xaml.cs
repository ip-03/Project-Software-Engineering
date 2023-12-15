using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace WeatherApp
{
    public class Main : DbContext
    {
        public DbSet<device> device { get; set; } // represent the tables
        public DbSet<gateway> gateway { get; set; }
        public DbSet<sensor_data> sensor_data { get; set; }
        public DbSet<gateway_metadata> gateway_metadata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=projectgroup5saxion.database.windows.net;Initial Catalog=project_software_engineering;Persist Security Info=False;User ID=project_group5_saxion;Password=weatherstation5!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString); // connect to the database
        }
    }

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

                GraphWindow graphWindow = new GraphWindow();

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
                }
            }
            catch (Exception ex)
            {

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
            MessageBox.Show("No Help Currently", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      

    }


}


