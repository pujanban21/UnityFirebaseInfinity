using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class VisitedPlaces_Info
    {
        public string latitude;
        public string longitude;

        public VisitedPlaces_Info()
        {
        }

        public VisitedPlaces_Info(string latitude, string longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }


        public string Latitude
        {
            get; set;
        }
        public string Longitude
        {
            get; set;
        }

        public Dictionary<string, Object> SaveLocation()
        {
            Dictionary<string, Object> locate = new Dictionary<string, object>();
            locate["latitude"] = latitude;
            locate["longitude"] = longitude;

            return locate;
        }
    }
}
