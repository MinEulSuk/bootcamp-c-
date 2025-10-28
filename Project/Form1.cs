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

        // HardwareController는 주석 처리 (그대로)
        // private HardwareController _hardwareController; 

        // --- 경고 카운터 및 플래그 (그대로) ---
        private int _tempWarningCount = 0;
        private int _humidityWarningCount = 0;
        private int _dustWarningCount = 0;
        private int _co2WarningCount = 0;
        private const int WARNING_THRESHOLD_COUNT = 3;

        private bool _isTempWarningActive = false;
        private bool _isHumidityWarningActive = false;
        private bool _isDustWarningActive = false;
        private bool _isCo2WarningActive = false;

        // 마스터 데이터 소스
        public ChartValues<double> TempValues { get; set; }
        public ChartValues<double> HumidityValues { get; set; }
        public ChartValues<double> DustValues { get; set; }
        public ChartValues<double> CO2Values { get; set; }

        public Form1()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                // [수정] 파일 경로 대신, MySQL 접속 정보가 내장된 새 DB 매니저 생성
                _dbManager = new DatabaseManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 연결 실패: " + ex.Message);
                Environment.Exit(1);
            }

            // --- [수정] 누락되었던 초기화 코드 ---

            // 마스터 데이터 소스 초기화
            TempValues = new ChartValues<double>();
            HumidityValues = new ChartValues<double>();
            DustValues = new ChartValues<double>();
            CO2Values = new ChartValues<double>();

            // 차트 초기 설정 (모든 탭)
            InitializeAllCharts();

            // 타이머 시작 (1초마다 DB 조회)
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            // --- 여기까지 ---
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_dbManager == null) return;

            SensorData data = _dbManager.GetLatestSensorData();
            if (data != null)
            {
                UpdateCharts(data);
                UpdateStatusUI(data);
            }
        }

        private void UpdateCharts(SensorData data)
        {
            // 차트에 값을 추가하기 전에 null 체크
            if (data.Temperature.HasValue) TempValues.Add(Convert.ToDouble(data.Temperature.Value));
            if (data.Humidity.HasValue) HumidityValues.Add(Convert.ToDouble(data.Humidity.Value));
            if (data.Pm2_5.HasValue) DustValues.Add(Convert.ToDouble(data.Pm2_5.Value));
            if (data.Co2Ppm.HasValue) CO2Values.Add(Convert.ToDouble(data.Co2Ppm.Value));

            // 차트에 표시할 최대 데이터 포인트 수
            int maxPoints = 30;
            while (TempValues.Count > maxPoints) TempValues.RemoveAt(0);
            while (HumidityValues.Count > maxPoints) HumidityValues.RemoveAt(0);
            while (DustValues.Count > maxPoints) DustValues.RemoveAt(0);
            while (CO2Values.Count > maxPoints) CO2Values.RemoveAt(0);
        }

        private void UpdateStatusUI(SensorData data)
        {
            bool isAnyWarning = false;

            // [수정] 경고 로직 실행 전 null 체크
            // --- 온도 경고 체크 ---
            if (data.Temperature.HasValue)
            {
                if (data.Temperature < 22.5 || data.Temperature > 23.5) { _tempWarningCount++; } else { _tempWarningCount = 0; }

                if (_tempWarningCount >= WARNING_THRESHOLD_COUNT)
                {
                    panel1.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isTempWarningActive)
                    {
                        _isTempWarningActive = true;
                        string msg = data.Temperature < 22.5 ? "하한선 이탈" : "상한선 초과";
                        _dbManager.LogWarning("온도 경고", data.Temperature.Value, msg);
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    panel1.BackColor = SystemColors.Control;
                    if (_isTempWarningActive)
                    {
                        _isTempWarningActive = false;
                        _dbManager?.LogCommand("FAN_OFF");
                    }
                }
            }

            // --- 습도 경고 체크 ---
            if (data.Humidity.HasValue)
            {
                if (data.Humidity < 40 || data.Humidity > 50) { _humidityWarningCount++; } else { _humidityWarningCount = 0; }

                if (_humidityWarningCount >= WARNING_THRESHOLD_COUNT)
                {
                    panel2.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isHumidityWarningActive)
                    {
                        _isHumidityWarningActive = true;
                        string msg = data.Humidity < 40 ? "하한선 이탈" : "상한선 초과";
                        _dbManager.LogWarning("습도 경고", data.Humidity.Value, msg);
                        // _dbManager?.LogCommand("HUMIDIFIER_ON");
                    }
                }
                else
                {
                    panel2.BackColor = SystemColors.Control;
                    if (_isHumidityWarningActive)
                    {
                        _isHumidityWarningActive = false;
                        // _dbManager?.LogCommand("HUMIDIFIER_OFF");
                    }
                }
            }

            // --- 미세먼지 경고 체크 ---
            if (data.Pm2_5.HasValue)
            {
                if (data.Pm2_5 >= 35) { _dustWarningCount++; } else { _dustWarningCount = 0; }

                if (_dustWarningCount >= WARNING_THRESHOLD_COUNT)
                {
                    panel3.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isDustWarningActive)
                    {
                        _isDustWarningActive = true;
                        _dbManager.LogWarning("미세먼지 경고", data.Pm2_5.Value, "기준치 초과");
                        // _dbManager?.LogCommand("AIR_PURIFIER_ON");
                    }
                }
                else
                {
                    panel3.BackColor = SystemColors.Control;
                    if (_isDustWarningActive)
                    {
                        _isDustWarningActive = false;
                        // _dbManager?.LogCommand("AIR_PURIFIER_OFF");
                    }
                }
            }

            // --- CO2 경고 체크 ---
            if (data.Co2Ppm.HasValue)
            {
                if (data.Co2Ppm >= 1000) { _co2WarningCount++; } else { _co2WarningCount = 0; }

                if (_co2WarningCount >= WARNING_THRESHOLD_COUNT)
                {
                    panel4.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isCo2WarningActive)
                    {
                        _isCo2WarningActive = true;
                        _dbManager.LogWarning("CO2 경고", data.Co2Ppm.Value, "기준치 초과");
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    panel4.BackColor = SystemColors.Control;
                    if (_isCo2WarningActive)
                    {
                        _isCo2WarningActive = false;
                        // _dbManager?.LogCommand("FAN_OFF");
                    }
                }
            }

            this.Text = isAnyWarning ? "!! 경고 !! - 클린룸 통합 모니터링" : "클린룸 통합 모니터링";
        }

        private void InitializeAllCharts()
        {
            // '종합' 탭 차트 연결 (기존)
            chart_Overall_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
            chart_Overall_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
            chart_Overall_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
            chart_Overall_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };

            // --- [수정] '상세' 탭 차트들을 "실제 이름"으로 "똑같은" 데이터 소스에 연결 ---

            try
            {
                // 스크린샷에서 확인한 실제 이름
                chart_Detail_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
                chart_Detail_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
                chart_Detail_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
                chart_Detail_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };
            }
            catch (Exception ex)
            {
                // 이름이 정확하다면 이 에러는 안 뜸
                Console.WriteLine("상세 차트 연결 실패 (이름이 다를 수 있음): " + ex.Message);
            }
        }

        // 폼 종료 시
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // HardwareController Dispose 로직 주석 처리 (그대로)
            // _hardwareController?.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

