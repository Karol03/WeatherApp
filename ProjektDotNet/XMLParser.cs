using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ProjektDotNet
{
    class XMLParser
    {
        async static public Task<WeatherGetter.ParserData> ParseAsync(String URL, String cityName)
        {
            Console.WriteLine("ParseAsync(String URL, String cityName)");
            string rawXML = await GetRawDataAsync(URL);
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(rawXML)))
            {
                return GetParserData(stream, cityName);
            }
            throw new InvalidDataException("Cannot download and parse data from weather site");
        }

        static async private Task<string> GetRawDataAsync(string URL)
        {
            Task<string> result;
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(URL))
            using (HttpContent content = response.Content)
            {
                result = content.ReadAsStringAsync();
            }
            return await result;
        }

        static private WeatherGetter.ParserData GetParserData(System.IO.Stream stream, String cityName)
        {
            float temp = 0;
            String clouds = "";
            int humidity = 0;
            XmlTextReader reader = new XmlTextReader(stream);

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

            return new WeatherGetter.ParserData() { Name = cityName, Clouds = clouds, Humidity = humidity.ToString() + " ", Temp = ((int)temp - 273).ToString() + " " };
        }
    }
}
