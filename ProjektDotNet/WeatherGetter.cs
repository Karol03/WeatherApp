using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektDotNet
{
    class WeatherGetter
    {
        public class ParserData
        {
            public String Name;
            public String Humidity;
            public String Temp;
            public String Clouds;
            
            static public CityDB ConvertToCityDB(ParserData rawData)
            {
                CityDB city = new CityDB
                {
                    Name = rawData.Name,
                    Humidity = rawData.Humidity,
                    Temps = rawData.Temp,
                    Clouds = rawData.Clouds
            };
                return city;
            }
        }

        static private BlogDBContext Db = new BlogDBContext();
        async static public Task<bool> TryGetAsync(String URL, String cityName)
        {
            Console.WriteLine("TryGetAsync(String URL, String cityName)");
            try
            {
                ParserData rawData = await XMLParser.ParseAsync(URL, cityName);
                AppendToDatabase(rawData);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
            return true;
        }

        async static public void RemoveCity(String cityName)
        {
            var city = Db.Cities.SingleOrDefault(c => c.Name == cityName);
            if (city != null)
            {
                Db.Cities.Remove(city);
            }
            await Db.SaveChangesAsync();
        }

        static public List<String> GetAllCitiesNamesFromDatabase()
        {
            List<String> ls = new List<String>();

            foreach (var city in Db.Cities)
            {
                ls.Add(city.Name);
            }
            return ls;
        }

        static private void AppendToDatabase(ParserData city)
        {
            Console.WriteLine("AppendToDatabaseAsync(ParserData city)");
            var result = Db.Cities.SingleOrDefault(c => c.Name == city.Name);
            if (result != null)
            {
                Console.WriteLine("Update existing city");
                result.Temps += city.Temp;
                result.Humidity += city.Humidity;
                result.Clouds = city.Clouds;
            }
            else
            {
                Console.WriteLine("AppendToDatabase");
                var cities = Db.Set<CityDB>();
                cities.Add(new CityDB { Name = city.Name, Clouds = city.Clouds, Humidity = city.Humidity, Temps = city.Temp });
            }
            Db.SaveChanges();
        }
        
        async static public Task<CityDB> GetCity(String name)
        {
            return await Db.Cities.FindAsync(name);
        }
    }
}
