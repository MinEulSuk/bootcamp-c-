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
            /*
            // [수정] 절대 경로를 쓰면 이 파일 존재 여부 체크가 오히려 방해가 될 수 있음.
            // 일단 주석 처리. DB 연결은 C#이 아니라 파이썬이 생성하는 것을 전제로 함.
            if (!File.Exists(dbFilePath))
            {
                throw new FileNotFoundException("DB 파일을 찾을 수 없음", dbFilePath);
            }
            */
            connectionString = $"Data Source={dbFilePath};Version=3;";
        }

        public SensorData GetLatestSensorData()
        {
            SensorData latestData = null;

            // [수정 1] 
            // 파이썬이 생성한 DB 컬럼 이름(대소문자 구분)과 정확히 일치시킴
            // 'status' 컬럼은 파이썬이 만든 적 없으므로 쿼리에서 제거
            string query = "SELECT id, Co2Ppm, Temperature, Humidity, Pm2_5 FROM sensor_data ORDER BY id DESC LIMIT 1;";

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

                                // [수정 2] 
                                // 'status' 컬럼은 읽어오지 않으므로 C#에서도 매핑 로직 제거
                                // Status = reader.IsDBNull(5) ? "알 수 없음" : reader.GetString(5)
                                Status = "DB에서 안 읽음" // 임시값
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 읽기 실패: " + ex.Message);
                // [수정 3] 에러가 났을 때 메시지 박스로 띄워야 확실히 알 수 있음
                System.Windows.Forms.MessageBox.Show("DB 읽기 실패!\n" + ex.Message);
                return null;
            }
            return latestData;
        }

        // --- 경고 로그를 DB에 기록하는 새로운 메서드 추가 ---
        // (이건 수정할 필요 없음. 나중에 warning_log 테이블 만들고 쓸 거)
        public void LogWarning(string warningType, float value, string message)
        {
            // 'warning_log' 테이블이 있어야 동작함
            string query = "INSERT INTO warning_log (warning_type, value, message) VALUES (@type, @val, @msg)";

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@type", warningType);
                    command.Parameters.AddWithValue("@val", value);
                    command.Parameters.AddWithValue("@msg", message);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("경고 로그 기록 실패: " + ex.Message);
            }
        }

        // [수정 4] Form1.cs에서 이 메서드를 호출할 것이므로 추가
        public void LogCommand(string command) // <-- 파라미터 이름은 'command' (문제 없음)
        {
            // 'commands' 테이블이 있어야 동작함
            string query = "INSERT INTO commands (command_text, processed) VALUES (@command, 0)";
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // [수정] 변수 이름을 'cmd'로 변경해서 파라미터 'command'와의 충돌을 피함
                    var cmd = new SQLiteCommand(query, connection);

                    cmd.Parameters.AddWithValue("@command", command); // <-- @command에는 파라미터 'command'를 전달
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