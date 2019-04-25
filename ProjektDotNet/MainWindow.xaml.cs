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

        public void RefreshChart()
        {
            Console.WriteLine("refresh chart method");
            Chart = Plotter.DrawChartOf(activeCityName);
            Chart.InvalidatePlot(true);
            ChartView.GetBindingExpression(PlotView.ModelProperty).UpdateTarget();
        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RefreshChart();
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.",
                                "Info",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                textBox1.Clear();
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var value = int.Parse(textBox1.Text);
                WeatherObserver.ChangeRefreshTime(value);
                textBox1.Text = "5";
                String msg = "Refresh frequency was changed to 1 per ";
                int hours = value / 3600;
                int minutes = (value % 3600) / 60;
                int seconds = (value % 60);

                if (hours > 0)
                {
                    msg += hours + "h ";
                }
                if (minutes > 0)
                {
                    msg += minutes + "m ";
                }
                if (seconds> 0)
                {
                    msg += seconds + "s";
                }
                MessageBox.Show(msg,
                                "Info",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message,
                                "Info",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception)
            {}
        }
    }
}
