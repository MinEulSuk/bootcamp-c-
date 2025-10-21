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

        // --- 지능형 경고를 위한 카운터 ---
        private int _tempWarningCount = 0;
        private int _humidityWarningCount = 0;
        private int _dustWarningCount = 0;
        private int _co2WarningCount = 0;
        private const int WARNING_THRESHOLD_COUNT = 3; // 3회 연속 이상이어야 경고!

        // 모든 차트가 공유할 마스터 데이터 소스
        public ChartValues<double> TempValues { get; set; }
        public ChartValues<double> HumidityValues { get; set; }
        public ChartValues<double> DustValues { get; set; } // PM2.5 값을 표시
        public ChartValues<double> CO2Values { get; set; }

        public Form1()
        {
            InitializeComponent();

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

        // --- 모니터링 코드 (테스트용 가짜 데이터 생성) ---
        private Random _random = new Random();
        private void Timer_Tick(object sender, EventArgs e)
        {
            // --- DB 읽는 부분 대신 가짜 데이터 생성 ---
            SensorData dummyData = new SensorData();

            // 랜덤 값 생성
            dummyData.Temperature = (float)(23 + (_random.NextDouble() - 0.5) * 3);
            dummyData.Humidity = (float)(45 + (_random.NextDouble() - 0.5) * 20);
            dummyData.Pm2_5 = (float)_random.Next(10, 50);
            dummyData.Co2Ppm = (float)_random.Next(950, 1100);

            UpdateCharts(dummyData);
            UpdateStatusUI(dummyData);
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

        // --- 지능형 경고 로직 UI 업데이트 코드 ---
        private void UpdateStatusUI(SensorData data)
        {
            bool isAnyWarning = false;

            // 1. 온도 상태 체크
            if (data.Temperature.HasValue && (data.Temperature < 22.5 || data.Temperature > 23.5))
            {
                _tempWarningCount++; // 위반 카운트 증가
            }
            else
            {
                _tempWarningCount = 0; // 정상 범위면 카운트 리셋
            }

            if (_tempWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel1.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
            }
            else
            {
                panel1.BackColor = SystemColors.Control;
            }

            // 2. 습도 상태 체크
            if (data.Humidity.HasValue && (data.Humidity < 40 || data.Humidity > 50))
            {
                _humidityWarningCount++;
            }
            else
            {
                _humidityWarningCount = 0;
            }

            if (_humidityWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel2.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
            }
            else
            {
                panel2.BackColor = SystemColors.Control;
            }

            // 3. 미세먼지 상태 체크
            if (data.Pm2_5.HasValue && data.Pm2_5 >= 35)
            {
                _dustWarningCount++;
            }
            else
            {
                _dustWarningCount = 0;
            }

            if (_dustWarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel3.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
            }
            else
            {
                panel3.BackColor = SystemColors.Control;
            }

            // 4. 이산화탄소 상태 체크
            if (data.Co2Ppm.HasValue && data.Co2Ppm >= 1000)
            {
                _co2WarningCount++;
            }
            else
            {
                _co2WarningCount = 0;
            }

            if (_co2WarningCount >= WARNING_THRESHOLD_COUNT)
            {
                panel4.BackColor = System.Drawing.Color.IndianRed;
                isAnyWarning = true;
            }
            else
            {
                panel4.BackColor = SystemColors.Control;
            }

            // 하나라도 진짜 경고 상태이면 프로그램 제목 변경
            if (isAnyWarning)
            {
                this.Text = "!! 경고 !! - 클린룸 통합 모니터링";
            }
            else
            {
                this.Text = "클린룸 통합 모니터링";
            }
        }

        // --- 이하 코드는 네가 준 버전과 동일 ---

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

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            if (cmbTargetChart.SelectedItem == null)
            {
                MessageBox.Show("차트 타입을 선택해주세요.");
                return;
            }
            string selectedType = cmbTargetChart.SelectedItem.ToString();
            ChangeChartType(chart_Detail_Temp, TempValues, "온도", selectedType);
            ChangeChartType(chart_Detail_Humidity, HumidityValues, "습도", selectedType);
            ChangeChartType(chart_Detail_Dust, DustValues, "미세먼지", selectedType);
            ChangeChartType(chart_Detail_CO2, CO2Values, "이산화탄소", selectedType);
            MessageBox.Show($"상세 차트들이 {selectedType}로 변경되었습니다!");
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

        private void cmbTargetChart_SelectedIndexChanged(object sender, EventArgs e) { }
        private void chart_Overall_CO2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e) { }
    }
}

