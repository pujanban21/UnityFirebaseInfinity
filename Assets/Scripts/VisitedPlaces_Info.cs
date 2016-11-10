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
            get {
                return latitude;
            }
            set {
                latitude = value;
            }
        }
        public string Longitude
        {
            get {
                return Longitude;
            }
            set {
                longitude = value;
            }
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
