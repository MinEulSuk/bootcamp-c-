using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
// using System.Data.SqlClient; // MySQL만 사용하므로 이 줄은 제거해도 됩니다.
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager()
        {
            // DB 접속 정보 수정 필수!!
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";    // host="localhost"
            builder.UserID = "root";         // user="root"
            builder.Password = "1234";       // password="1234"
            builder.Database = "sensor_project"; // database="sensor_project"

            connectionString = builder.ToString();
        }

        public SensorData GetLatestSensorData()
        {
            SensorData latestData = null;

            // [수정] pm1_0, pm10 컬럼도 함께 읽어오도록 쿼리 변경
            string query = "SELECT id, co2_ppm, temperature, humidity, pm1_0, pm2_5, pm10 FROM sensor_data ORDER BY id DESC LIMIT 1;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(query, connection);

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

                                // [수정] 올바른 인덱스에서 Pm1.0, Pm2.5, Pm10 값을 읽어옴 (버그 수정)
                                Pm1_0 = reader.IsDBNull(4) ? (float?)null : reader.GetFloat(4),
                                Pm2_5 = reader.IsDBNull(5) ? (float?)null : reader.GetFloat(5),
                                Pm10 = reader.IsDBNull(6) ? (float?)null : reader.GetFloat(6),

                                Status = "DB에서 안 읽음"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 읽기 실패: " + ex.Message);
                MessageBox.Show("DB 읽기 실패!\n" + ex.Message); // 에러를 시각적으로 표시
                return null;
            }
            return latestData;
        }

        public void LogWarning(string warningType, float value, string message)
        {
            // (이 메서드는 이제 Form1에서 사용되지 않지만, 만일을 위해 남겨둡니다)
            string query = "INSERT INTO warning_log (warning_type, value, message) VALUES (@type, @val, @msg)";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@type", warningType);
                    cmd.Parameters.AddWithValue("@val", value);
                    cmd.Parameters.AddWithValue("@msg", message);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("경고 로그 기록 실패: " + ex.Message);
            }
        }

        // --- [수정된 부분] ---
        // 'outline_table' 스키마에 맞게 INSERT 쿼리와 파라미터를 수정한 메서드
        public void LogOutlierData(SensorData data, string message)
        {
            // [수정] 1. 테이블 이름을 'outline_table'로 변경
            // [수정] 2. 컬럼 'message'를 'message_key'로 변경
            // [참고] 'pressure', 'altitude'는 SensorData에 없으므로 INSERT에서 제외 (자동 NULL)
            string query = @"INSERT INTO outline_table 
                             (co2_ppm, temperature, humidity, pm1_0, pm2_5, pm10, message_key) 
                             VALUES (@co2, @temp, @hum, @pm1, @pm25, @pm10, @msg_key)";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);

                    // DBNull.Value 처리를 통해 Nullable<float> (float?) 값을 안전하게 삽입
                    cmd.Parameters.AddWithValue("@co2", data.Co2Ppm.HasValue ? (object)data.Co2Ppm.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@temp", data.Temperature.HasValue ? (object)data.Temperature.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@hum", data.Humidity.HasValue ? (object)data.Humidity.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pm1", data.Pm1_0.HasValue ? (object)data.Pm1_0.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pm25", data.Pm2_5.HasValue ? (object)data.Pm2_5.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@pm10", data.Pm10.HasValue ? (object)data.Pm10.Value : DBNull.Value);

                    // [수정] 3. 파라미터 이름을 '@msg_key'로 변경
                    cmd.Parameters.AddWithValue("@msg_key", message);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("이상치(Outlier) 로그 기록 실패: " + ex.Message);
            }
        }
        // --- [수정 완료] ---


        public void LogCommand(string commandText)
        {
            // (이 메서드는 이제 Form1에서 사용되지 않지만, 만일을 위해 남겨둡니다)
            string query = "INSERT INTO commands (command_text, processed) VALUES (@command, 0)";
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@command", commandText); // 파라미터 이름과 변수 이름 충돌 방지
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 명령 기록 실패: " + ex.Message);
            }
        }
    }
}