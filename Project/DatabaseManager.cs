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

        // --- 경고 로그를 DB에 기록하는 새로운 메서드 추가 --- 예시
        public void LogWarning(string warningType, float value, string message)
        {
            // 'warning_log' 테이블에 데이터를 삽입하는 SQL 쿼리
            string query = "INSERT INTO warning_log (warning_type, value, message) VALUES (@type, @val, @msg)";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);

                    // SQL 인젝션 공격을 방지하기 위해 파라미터 사용
                    command.Parameters.AddWithValue("@type", warningType);
                    command.Parameters.AddWithValue("@val", value);
                    command.Parameters.AddWithValue("@msg", message);

                    // 쿼리 실행 (데이터를 읽어오는 게 아니므로 ExecuteNonQuery 사용)
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // UI를 멈추게 할 순 없으니, 콘솔에만 에러를 기록한다.
                Console.WriteLine("경고 로그 기록 실패: " + ex.Message);
            }
        }
    }
}
