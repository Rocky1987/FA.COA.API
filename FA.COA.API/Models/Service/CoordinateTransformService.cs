using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FA.COA.API.Models.Service
{
    public class CoordinateTransformService
    {
        private static double a = 6378137.0;
        private static double b = 6356752.3142451;
        private double lon0 = 121 * Math.PI / 180;
        private double k0 = 0.9999;
        private int dx = 250000;
        private int dy = 0;
        private double e = 1 - Math.Pow(b, 2) / Math.Pow(a, 2);
        private double e2 = (1 - Math.Pow(b, 2) / Math.Pow(a, 2)) / (Math.Pow(b, 2) / Math.Pow(a, 2));

        public CoordinateTransformService()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        public PointLocation lonlat_To_twd97(int lonD, int lonM, int lonS, int latD, int latM, int latS)
        {
            double RadianLon = (double)(lonD) + (double)lonM / 60 + (double)lonS / 3600;
            double RadianLat = (double)(latD) + (double)latM / 60 + (double)latS / 3600;
            return Cal_lonlat_To_twd97(RadianLon, RadianLat);
        }

        public PointLocation Cal_lonlat_To_twd97(double lon, double lat)
        {
            string TWD97 = "";

            lon = (lon - Math.Floor((lon + 180) / 360) * 360) * Math.PI / 180;
            lat = lat * Math.PI / 180;

            double V = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(lat), 2));
            double T = Math.Pow(Math.Tan(lat), 2);
            double C = e2 * Math.Pow(Math.Cos(lat), 2);
            double A = Math.Cos(lat) * (lon - lon0);
            double M = a * ((1.0 - e / 4.0 - 3.0 * Math.Pow(e, 2) / 64.0 - 5.0 * Math.Pow(e, 3) / 256.0) * lat -
                  (3.0 * e / 8.0 + 3.0 * Math.Pow(e, 2) / 32.0 + 45.0 * Math.Pow(e, 3) / 1024.0) *
                  Math.Sin(2.0 * lat) + (15.0 * Math.Pow(e, 2) / 256.0 + 45.0 * Math.Pow(e, 3) / 1024.0) *
                  Math.Sin(4.0 * lat) - (35.0 * Math.Pow(e, 3) / 3072.0) * Math.Sin(6.0 * lat));
            // x
            double x = dx + k0 * V * (A + (1 - T + C) * Math.Pow(A, 3) / 6 + (5 - 18 * T + Math.Pow(T, 2) + 72 * C - 58 * e2) * Math.Pow(A, 5) / 120);
            // y
            double y = dy + k0 * (M + V * Math.Tan(lat) * (Math.Pow(A, 2) / 2 + (5 - T + 9 * C + 4 * Math.Pow(C, 2)) * Math.Pow(A, 4) / 24 + (61 - 58 * T + Math.Pow(T, 2) + 600 * C - 330 * e2) * Math.Pow(A, 6) / 720));

            TWD97 = x.ToString() + "," + y.ToString();
            return new PointLocation(x.ToString(), y.ToString());
          
        }

        /// <summary>
        /// 點物件
        /// </summary>
        public class PointLocation
        {
            public string x = "";
            public string y = "";

            public PointLocation(string _x, string _y)
            {
                x = _x;
                y = _y;
            }

            public PointLocation() { }
        }
    }
}