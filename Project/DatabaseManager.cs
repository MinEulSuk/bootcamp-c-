using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

            string query = "SELECT id, co2_ppm, temperature, humidity, pm2_5 FROM sensor_data ORDER BY id DESC LIMIT 1;";

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
                                Pm2_5 = reader.IsDBNull(4) ? (float?)null : reader.GetFloat(4),
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

        public void LogCommand(string commandText)
        {
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

