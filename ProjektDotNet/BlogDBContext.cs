using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektDotNet
{
    public class CityDB
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Temps { get; set; }
        public String Humidity { get; set; }
        public String Clouds { get; set; }
        public int SamplesAmount { get; set; }

        public CityDB(String name, String temp, String humidity, String clouds, int samplesAmount = 1)
        {
            Name = name;
            Temps = temp;
            Humidity = humidity;
            Clouds = clouds;
            SamplesAmount += samplesAmount;
        }

        static public List<int> Convert(String series)
        {
            List<int> list = new List<int>();
            String number = "";
            foreach (var c in series)
            {
                if (char.IsLetter(c))
                {
                    return list;
                }

                if (c == ' ')
                {
                    int x = int.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
                    list.Add(x);
                    number = "";
                }
                else
                {
                    number += c;
                }
            }
            return list;
        }

        public bool Equals(CityDB city)
        {
            return Name == city.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is CityDB objAsCityDB)) return false;
            else return Equals(objAsCityDB);
        }

        public static CityDB operator +(CityDB x, CityDB y)
        {
            if (x.Name != y.Name)
            {
                throw new FormatException();
            }
            x.Temps += y.Temps;
            x.Humidity += y.Humidity;
            x.Clouds = y.Clouds;
            x.SamplesAmount += y.SamplesAmount;
            return x;
        }
    }

    public class BlogDBContext : DbContext
    {
        static private List<CityDB> cities = new List<CityDB>();

        static public void Add(CityDB city)
        {
            try
            {
                if (cities.Contains(city))
                {
                    AppendToCity(city);
                }
                else
                {
                    cities.Add(city);
                }
            }
            catch (Exception)
            {}
        }

        static public CityDB GetCity(String name)
        {
            return cities.Find(c => c.Name == name);
        }

        static private void AppendToCity(CityDB city)
        {
            CityDB city_ = cities.Find(c => c.Name == city.Name);
            city_ += city;
        }

    }
}
