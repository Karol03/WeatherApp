using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProjektDotNet
{
    class XMLParser
    {
        async static public Task<CityDB> ParseAsync(String URL, String cityName)
        {
            float temp = 0;
            String clouds = "";
            int humidity = 0;
            XmlTextReader reader = new XmlTextReader(URL);
            
            while (reader.Read())
            {
                if (reader.Name == "temperature" && temp == 0)
                {
                    temp = float.Parse(reader.GetAttribute("value"), System.Globalization.CultureInfo.InvariantCulture);
                }
                if (reader.Name == "clouds" && clouds == "")
                {
                    clouds = reader.GetAttribute("name");
                }
                if (reader.Name == "humidity" && humidity == 0)
                {
                    humidity = int.Parse(reader.GetAttribute("value"), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return new CityDB(cityName, ((int)(temp - 273.0)).ToString() + " ", humidity.ToString() + " ", clouds);
        }
    }
}
