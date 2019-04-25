using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektDotNet
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private Timer timer = new Timer(1000);
        WeatherObserver weatherObserver = new WeatherObserver();
        String activeCityName = "";
        public PlotModel Chart { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            WeatherObserver.SetAction(RefreshChart);
            DataContext = this;
            Chart = Plotter.CreateDefaultChart();
        }

        async private Task<bool> TryAddCityAsync(City city)
        {
            bool result = await weatherObserver.AddCityAsync(city);
            if (result)
            {
                ListBox.Items.Add(city.name);
                return true;
            }
            return false;
        }

        async private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool result = textBox.Text.ToString().All(Char.IsLetter);
            if (result == false || textBox.Text.Length == 0)
            {
                MessageBox.Show("City name has to contain only letters",
                                "Info",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            else
            {
                String rawCityName = textBox.Text.ToString().ToLower();
                String cityName = char.ToUpper(rawCityName[0]) + rawCityName.Substring(1);
                City city = new City(cityName);
                textBox.Clear();
                if (weatherObserver.IsCityWatched(city) == false)
                {
                    bool IsAdded = await TryAddCityAsync(city);
                    if (IsAdded == false)
                    {
                        MessageBox.Show("Cannot reach temperature for given city name",
                                        "Warning",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("City is already in list",
                                    "Info",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            String cityName = Convert.ToString(ListBox.SelectedItem);
            ListBox.Items.Remove(ListBox.SelectedItem);
            weatherObserver.RemoveCity(cityName);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeCityName = Convert.ToString(ListBox.SelectedItem);
            RefreshChart();
        }

        private void RefreshChart()
        {
            Chart = Plotter.DrawChartOf(activeCityName);
            Chart.InvalidatePlot(true);
            ChartView.GetBindingExpression(PlotView.ModelProperty).UpdateTarget();
        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RefreshChart();
        }
    }
}
