using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace ProjektDotNet
{
    class WeatherObserver
    {
        static private int OneSecond = 1000; // in ms
        static private int refreshTimePeriodInSeconds = 10;
        static private List<City> watchedCities = new List<City>();
        static private Timer aTimer = new Timer(refreshTimePeriodInSeconds * OneSecond);
        static private Action action;

        public WeatherObserver()
        {
            aTimer.Elapsed += Observe;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        static public void ChangeRefreshTime(int value)
        {
            if (value < 5)
            {
                throw new ArgumentException("Value is too small");
            }
            refreshTimePeriodInSeconds = value;
            aTimer.Stop();
            aTimer.Interval = refreshTimePeriodInSeconds * OneSecond;
            aTimer.Start();
        }

        static public void SetAction(Action action_)
        {
            action = action_;
        }
        
        async private void Observe(Object source, ElapsedEventArgs e)
        {
            bool IsAnyRefresh = false;
            aTimer.Stop();
            foreach (var city in watchedCities)
            {
                IsAnyRefresh = true;
                Console.WriteLine("Observe(Object source, ElapsedEventArgs e)");
                await WeatherGetter.TryGetAsync(GetURL(city), city.name);
            }
            try
            {
                if (IsAnyRefresh)
                {
                    Console.WriteLine("REFRESH");
                    Application.Current.Dispatcher.Invoke(action);
                }
            }
            catch (Exception m)
            {
                Console.WriteLine(m.Message);
            }
            aTimer.Start();
        }

        public bool IsCityWatched(City city)
        {
            var result = watchedCities.SingleOrDefault(c => c.name == city.name);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        async public Task<bool> AddCityAsync(City city)
        {
            bool isCityExist = await IsCityExistAsync(city);
            if (!isCityExist)
            {
                return false;
            }
            watchedCities.Add(city);
            return true;
        }

        public bool RemoveCity(String cityName)
        {
            watchedCities.Remove(new City(cityName));
            return true;
        }

        async private Task<bool> IsCityExistAsync(City city)
        {
            return await WeatherGetter.TryGetAsync(GetURL(city), city.name);
        }

        private String GetURL(City city)
        {
            return "https://api.openweathermap.org/data/2.5/weather?q=" + city.name + "&apikey=3a94fe528c33b53b831237ea0d67007f&mode=xml";
        }
    }

}
