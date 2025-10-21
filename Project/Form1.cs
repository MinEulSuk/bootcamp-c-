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
        private DatabaseManager _dbManager;       // DB 전문가 (감시용)

        // --- 깜빡임 제어를 위한 변수 ---
        private bool _isWarning = false;      // 현재 경고 상태인가?
        private bool _isPanelRed = false;     // 패널이 빨간색인가?
        private Timer _blinkTimer;          // 깜빡임을 위한 별도의 타이머

        // 모든 차트가 공유할 마스터 데이터 소스
        public ChartValues<double> TempValues { get; set; }
        public ChartValues<double> HumidityValues { get; set; }
        public ChartValues<double> DustValues { get; set; } // PM2.5 값을 표시한다고 가정
        public ChartValues<double> CO2Values { get; set; }

        public Form1()
        {
            InitializeComponent(); // 디자이너 코드를 실행 (절대 지우면 안 됨!)

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

            // 데이터 조회 타이머 시작
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // 깜빡임 타이머 초기화
            _blinkTimer = new Timer { Interval = 500 }; // 0.5초마다 깜빡임
            _blinkTimer.Tick += BlinkTimer_Tick;
        }

        // --- 모니터링 코드 ---
        private Random _random = new Random();
        private void Timer_Tick(object sender, EventArgs e)
        {
            // --- DB 읽는 부분 대신 가짜 데이터 생성 ---
            SensorData dummyData = new SensorData();

            // 랜덤 값 생성 (예전 코드와 유사하게)
            dummyData.Temperature = (float)(23 + (_random.NextDouble() - 0.5) * 1.5);
            dummyData.Humidity = (float)(45 + (_random.NextDouble() - 0.5) * 10);
            dummyData.Pm2_5 = (float)_random.Next(10, 50); // 미세먼지 값 범위 조절
            dummyData.Co2Ppm = (float)_random.Next(450, 1100);

            // 상태(Status)도 랜덤하게 '정상' 또는 '경고'로 설정 (테스트용)
            dummyData.Status = (_random.Next(0, 5) == 0) ? "경고" : "정상";

            // --- 생성된 가짜 데이터로 차트 및 UI 업데이트 ---
            // 이 부분은 기존 코드와 동일
            UpdateCharts(dummyData);
            UpdateStatusUI(dummyData);
            //SensorData data = _dbManager.GetLatestSensorData();
            //if (data != null)
            //{
            //    UpdateCharts(data);
            //    UpdateStatusUI(data);
            //}
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

        // --- 경고 상태 및 UI 업데이트 코드 ---

        // DB 상태에 따라 경고 모드를 켜고 끄는 메서드
        private void UpdateStatusUI(SensorData data)
        {
            // 경고 상태에 진입하는 경우
            if (data.Status == "경고" && !_isWarning)
            {
                _isWarning = true;        // 경고 모드 ON
                _blinkTimer.Start();      // 깜빡임 타이머 시작
                this.Text = "!! 경고 !! - 클린룸 통합 모니터링";
            }
            // 정상 상태로 복귀하는 경우
            else if (data.Status != "경고" && _isWarning)
            {
                _isWarning = false;       // 경고 모드 OFF
                _blinkTimer.Stop();       // 깜빡임 타이머 정지
                this.Text = "클린룸 통합 모니터링";

                // 모든 패널을 원래 색으로 확실하게 되돌린다.
                SetAllPanelColors(SystemColors.Control);
            }
        }

        // 0.5초마다 실행되어 화면을 깜빡이게 하는 메서드
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (_isPanelRed)
            {
                SetAllPanelColors(SystemColors.Control);
            }
            else
            {
                SetAllPanelColors(System.Drawing.Color.IndianRed);
            }
            _isPanelRed = !_isPanelRed; // 상태 반전
        }

        // 4개 패널의 색상을 한 번에 바꾸는 헬퍼 메서드
        private void SetAllPanelColors(System.Drawing.Color color)
        {
            panel1.BackColor = color;
            panel2.BackColor = color;
            panel3.BackColor = color;
            panel4.BackColor = color;
        }


        // --- 차트 초기 설정 관련 코드 ---

        private void InitializeAllCharts()
        {
            // 종합 현황 탭 차트 설정 (선만 표시)
            chart_Overall_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
            chart_Overall_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
            chart_Overall_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
            chart_Overall_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };

            // 상세 탭 차트 설정 (기준선 포함)
            SetupDetailChart(chart_Detail_Temp, TempValues, "온도 (°C)", "°C", 22.5, 23.5, Brushes.Gold);
            SetupDetailChart(chart_Detail_Humidity, HumidityValues, "습도 (%)", "%", 40, 50, Brushes.DodgerBlue);
            SetupDetailChart(chart_Detail_Dust, DustValues, "미세먼지 (PM2.5 µg/m³)", "µg/m³", null, 35, Brushes.OrangeRed, true);
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
                axis.Sections.Add(new AxisSection { Value = lowerLimit.Value, Stroke = limitColor, StrokeThickness = 2 });
            }
            if (upperLimit.HasValue)
            {
                axis.Sections.Add(new AxisSection { Value = upperLimit.Value, Stroke = limitColor, StrokeThickness = 2 });
            }
            chart.AxisY.Add(axis);
            chart.AxisX.Clear();
            chart.AxisX.Add(new Axis { Title = "Time", Labels = null });
        }

        // --- 차트 제목 관련 코드 ---

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

        // --- 설정 탭 관련 코드 (차트 타입 변경) ---
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

        // 디자이너에서 자동으로 생성된 빈 이벤트 핸들러들
        private void cmbTargetChart_SelectedIndexChanged(object sender, EventArgs e) { }
        private void chart_Overall_CO2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e) { }

    } // End of Form1 class
} // End of namespace

