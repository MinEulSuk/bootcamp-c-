using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string dbFilePath)
        {
            if (!File.Exists(dbFilePath))
            {
                throw new FileNotFoundException("DB 파일을 찾을 수 없음", dbFilePath);
            }
            connectionString = $"Data Source={dbFilePath};Version=3;";
        }

        public SensorData GetLatestSensorData()
        {
            SensorData latestData = null;
            string query = "SELECT id, co2_ppm, temperature, humidity, pm2_5, status FROM sensor_data ORDER BY id DESC LIMIT 1;";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            latestData = new SensorData
                            {
                                Id = reader.GetInt32(0),
                                Co2Ppm = reader.IsDBNull(1) ? (float?)null : reader.GetFloat(1),
                                Temperature = reader.IsDBNull(2) ? (float?)null : reader.GetFloat(2),
                                Humidity = reader.IsDBNull(3) ? (float?)null : reader.GetFloat(3),
                                Pm2_5 = reader.IsDBNull(4) ? (float?)null : reader.GetFloat(4),
                                Status = reader.IsDBNull(5) ? "알 수 없음" : reader.GetString(5)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 읽기 실패: " + ex.Message);
                return null;
            }
            return latestData;
        }
    }
}
