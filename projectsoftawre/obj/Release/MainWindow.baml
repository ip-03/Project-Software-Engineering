   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  $  Project-Software-Engineering-GUI   WeatherApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf       � Title$  Weather��$	��700��$	��450��.��� $��
20,20,20,4q� � ColumnDefinitions �� $
��Auto ��� $��* �.+��� $��0�� �� Orientation$ Vertical=�$��Center=�$��Center=�$	��321��.+��� $��
WeatherApp��$��24(�$
��Bold"�
������$��0 0 0 15q�$��Center=��� $��City Selection��$��0 5 0 0q�.���� $ 
Horizontal=�.+��� -   $��checkBoxSaxion��$��Saxion��$��5q��� -   $��checkBoxGronau��$��Gronau��$��5q��� -   $��checkBoxWierden��$��Wierden��$��5q��� -   $��checkBoxLocal��$��Local��$��5q��� $��Select weather option:��$��13(�$��20��$��0 10 0 0q�$��Center=��� $ 
Horizontal=�.+��� -    ��Name$ sensorTypeComboBox�� �� SelectedIndex$ 0��$��5q�.@��� $��Temperature In���� $��Temperature Out���� $��Humidity���� $��Ambient Light���� $��BarometricPressure��(  "System.Windows.Controls.DatePicker  -     Name$ 
datePicker��$��5q��� -    ��Name$ GetWeatherButton��$��Display��
�������
���   �$��34��$
��0 10q�$	��208���� -   $
��Help��$��29��
���   �
�������$	��186���� -	   $��Sensor Data��
���   �
�������$��1��$��271,342,10,10q�#   LiveCharts.Wpf.CartesianChart  -
   $��chart��$��14,20,-4,52q�$��1��&    LiveCharts.Wpf.Charts.Base.Chart   Series �� *ChartSeriesCollection+   AxisX    LiveCharts.Wpf.Axis  	   Title$
	 Time��
   Labels
 �� *Labels+   AxisY   $	 Value��   DataTooltip #   LiveCharts.Wpf.DefaultTooltip     SelectionMode$ SharedXValuesX