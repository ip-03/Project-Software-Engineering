using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace WeatherApp
{
    public class Main : DbContext
    {
        public DbSet<device> device { get; set; } // represent the tables
        public DbSet<gateway> gateway { get; set; }
        public DbSet<sensor_data> sensor_data { get; set; }
        public DbSet<battery_status> battery_status { get; set; }

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

            // Set the chart data context
            DataContext = this;
        }

        private void DisplayButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDevice = (deviceComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); 
                var selectedSensorType = (sensorTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var selectedDate = datePicker.SelectedDate ?? DateTime.Today;


                var sensorData = Main.sensor_data // retrieve the sensor data
                    .Where(sensor => sensor.device_id == selectedDevice && sensor.date_time.Date == selectedDate)
                    .OrderBy(sensor => sensor.date_time);
                    

                ChartSeriesCollection.Clear(); // clears anything from the chart

                LineSeries series = new LineSeries { Title = selectedSensorType }; // create a line series graph on selected sensor type

                switch (selectedSensorType)
                {
                    case "Temperature In":
                        series.Values = new ChartValues<double>(sensorData.Select(sensor => sensor.temperature_in ?? 0)); // check if all values are null if some are change to 0;
                        break;
                    case "Temperature Out":
                        series.Values = new ChartValues<double>(sensorData.Select(sensor => sensor.temperature_out));
                        break;
                    case "Humidity":
                        series.Values = new ChartValues<double>(sensorData.Select(sensor => sensor.humidity));
                        break;
                    case "Ambient Light":
                        series.Values = new ChartValues<double>(sensorData.Select(sensor => sensor.ambient_light));
                        break;
                    default:
                        break;
                }


                ChartSeriesCollection.Add(series); // adds the line series
            }
            catch (Exception ex)
            {
               
                // resultTextBox.AppendText($"An error occurred: {ex.Message}\n");
            }
        }


        private void UpdateDisplay(object sender, SelectionChangedEventArgs e)
        {
            // Call the display button to update the graph if another selected item was chosen
            DisplayButton(sender, e);
        }

        private void ChartLoading(object sender, RoutedEventArgs e)
        {

        }
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hi", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }



    public class device

    {
        [Key]
        public string device_id { get; set; }
        public string gateway_id { get; set; }

    }


    public class gateway
    {
        [Key]
        public string gateway_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
        public double rssi { get; set; }
        public double snr { get; set; }

        public double avg_airtime { get; set; }

    }


    public class sensor_data
    {
        [Key]
        public int data_id { get; set; }
        public string device_id { get; set; }
        public double? temperature_in { get; set; }  // Change to double
        public double temperature_out { get; set; }  // Change to double

        public double humidity { get; set; }  // Change to double
        public double ambient_light { get; set; }  // Change to double
        public double? barometric_pressure { get; set; }  // Change to double
        public DateTime date_time { get; set; }
    }


    public class battery_status
    {
        [Key]
        public string device_id { get; set; }
        public int yo { get; set; } // change battery_status in the database as it wont work in c#
        public double BatV { get; set; }


    }



}


