   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    projectsoftawre   WeatherApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  Weather��5   	   $	��300��6   $	��400��6%   .��5
      � $��
20,20,20,4q�6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��Center=�6,   $��Center=�6I   $	��321��6d   .+�5      �� $��
WeatherApp��6   $��24(�6*   $
��Bold"�68   
������$��0 0 0 15q�6b   $��Center=�6t   �� 5      $��City Selection��6   $��0 5 0 0q�6/   .��5      �� $ 
Horizontal=�6   .+�5      �� -   5        $��checkBoxSaxion��5      $��Saxion��65   $��5q�6F   �� 5      -   5        $��checkBoxGronau��5      $��Gronau��65   $��5q�6F   �� 5      -   5        $��checkBoxWierden��5      $��Wierden��66   $��5q�6H   5      5      �� 5      $��Select weather option:��6   $��13(�66   $��20��6D   $��0 10 0 0q�6P   $��Center=�6b   �� 5      $ 
Horizontal=�6   .+�5      �� -   5         ��Name$ sensorTypeComboBox��5       �� SelectedIndex$ 0��67   $��5q�6I   .@�5      �� $��Temperature In��6#   �� 5      $��Temperature Out��6#   �� 5      $��Humidity��6#   �� 5      $��Ambient Light��6#   �� 5       $��BarometricPressure��6#   5!      (  "System.Windows.Controls.DatePicker  5"      -   5          Name$ 
datePicker��5"      $��5q�61   5$      �� 5&      -   5         ��Name$ GetWeatherButton��5&      $��Display��6/   
�������
���   �$��35��6}   $
��0 10q�6�   �� 5'      -   5        $
��Help��5'      $��25��67   
���   �
�������6V   5(      5)      5*      