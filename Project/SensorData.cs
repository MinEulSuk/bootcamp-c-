using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class SensorData
    {

        //실제 db에서 가져올 센서데이터
        public int Id { get; set; }
        public float? Co2Ppm { get; set; }
        public float? Temperature { get; set; }
        public float? Humidity { get; set; }
        public float? Pm2_5 { get; set; }
        public string Status { get; set; } // '정상', '경고' 상태
    }
}
