using FEC_Michiten_ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEC_Michiten_ClassLibrary.Kml
{
     public static class KmlHelper
    {

        /// <summary>
        /// Lat, Lonいずれかの値がNaNならTrueを返す（取込み回避用に利用）
        /// </summary>
        /// <param name="kmlDescription"></param>
        /// <returns></returns>
        public static bool IsContainNaN(string kmlDescription)
        {
            var lonRegex = Regex.Match(kmlDescription, @"Lon= NaN");
            var latRegex = Regex.Match(kmlDescription, @"Lat= NaN");

            return (lonRegex.Success || latRegex.Success);
        }


        /// <summary>
        /// 速度、緯度、経度をDescriptionから取り出してセット
        /// </summary>
        /// <param name="kmlDescription"></param>
        /// <param name="setTarget"></param>
        public static void SetAttribute(string kmlDescription, Coordinate setTarget)
        {
            var spdRegex = Regex.Match(kmlDescription, @"Spd= [0-9.]+");
            if (spdRegex.Success)
            {
                setTarget.Spd = AttributeToDouble(spdRegex.Value);
            }

            var lonRegex = Regex.Match(kmlDescription, @"Lon= -?[0-9.]+");
            if (lonRegex.Success)
            {
                setTarget.Lng = AttributeToDouble(lonRegex.Value);
            }

            var latRegex = Regex.Match(kmlDescription, @"Lat= -?[0-9.]+");
            if (latRegex.Success)
            {
                setTarget.Lat = AttributeToDouble(latRegex.Value);
            }
        }


        /// <summary>
        /// 属性文字列から数値文字取り出してDoubleで返す
        /// </summary>
        /// <param name="attributeStr"></param>
        /// <returns></returns>
        private static double AttributeToDouble(string attributeStr)
        {
            var valueStr = Regex.Replace(attributeStr, @"[^0-9.-]+", "");
            if(Double.TryParse(valueStr, out double value))
            {
                return value;
            }

            return 0;
        }



        /// <summary>
        /// 方位情報をStyleUrlから取り出してセット
        /// </summary>
        /// <param name="styleLineStr"></param>
        /// <param name="setTarget"></param>
        public static void SetDirection(string styleLineStr, Coordinate setTarget)
        {
            var directionRegex = Regex.Match(styleLineStr, @"#style_id_direc[0-9.]+");
            if (directionRegex.Success)
            {
                setTarget.Direction = (int)AttributeToDouble(directionRegex.Value);
            }
        }

    }
}
