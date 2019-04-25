using System;
using System.Collections.Generic;

namespace ProjektDotNet
{
    public class City
    {
        public City(String str) { name = str; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is City objAsCity)) return false;
            else return Equals(objAsCity);
        }

        public bool Equals(City obj)
        {
            return obj.name == name;
        }

        public override String ToString()
        {
            return name;
        }

        public String name { get; set; }
}
}