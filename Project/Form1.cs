using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media; // Brushes 사용을 위해 추가
using System.Windows.Threading;
using Color = System.Drawing.Color;

namespace Project
{
    public partial class Form1 : Form
    {
        private Timer _timer;
        private Random _random = new Random();

        // 모든 차트가 공유할 마스터 데이터 소스
        public ChartValues<double> TempValues { get; set; }
        public ChartValues<double> HumidityValues { get; set; }
        public ChartValues<double> DustValues { get; set; }
        public ChartValues<double> CO2Values { get; set; }

        public Form1()
        {
            InitializeComponent();

            // 마스터 데이터 소스 초기화
            TempValues = new ChartValues<double>();
            HumidityValues = new ChartValues<double>();
            DustValues = new ChartValues<double>();
            CO2Values = new ChartValues<double>();
            // 차트 설정
            InitializeAllCharts();



            // 실시간 시뮬레이션 시작
            _timer = new Timer { Interval = 2000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void InitializeAllCharts()
        {
            // --- 1. 종합 현황 탭의 4개 차트 설정 ---
            chart_Overall_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도", Values = TempValues, PointGeometry = null } };
            chart_Overall_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도", Values = HumidityValues, PointGeometry = null } };
            chart_Overall_Dust.Series = new SeriesCollection { new LineSeries { Title = "미세먼지", Values = DustValues, PointGeometry = null } };
            chart_Overall_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소", Values = CO2Values, PointGeometry = null } };

            // --- 2. 각 상세 탭의 차트 설정 (기준선 포함) ---

            // 온도 상세 차트 (기준: 22.5 ~ 23.5°C)
            chart_Detail_Temp.Series = new SeriesCollection { new LineSeries { Title = "온도 (°C)", Values = TempValues } };
            chart_Detail_Temp.AxisY.Add(new Axis
            {
                Title = "°C",
                Sections = new SectionsCollection {
                    new AxisSection { Value = 22.5, Stroke = System.Windows .Media.Brushes.Gold, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 4 }) },
                    new AxisSection { Value = 23.5, Stroke = System.Windows.Media.Brushes.Gold, StrokeThickness = 2, StrokeDashArray = new DoubleCollection(new double[] { 4 }) }
                }
            });

            // 습도 상세 차트 (기준: 40 ~ 50%)
            chart_Detail_Humidity.Series = new SeriesCollection { new LineSeries { Title = "습도 (%)", Values = HumidityValues } };
            chart_Detail_Humidity.AxisY.Add(new Axis
            {
                Title = "%",
                Sections = new SectionsCollection {
                    new AxisSection { Value = 40, Stroke = System.Windows.Media.Brushes.DodgerBlue, StrokeThickness = 2 },
                    new AxisSection { Value = 50, Stroke = System.Windows.Media.Brushes.DodgerBlue, StrokeThickness = 2 }
                }
            });

            // 미세먼지 상세 차트 (기준: 1000개 미만)
            chart_Detail_Dust.Series = new SeriesCollection { new ColumnSeries { Title = "미세먼지 입자 (개/m³ @0.1µm)", Values = DustValues } };
            chart_Detail_Dust.AxisY.Add(new Axis
            {
                Title = "Particle Count",
                Sections = new SectionsCollection {
                    new AxisSection { Value = 0, Fill = new SolidColorBrush { Color = Colors.Green, Opacity = 0.2 } },
                    new AxisSection { Value = 1000, Fill = new SolidColorBrush { Color = Colors.Orange, Opacity = 0.2 } },
                    new AxisSection { Value = 1500, Fill = new SolidColorBrush { Color = Colors.Red, Opacity = 0.2 } }
                }
            });

            // 이산화탄소 상세 차트 (기준: 1000ppm 미만)
            chart_Detail_CO2.Series = new SeriesCollection { new LineSeries { Title = "이산화탄소 (ppm)", Values = CO2Values } };
            chart_Detail_CO2.AxisY.Add(new Axis
            {
                Title = "ppm",
                Sections = new SectionsCollection {
                    new AxisSection { Value = 1000, Stroke = System.Windows.Media.Brushes.Red, StrokeThickness = 2 }
                }
            });
        }
        private void AddChartTitles()
        {
            // 종합 현황 탭의 차트들에 제목 추가
            AddTitleToChart(chart_Overall_Temp, "온도 (°C)");
            AddTitleToChart(chart_Overall_Humidity, "습도 (%)");
            AddTitleToChart(chart_Overall_Dust, "미세먼지 (개/m³)");
            AddTitleToChart(chart_Overall_CO2, "이산화탄소 (ppm)");

            // 상세 탭 차트들에 제목 추가
            AddTitleToChart(chart_Detail_Temp, "온도 상세 모니터링 (기준: 22.5~23.5°C)");
            AddTitleToChart(chart_Detail_Humidity, "습도 상세 모니터링 (기준: 40~50%)");
            AddTitleToChart(chart_Detail_Dust, "미세먼지 상세 모니터링 (기준: <1000개/m³)");
            AddTitleToChart(chart_Detail_CO2, "이산화탄소 상세 모니터링 (기준: <1000ppm)");
        }

