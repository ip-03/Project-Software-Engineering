using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WeatherApp
{
    public partial class MoreDataWindow : Window
    {
        public MoreDataWindow(List<string> selectedDevices)
        {
            InitializeComponent();

            using (var main = new Main())
            {
                var data = main.device
                    .Where(device => selectedDevices.Contains(device.device_id))
                    .Join(
                        main.gateway,
                        device => device.gateway_id,
                        gateway => gateway.gateway_id,
                        (device, gateway) => new { Device = device, Gateway = gateway }
                    )
                    .ToList();

                if (data.Any())
                {
                    // Set up DataGrid columns
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Device", Binding = new System.Windows.Data.Binding("Device.device_id") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Gateway", Binding = new System.Windows.Data.Binding("Gateway.gateway_id") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Battery Status", Binding = new System.Windows.Data.Binding("Device.battery_status") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Battery Voltage", Binding = new System.Windows.Data.Binding("Device.BatV") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Device Latitude", Binding = new System.Windows.Data.Binding("Gateway.latitude") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Device Longitude", Binding = new System.Windows.Data.Binding("Gateway.longitude") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Device Altitude", Binding = new System.Windows.Data.Binding("Gateway.altitude") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Average RSSI", Binding = new System.Windows.Data.Binding("Gateway.avg_rssi") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Average SNR", Binding = new System.Windows.Data.Binding("Gateway.avg_snr") });
                    dataGrid.Columns.Add(new DataGridTextColumn { Header = "Average Airtime", Binding = new System.Windows.Data.Binding("Gateway.avg_airtime") });

                    dataGrid.ItemsSource = data;
                }
            }
        }
    }
}
