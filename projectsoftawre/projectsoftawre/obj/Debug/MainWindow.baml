   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    projectsoftawre   WeatherApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  
WeatherApp��5   	   $	��600��6   $	��525��6)    � 
ResizeMode$ NoResize=�65   .��5
      � .+�5   
   �� $��
WeatherApp��6   $��24(�6&   $
��Bold"�64   
���   �$��0 0 0 15q�6Y   $��Center=�6k   #   LiveCharts.Wpf.CartesianChart  5   
   -   5        $��chart��5      $��	5,36,5,92q�6*   &    LiveCharts.Wpf.Charts.Base.Chart   Series 6=   �� *ChartSeriesCollection+   AxisX 5         LiveCharts.Wpf.Axis     Title$
 Time��65      Labels 6   �� *Labels+65   5      �� 5   
    �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $��Center=�6F   $��	0,487,0,0q�6c   .+�5      �� -   5         ��Name$ deviceComboBox��5       �� SelectedIndex$ 0��6/   $��5q�6A   .@�5      �� $��
lht-saxion��6   �� 5      $��
lht-gronau��6   �� 5      $��lht-wierden��6   5      �� 5      -   5        $ sensorTypeComboBox��5      $ 0��63   $��5q�6E   .@�5      �� $��Temperature In��6   �� 5      $��Temperature Out��6   �� 5       $��Humidity��6   �� 5!      $��Ambient Light��6   5"      (  "System.Windows.Controls.DatePicker  5#      -   5        $��
datePicker��5#      $��5q�6+   5$      �� 5(   
   -   5        $��Display��5(      $��	0,530,0,0q�6#   
���   �
�������$	��183��6r   $	��Top=�6~   $��Center=�6�   $��29��6�   �� 5*   
   -   5        $��24��5*      $��24��6   $	��Top=�6(   $��Right=�6@   $��
0,534,10,0q�6\   
���   �
�������$��i��6�   $��0q�6�   
������ ��5+      �� 5,      	 �� 
TargetType"	 ��6"   .R�5-      � .+�5.      A� #��z��6"   �� 5/      $��Center=�6+   $��Center=�6H   50      51      53      55      56      