        private void AddTitleToChart(LiveCharts.WinForms.CartesianChart chart, string title)
        {
            // 차트의 부모 컨트롤에 Label 추가
            Label titleLabel = new Label
            {
                Text = title,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 25,
                BackColor = System.Drawing.Color.LightGray,
                ForeColor = System.Drawing.Color.Black
            };

            // 차트의 Dock을 Fill에서 변경
            chart.Dock = DockStyle.Fill;

            // 부모 컨트롤에 Label 추가
            chart.Parent.Controls.Add(titleLabel);
           // titleLabel.BringToFront(); // Label을 맨 위로
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // 실시간 데이터 시뮬레이션
            double temp = 23 + (_random.NextDouble() - 0.5) * 1.5;
            double humidity = 45 + (_random.NextDouble() - 0.5) * 10;
            double dust = _random.Next(100, 1800);
            double co2 = _random.Next(450, 1100);

            // 마스터 데이터 소스에 값 추가
            TempValues.Add(Math.Round(temp, 2));
            HumidityValues.Add(Math.Round(humidity, 1));
            DustValues.Add(dust);
            CO2Values.Add(co2);

            // 오래된 데이터 제거 (차트 스크롤 효과)
            int maxPoints = 30;
            if (TempValues.Count > maxPoints)
            {
                TempValues.RemoveAt(0);
                HumidityValues.RemoveAt(0);
                DustValues.RemoveAt(0);
                CO2Values.RemoveAt(0);
            }
            //CheckAllStatuses();
        }

        //private void CheckAllStatuses()
        //{
        //    // --- 온도 감시 (기준: 22.5 ~ 23.5°C) ---
        //    double currentTemp = TempValues[TempValues.Count - 1]; // 가장 최신 값
        //    if (currentTemp < 22.5 || currentTemp > 23.5)
        //    {
        //        panel1.BackColor = Color.IndianRed; // 배경색 변경
        //        label_TempStatus.Text = "상태: 온도 이탈!";
        //        label_TempStatus.ForeColor = System.Drawing.Color.White;
        //    }
        //    else
        //    {
        //        panel1.BackColor = SystemColors.Control; // 원래 배경색
        //        label_TempStatus.Text = "상태: 정상";
        //        label_TempStatus.ForeColor = System.Drawing.Color.Black;
        //    }

        //    // --- 습도 감시 (기준: 40 ~ 50%) ---
        //    double currentHumidity = HumidityValues[HumidityValues.Count - 1];
        //    if (currentHumidity < 40 || currentHumidity > 50)
        //    {
        //        panel2.BackColor = Color.IndianRed;
        //        label_HumidityStatus.Text = "상태: 습도 이탈!";
        //        label_HumidityStatus.ForeColor = Color.White;
        //    }
        //    else
        //    {
        //        panel2.BackColor = SystemColors.Control;
        //        label_HumidityStatus.Text = "상태: 정상";
        //        label_HumidityStatus.ForeColor = Color.Black;
        //    }

        //    // --- 미세먼지 감시 (기준: 1000개 미만) ---
        //    double currentDust = DustValues[DustValues.Count - 1];
        //    if (currentDust >= 1000)
        //    {
        //        panel3.BackColor = Color.IndianRed;
        //        label_DustStatus.Text = "상태: 미세먼지 초과!";
        //        label_DustStatus.ForeColor = Color.White;
        //    }
        //    else
        //    {
        //        panel3.BackColor = SystemColors.Control;
        //        label_DustStatus.Text = "상태: 정상";
        //        label_DustStatus.ForeColor = Color.Black;
        //    }

        //    // --- 이산화탄소 감시 (기준: 1000ppm 미만) ---
        //    double currentCO2 = CO2Values[CO2Values.Count - 1];
        //    if (currentCO2 >= 1000)
        //    {
        //        panel4.BackColor = Color.IndianRed;
        //        label_CO2Status.Text = "상태: CO2 초과!";
        //        label_CO2Status.ForeColor = Color.White;
        //    }
        //    else
        //    {
        //        panel4.BackColor = SystemColors.Control;
        //        label_CO2Status.Text = "상태: 정상";
        //        label_CO2Status.ForeColor = Color.Black;
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            // 제목
            AddChartTitles();
        }

        private void label_Temp_Click(object sender, EventArgs e)
        {

        }

        private void label_Humidity_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        // 설정 탭
        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("--- 적용 버튼 클릭됨 ---");

            // 방어 코드
            if (cmbTargetChart.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("차트 타입을 선택해주세요.");
                return;
            }

            string selectedType = cmbTargetChart.SelectedItem.ToString();

            // 모든 차트들을 동일한 타입으로 변경
            ChangeChartType(chart_Detail_Temp, TempValues, "온도", selectedType);
            ChangeChartType(chart_Detail_Humidity, HumidityValues, "습도", selectedType);
            ChangeChartType(chart_Detail_Dust, DustValues, "미세먼지", selectedType);
            ChangeChartType(chart_Detail_CO2, CO2Values, "이산화탄소", selectedType);

            // 종합 현황 탭 차트들도 같이 변경 (선택사항)
            ChangeChartType(chart_Overall_Temp, TempValues, "온도", selectedType);
            ChangeChartType(chart_Overall_Humidity, HumidityValues, "습도", selectedType);
            ChangeChartType(chart_Overall_Dust, DustValues, "미세먼지", selectedType);
            ChangeChartType(chart_Overall_CO2, CO2Values, "이산화탄소", selectedType);

            Debug.WriteLine($"모든 차트가 {selectedType}로 변경되었습니다.");
            System.Windows.MessageBox.Show($"모든 차트가 {selectedType}로 변경되었습니다!");
        }

        // 개별 차트 타입을 변경하는 헬퍼 메서드
        private void ChangeChartType(LiveCharts.WinForms.CartesianChart chart, ChartValues<double> values, string title, string chartType)
        {
            ISeriesView newSeries;

            switch (chartType)
            {
                case "컬럼 차트":
                    newSeries = new ColumnSeries
                    {
                        Title = title,
                        Values = values
                    };
                    break;
                case "계단식 라인":
                    newSeries = new StepLineSeries
                    {
                        Title = title,
                        Values = values
                    };
                    break;
                case "라인 차트":
                default:
                    newSeries = new LineSeries
                    {
                        Title = title,
                        Values = values,
                        PointGeometry = null // 점 없는 깔끔한 라인
                    };
                    break;
            }

            // 기존 시리즈 교체
            chart.Series.Clear();
            chart.Series.Add(newSeries);
        }

        private void cmbTargetChart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart_Overall_CO2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}