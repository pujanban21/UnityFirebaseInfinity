using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class VisitedPlacesInfo
    {
        private const string TAG = "VisitedPlacesInfo";

        private string latitude;
        private string longitude;

        public VisitedPlacesInfo()
        {
        }

        public VisitedPlacesInfo(string latitude, string longitude)
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
            locate[StringValues.LATITUDE] = latitude;
            locate[StringValues.LONGTITUDE] = longitude;

            return locate;
        }
    }
}
