using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektDotNet
{
    class Plotter
    {
        static public PlotModel DrawChartOf(String cityName)
        {
            try
            {
                Console.WriteLine(cityName);
                var city = WeatherGetter.GetCity(cityName);
         
                var plot = new PlotModel
                {
                    Title = city.Name,
                    Subtitle = "Clouds: " + city.Clouds
                };

                var temps = CityDB.Convert(city.Temps);
                var humidity = CityDB.Convert(city.Humidity);

                int temp_min = temps.Min() - 2;
                int temp_max = temps.Max() + 2;
                
                plot.Axes.Add(new LinearAxis
                {
                    Title = "Temperature [C]",
                    MinorGridlineThickness = 10,
                    MajorGridlineStyle = LineStyle.Solid,
                    Position = AxisPosition.Left,
                    Minimum = temp_min,
                    Maximum = temp_max,
                    MajorStep = 2,
                    MinorStep = 1,
                    TickStyle = TickStyle.Inside
                });
                plot.Axes.Add(new LinearAxis
                {
                    Title = "Samples",
                    MinorGridlineThickness = 10,
                    MajorGridlineStyle = LineStyle.Solid,
                    Position = AxisPosition.Bottom,
                    Minimum = 0,
                    Maximum = 26,
                    MajorStep = 1,
                    MinorStep = 1,
                    TickStyle = TickStyle.Inside
                });
                plot.Axes.Add(new LinearAxis
                {
                    Title = "Humidity [%]",
                    Position = AxisPosition.Right,
                    Minimum = 0,
                    Maximum = 100,
                    MajorStep = 10,
                    MinorStep = 5,
                    TickStyle = TickStyle.Inside
                });

                plot.Series.Add(CreateSeriesOf(temps, "Temperature"));
                plot.Series.Add(CreateSeriesOf(humidity, "Humidity", (double)temp_min, (double)temp_max));
                return plot;
            }
            catch (Exception)
            {}
            return CreateDefaultChart();
        }

        private static LineSeries CreateSeriesOf(List<int> values, String title)
        {
            var ls = new LineSeries
            {
                Title = title
            };

            int x = 0;
            foreach (var y in values)
            {
                ls.Points.Add(new DataPoint(x, y));
                x++;
            }
            return ls;
        }

        private static LineSeries CreateSeriesOf(List<int> values, String title, double min, double max)
        {
            var ls = new LineSeries
            {
                Title = title
            };

            int x = 0;
            foreach (var y in values)
            {
                double yy = (double)y;
                yy = (max - min) / 100.0 * yy + min;
                ls.Points.Add(new DataPoint(x, yy));
                x++;
            }
            return ls;
        }

        static public PlotModel CreateDefaultChart()
        {
            var plot = new PlotModel
            {
                Title = "City",
                Subtitle = "Current weather"
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 10,
                MajorStep = 1,
                MinorStep = 1,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -5,
                Maximum = 25,
                MajorStep = 1,
                MinorStep = 1,
                TickStyle = TickStyle.Inside
            });
            return plot;
        }
    }
}
