   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5   	    projectsoftawre   WeatherApp.GraphWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  
WeatherApp��5   	   $	��566��6   $	��600��6)   .��5
      � .+�5   
   �� $��
WeatherApp��6   $��24(�6&   $
��Bold"�64   
���   �$��
10,10,0,10q�6Y   $
��Left=�6m   #   LiveCharts.Wpf.CartesianChart  5   
   -   5        $��chart��5      $��
10,50,0,23q�6*   &    LiveCharts.Wpf.Charts.Base.Chart   Series 6>   �� *ChartSeriesCollection+   AxisX 5         LiveCharts.Wpf.Axis     Title$
 Time��65      Labels 6   �� *Labels+65      AxisY 5        $ Value��6      DataTooltip 5      #   LiveCharts.Wpf.DefaultTooltip  5         SelectionMode$ SharedXValuesX�6%   5      5      5      