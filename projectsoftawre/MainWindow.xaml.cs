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
    /// <summary>
    /// Represents the main window of the WeatherApp.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets or sets the database context for the application.
        /// </summary>
        private Main Main;

        /// <summary>
        /// Gets or sets the series collection for the charts.
        /// </summary>
        public SeriesCollection ChartSeriesCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Main = new Main();

            // Initialize the graph properties
            ChartSeriesCollection = new SeriesCollection();
            DataContext = this;

            datePicker.SelectedDate = DateTime.Today;
        }

        /// <summary>
        /// Gets a list of selected devices based on checkbox states.
        /// </summary>
        /// <returns>List of selected device IDs.</returns>
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

        /// <summary>
        /// Handles the display button click event to show sensor data on the chart.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
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

                        var deviceSensorData = sensorData
                            .Where(sensor => sensor.device_id == selectedDevice && sensor.date_time.Date == selectedDate)
                            .OrderBy(sensor => sensor.date_time);

                        string unitLabel = "";

                        switch (selectedSensorType)
                        {
                            case "Temperature In":
                                break;
                            case "Temperature Out":
                                break;
                            case "Humidity":
                                break;
                            case "Ambient Light":
                                break;
                            case "Barometric Pressure":
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

        /// <summary>
        /// Handles the chart loading event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChartLoading(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Displays a help message when the help button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void HelpButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Select the appropriate city to display the information of the set location. Information to be displayed as well can be changed accordingly. To proceed " +
                "it is necessary to select at least one city and a date.", "Help");
        }

        /// <summary>
        /// Opens a new window to display more sensor data when the sensor data button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
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
