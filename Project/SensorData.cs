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

        // --- [수정] ---
        public float? Pm1_0 { get; set; }   // pm1_0 값을 담기 위해 추가
        public float? Pm2_5 { get; set; }   // (기존)
        public float? Pm10 { get; set; }    // pm10 값을 담기 위해 추가
        // --------------

        public string Status { get; set; } // '정상', '경고' 상태
    }
}