using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using System.ComponentModel; // LicenseManager 사용

namespace Project
{
    public partial class Form1 : Form
    {
        private Timer _timer;
        private DatabaseManager _dbManager; // DB 전문가 (감시용)
        private HardwareController _hardwareController; // 장비 전문가 (제어용)

        // --- 지능형 경고를 위한 카운터 ---
        private int _tempWarningCount = 0;
        private int _humidityWarningCount = 0;
        private int _dustWarningCount = 0;
        private int _co2WarningCount = 0;
        private const int WARNING_THRESHOLD_COUNT = 3; // 3회 연속 이상이어야 경고!

        // --- 경고 상태 지속 확인용 플래그 ---
        private bool _isTempWarningActive = false;
        private bool _isHumidityWarningActive = false;
        private bool _isDustWarningActive = false;
        private bool _isCo2WarningActive = false;

        // 모든 차트가 공유할 마스터 데이터 소스
        public ChartValues<double> TempValues { get; set; }
        public ChartValues<double> HumidityValues { get; set; }
        public ChartValues<double> DustValues { get; set; } // PM2.5 값을 표시
        public ChartValues<double> CO2Values { get; set; }

        public Form1()
        {
            InitializeComponent();

            // --- 디자이너에서 실행 중이면 아래 코드는 건너뜀 ---
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // 런타임 초기화
            try
            {
                _hardwareController = new HardwareController("COM8", 115200); // COM 포트 연결 (디자이너에서는 실패)
            }
            catch (Exception ex)
            {
                MessageBox.Show("장비 제어 포트 연결 실패: " + ex.Message);
            }

            try
            {
                _dbManager = new DatabaseManager("sensor_database.db"); // DB 연결 (디자이너에서는 실패)
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 연결 실패: " + ex.Message);
                Environment.Exit(1);
            }

            // 마스터 데이터 소스 초기화 (디자이너에서도 안전)
            TempValues = new ChartValues<double>();
            HumidityValues = new ChartValues<double>();
            DustValues = new ChartValues<double>();
            CO2Values = new ChartValues<double>();

            // 차트 초기 설정 (디자이너에서는 생략 가능)
            InitializeAllCharts();

            // 타이머 시작 (1초마다 DB 조회)
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_dbManager == null) return; // 디자이너에서 안전

            SensorData data = _dbManager.GetLatestSensorData();
            if (data != null)
            {
                UpdateCharts(data);
                UpdateStatusUI(data);
            }
        }

        private void UpdateCharts(SensorData data)
        {
            if (data.Temperature.HasValue) TempValues.Add(Convert.ToDouble(data.Temperature.Value));
            if (data.Humidity.HasValue) HumidityValues.Add(Convert.ToDouble(data.Humidity.Value));
            if (data.Pm2_5.HasValue) DustValues.Add(Convert.ToDouble(data.Pm2_5.Value));
            if (data.Co2Ppm.HasValue) CO2Values.Add(Convert.ToDouble(data.Co2Ppm.Value));

            int maxPoints = 30;
            while (TempValues.Count > maxPoints) TempValues.RemoveAt(0);
            while (HumidityValues.Count > maxPoints) HumidityValues.RemoveAt(0);
            while (DustValues.Count > maxPoints) DustValues.RemoveAt(0);
            while (CO2Values.Count > maxPoints) CO2Values.RemoveAt(0);
        }

        private void UpdateStatusUI(SensorData data)
        {
            bool isAnyWarning = false;

            // --- 경고 체크 및 자동 제어 코드 ---
            if (data.Temperature.HasValue && (data.Temperature < 22.5 || data.Temperature > 23.5)) { _tempWarningCount++; } else { _tempWarningCount = 0; }
            if (_tempWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel1.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
                _hardwareController?.TurnFanOn();
                if (!_isTempWarningActive)
                {
                    _isTempWarningActive = true;
                    string msg = data.Temperature < 22.5 ? "하한선 이탈" : "상한선 초과";
                    _dbManager.LogWarning("온도 경고", data.Temperature.Value, msg);
                }
            }
            else
            {
                panel1.BackColor = SystemColors.Control;
                if (_isTempWarningActive)
                {
                    _isTempWarningActive = false;
                    _hardwareController?.TurnFanOff();
                }
            }

            // 습도
            if (data.Humidity.HasValue && (data.Humidity < 40 || data.Humidity > 50)) { _humidityWarningCount++; } else { _humidityWarningCount = 0; }
            if (_humidityWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel2.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
                if (!_isHumidityWarningActive)
                {
                    _isHumidityWarningActive = true;
                    string msg = data.Humidity < 40 ? "하한선 이탈" : "상한선 초과";
                    _dbManager.LogWarning("습도 경고", data.Humidity.Value, msg);
                }
            }
            else
            {
                panel2.BackColor = SystemColors.Control;
                _isHumidityWarningActive = false;
            }

            // 미세먼지
            if (data.Pm2_5.HasValue && data.Pm2_5 >= 35) { _dustWarningCount++; } else { _dustWarningCount = 0; }
            if (_dustWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel3.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
                if (!_isDustWarningActive)
                {
                    _isDustWarningActive = true;
                    _dbManager.LogWarning("미세먼지 경고", data.Pm2_5.Value, "기준치 초과");
                }
            }
            else
            {
                panel3.BackColor = SystemColors.Control;
                _isDustWarningActive = false;
            }

            // CO2
            if (data.Co2Ppm.HasValue && data.Co2Ppm >= 1000) { _co2WarningCount++; } else { _co2WarningCount = 0; }
            if (_co2WarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel4.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
                if (!_isCo2WarningActive)
                {
                    _isCo2WarningActive = true;
                    _dbManager.LogWarning("CO2 경고", data.Co2Ppm.Value, "기준치 초과");
                }
            }
            else
            {
                panel4.BackColor = SystemColors.Control;
                _isCo2WarningActive = false;
            }

            this.Text = isAnyWarning ? "!! 경고 !! - 클린룸 통합 모니터링" : "클린룸 통합 모니터링";
        }

        private void InitializeAllCharts()
        {
            // 차트 초기화 (디자이너에서도 안전)
            chart_Overall_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
            chart_Overall_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
            chart_Overall_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
            chart_Overall_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };
        }

        // 폼 종료 시
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _hardwareController?.Dispose();
        }
    }
}
