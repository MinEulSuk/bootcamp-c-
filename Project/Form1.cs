using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

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

            // 장비 전문가 고용
            try
            {
                _hardwareController = new HardwareController("COM8", 115200);
            }
            catch (Exception ex)
            {
                MessageBox.Show("장비 제어 포트 연결 실패: " + ex.Message);
            }

            // DB 전문가 고용
            try
            {
                _dbManager = new DatabaseManager("sensor_database.db");
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 연결 실패: " + ex.Message);
                Environment.Exit(1);
            }

            // 마스터 데이터 소스 초기화
            TempValues = new ChartValues<double>();
            HumidityValues = new ChartValues<double>();
            DustValues = new ChartValues<double>();
            CO2Values = new ChartValues<double>();

            // 차트 초기 설정
            InitializeAllCharts();

            // 타이머 시작 (1초마다 DB 조회)
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        //// --- 모니터링 코드 (테스트용 가짜 데이터) ---
        //private Random _random = new Random();
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    SensorData dummyData = new SensorData();
        //    dummyData.Temperature = (float)(23 + (_random.NextDouble() - 0.5) * 3);
        //    dummyData.Humidity = (float)(45 + (_random.NextDouble() - 0.5) * 20);
        //    dummyData.Pm2_5 = (float)_random.Next(10, 50);
        //    dummyData.Co2Ppm = (float)_random.Next(950, 1100);

        //    UpdateCharts(dummyData);
        //    UpdateStatusUI(dummyData);
        //}

        // --- 모니터링 코드 (테스트용 가짜 데이터) ---
        //private Random _random = new Random();
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 실제 DB에서 데이터 가져오기
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

        // --- 지능형 경고 및 '자동 대응' 로직 ---
        private void UpdateStatusUI(SensorData data)
        {
            bool isAnyWarning = false;

            // 1. 온도 상태 체크 및 자동 대응/기록
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

            // 2. 습도 상태 체크
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

            // 3. 미세먼지 상태 체크
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

            // 4. 이산화탄소 상태 체크
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

            if (isAnyWarning) { this.Text = "!! 경고 !! - 클린룸 통합 모니터링"; } else { this.Text = "클린룸 통합 모니터링"; }
        }

        // --- 이하 코드는 거의 변경 없음 ---

        private void InitializeAllCharts()
        {
            chart_Overall_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
            chart_Overall_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
            chart_Overall_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
            chart_Overall_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };

            SetupDetailChart(chart_Detail_Temp, TempValues, "온도 (°C)", "°C", 22.5, 23.5, Brushes.Gold);
            SetupDetailChart(chart_Detail_Humidity, HumidityValues, "습도 (%)", "%", 40, 50, Brushes.DodgerBlue);
            SetupDetailChart(chart_Detail_Dust, DustValues, "미세먼지 (PM2.5 µg/m³)", "µg/m³", null, 35, Brushes.OrangeRed, false);
            SetupDetailChart(chart_Detail_CO2, CO2Values, "이산화탄소 (ppm)", "ppm", null, 1000, Brushes.Red);
        }

        private void SetupDetailChart(LiveCharts.WinForms.CartesianChart chart, ChartValues<double> values, string seriesTitle, string axisYTitle, double? lowerLimit, double? upperLimit, System.Windows.Media.Brush limitColor, bool useColumnSeries = false)
        {
            ISeriesView series = useColumnSeries
                ? (ISeriesView)new ColumnSeries { Title = seriesTitle, Values = values }
                : (ISeriesView)new LineSeries { Title = seriesTitle, Values = values, PointGeometry = null };

            chart.Series = new SeriesCollection { series };
            chart.AxisY.Clear();
            var axis = new Axis { Title = axisYTitle, Sections = new SectionsCollection() };
            if (lowerLimit.HasValue)
            {
                axis.Sections.Add(new AxisSection { Value = lowerLimit.Value, Stroke = limitColor, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 4 }) });
            }
            if (upperLimit.HasValue)
            {
                axis.Sections.Add(new AxisSection { Value = upperLimit.Value, Stroke = limitColor, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 4 }) });
            }
            chart.AxisY.Add(axis);
            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis { Title = "Time", Labels = null });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddChartTitles();
        }

        private void AddChartTitles()
        {
            AddTitleToChart(chart_Overall_Temp, "온도 (°C)");
            AddTitleToChart(chart_Overall_Humidity, "습도 (%)");
            AddTitleToChart(chart_Overall_Dust, "미세먼지 (PM2.5)");
            AddTitleToChart(chart_Overall_CO2, "이산화탄소 (ppm)");
            AddTitleToChart(chart_Detail_Temp, "온도 상세 (기준: 22.5~23.5°C)");
            AddTitleToChart(chart_Detail_Humidity, "습도 상세 (기준: 40~50%)");
            AddTitleToChart(chart_Detail_Dust, "미세먼지 상세 (PM2.5 기준: <35 µg/m³)");
            AddTitleToChart(chart_Detail_CO2, "이산화탄소 상세 (기준: <1000 ppm)");
        }

        private void AddTitleToChart(LiveCharts.WinForms.CartesianChart chart, string title)
        {
            string labelName = chart.Name + "_Title";
            if (chart.Parent.Controls.ContainsKey(labelName)) return;
            Label titleLabel = new Label
            {
                Name = labelName,
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 25,
                BackColor = System.Drawing.Color.LightGray,
                ForeColor = System.Drawing.Color.Black
            };
            chart.Parent.Controls.Add(titleLabel);
            titleLabel.BringToFront();
        }

        // --- '차트 타입 적용' 버튼 클릭 이벤트 핸들러 (수정됨) ---
        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            if (cmbTargetChart.SelectedItem == null)
            {
                MessageBox.Show("차트 타입을 선택해주세요.");
                return;
            }
            string selectedType = cmbTargetChart.SelectedItem.ToString();

            // 상세 탭 차트들 변경
            ChangeChartType(chart_Detail_Temp, TempValues, "온도", selectedType);
            ChangeChartType(chart_Detail_Humidity, HumidityValues, "습도", selectedType);
            ChangeChartType(chart_Detail_Dust, DustValues, "미세먼지", selectedType);
            ChangeChartType(chart_Detail_CO2, CO2Values, "이산화탄소", selectedType);

            // --- 종합 현황 탭 차트들도 함께 변경 (추가된 부분) ---
            ChangeChartType(chart_Overall_Temp, TempValues, "온도", selectedType);
            ChangeChartType(chart_Overall_Humidity, HumidityValues, "습도", selectedType);
            ChangeChartType(chart_Overall_Dust, DustValues, "미세먼지", selectedType);
            ChangeChartType(chart_Overall_CO2, CO2Values, "이산화탄소", selectedType);

            MessageBox.Show($"모든 차트가 {selectedType}로 변경되었습니다!");
        }

        private void ChangeChartType(LiveCharts.WinForms.CartesianChart chart, ChartValues<double> values, string title, string chartType)
        {
            ISeriesView newSeries;
            switch (chartType)
            {
                case "컬럼 차트": newSeries = new ColumnSeries { Title = title, Values = values }; break;
                case "계단식 라인": newSeries = new StepLineSeries { Title = title, Values = values }; break;
                case "라인 차트": default: newSeries = new LineSeries { Title = title, Values = values, PointGeometry = null }; break;
            }
            chart.Series.Clear();
            chart.Series.Add(newSeries);
        }

        // 폼이 닫힐 때 자원 정리
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _hardwareController?.Dispose();
        }

        private void cmbTargetChart_SelectedIndexChanged(object sender, EventArgs e) { }
        private void chart_Overall_CO2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e) { }

        // 수동 제어 버튼 이벤트 핸들러 (UI에 버튼 추가 필요)
        private void btnFanOn_Click(object sender, EventArgs e)
        {
            _hardwareController?.TurnFanOn();
        }

        private void btnFanOff_Click(object sender, EventArgs e)
        {
            _hardwareController?.TurnFanOff();
        }
    }
}

