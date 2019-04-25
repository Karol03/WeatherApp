using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektDotNet
{
    class WeatherGetter
    {
        async static public Task<bool> TryGetAsync(String URL, String cityName)
        {
            try
            {
                CityDB city = await XMLParser.ParseAsync(URL, cityName);
                BlogDBContext.Add(city);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        
        static public CityDB GetCity(String name)
        {
            return BlogDBContext.GetCity(name);
        }
    }
}
