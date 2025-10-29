using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using System.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts.Configurations;

namespace Project
{
    public partial class Form1 : Form
    {
        private Timer _timer;
        private DatabaseManager _dbManager;

        private double _ewmaTemp = -999;
        private double _ewmaHumidity = -999;
        private double _ewmaDust = -999;
        private double _ewmaCO2 = -999;
        private const double EWMA_ALPHA = 0.3;

        private bool _isTempWarningActive = false;
        private bool _isHumidityWarningActive = false;
        private bool _isDustWarningActive = false;
        private bool _isCo2WarningActive = false;

        public ChartValues<DateTimePoint> TempValues { get; set; }
        public ChartValues<DateTimePoint> HumidityValues { get; set; }
        public ChartValues<DateTimePoint> DustValues { get; set; }
        public ChartValues<DateTimePoint> CO2Values { get; set; }

        private int _maxChartPoints;
        private int _currentIntervalSeconds = 1;
        private int _currentMaxDurationSeconds = 30;

        public Form1()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                _dbManager = new DatabaseManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 연결 실패: " + ex.Message);
                Environment.Exit(1);
            }

            TempValues = new ChartValues<DateTimePoint>();
            HumidityValues = new ChartValues<DateTimePoint>();
            DustValues = new ChartValues<DateTimePoint>();
            CO2Values = new ChartValues<DateTimePoint>();

            InitializeChartMappers();
            InitializeAllCharts();

            _timer = new Timer();
            _timer.Interval = _currentIntervalSeconds * 1000;
            ApplyChartSettings(30);

            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_dbManager == null) return;

            SensorData data = _dbManager.GetLatestSensorData();
            if (data != null)
            {
                DateTime now = DateTime.Now;
                UpdateCharts(data, now);
                UpdateStatusUI(data, now);
            }

            UpdateAxisLimits();
        }

        private void UpdateCharts(SensorData data, DateTime now)
        {
            if (data.Temperature.HasValue) TempValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Temperature.Value)));
            if (data.Humidity.HasValue) HumidityValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Humidity.Value)));
            if (data.Pm2_5.HasValue) DustValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Pm2_5.Value)));
            if (data.Co2Ppm.HasValue) CO2Values.Add(new DateTimePoint(now, Convert.ToDouble(data.Co2Ppm.Value)));

            while (TempValues.Count > _maxChartPoints) TempValues.RemoveAt(0);
            while (HumidityValues.Count > _maxChartPoints) HumidityValues.RemoveAt(0);
            while (DustValues.Count > _maxChartPoints) DustValues.RemoveAt(0);
            while (CO2Values.Count > _maxChartPoints) CO2Values.RemoveAt(0);
        }

        private void UpdateStatusUI(SensorData data, DateTime now)
        {
            bool isAnyWarning = false;

            // EWMA 온도
            if (data.Temperature.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Temperature.Value);
                if (_ewmaTemp == -999) _ewmaTemp = currentValue;
                else _ewmaTemp = (EWMA_ALPHA * currentValue) + ((1 - EWMA_ALPHA) * _ewmaTemp);

                if (_ewmaTemp < 22.5 || _ewmaTemp > 23.5)
                {
                    panel1.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isTempWarningActive)
                    {
                        _isTempWarningActive = true;
                        string msg = _ewmaTemp < 22.5 ? "EWMA 하한선 이탈" : "EWMA 상한선 초과";
                        _dbManager.LogWarning("온도 경고 (EWMA)", (float)_ewmaTemp, msg);
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    panel1.BackColor = System.Drawing.Color.White;
                    if (_isTempWarningActive)
                    {
                        _isTempWarningActive = false;
                        _dbManager?.LogCommand("FAN_OFF");
                    }
                }
            }

            // EWMA 습도
            if (data.Humidity.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Humidity.Value);
                if (_ewmaHumidity == -999) _ewmaHumidity = currentValue;
                else _ewmaHumidity = (EWMA_ALPHA * currentValue) + ((1 - EWMA_ALPHA) * _ewmaHumidity);

                if (_ewmaHumidity < 40 || _ewmaHumidity > 50)
                {
                    panel2.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isHumidityWarningActive)
                    {
                        _isHumidityWarningActive = true;
                        string msg = _ewmaHumidity < 40 ? "EWMA 하한선 이탈" : "EWMA 상한선 초과";
                        _dbManager.LogWarning("습도 경고 (EWMA)", (float)_ewmaHumidity, msg);
                    }
                }
                else
                {
                    panel2.BackColor = System.Drawing.Color.White;
                    if (_isHumidityWarningActive) _isHumidityWarningActive = false;
                }
            }

            // EWMA 미세먼지
            if (data.Pm2_5.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Pm2_5.Value);
                if (_ewmaDust == -999) _ewmaDust = currentValue;
                else _ewmaDust = (EWMA_ALPHA * currentValue) + ((1 - EWMA_ALPHA) * _ewmaDust);

                if (_ewmaDust >= 35)
                {
                    panel3.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isDustWarningActive)
                    {
                        _isDustWarningActive = true;
                        _dbManager.LogWarning("미세먼지 경고 (EWMA)", (float)_ewmaDust, "EWMA 기준치 초과");
                    }
                }
                else
                {
                    panel3.BackColor = System.Drawing.Color.White;
                    if (_isDustWarningActive) _isDustWarningActive = false;
                }
            }

            // EWMA CO2
            if (data.Co2Ppm.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Co2Ppm.Value);
                if (_ewmaCO2 == -999) _ewmaCO2 = currentValue;
                else _ewmaCO2 = (EWMA_ALPHA * currentValue) + ((1 - EWMA_ALPHA) * _ewmaCO2);

                if (_ewmaCO2 >= 1000)
                {
                    panel4.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isCo2WarningActive)
                    {
                        _isCo2WarningActive = true;
                        _dbManager.LogWarning("CO2 경고 (EWMA)", (float)_ewmaCO2, "EWMA 기준치 초과");
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    panel4.BackColor = System.Drawing.Color.White;
                    if (_isCo2WarningActive) _isCo2WarningActive = false;
                }
            }

            string readableTime = "";
            switch (_currentMaxDurationSeconds)
            {
                case 30:
                    readableTime = "30초 보기";
                    break;
                case 60:
                    readableTime = "1분 보기";
                    break;
                case 180:
                    readableTime = "3분 보기";
                    break;
                case 600:
                    readableTime = "10분 보기";
                    break;
                case 1800:
                    readableTime = "30분 보기";
                    break;
                case 3600:
                    readableTime = "1시간 보기";
                    break;
                default:
                    readableTime = _currentMaxDurationSeconds + "초 보기";
                    break;
            }


            string baseTitle = isAnyWarning ? "!! 경고 !! - 클린룸 통합 모니터링" : "클린룸 통합 모니터링";
            this.Text = $"{baseTitle} - [{readableTime}]";
        }

        private void InitializeChartMappers()
        {
            var mapper = Mappers.Xy<DateTimePoint>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value);
            Charting.For<DateTimePoint>(mapper);
        }

        private Axis CreateStyledYAxis(string title)
        {
            return new Axis
            {
                Title = title,
                Foreground = Brushes.Gray,
                FontSize = 11,
                Separator = new Separator
                {
                    Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(220, 220, 220)),
                    StrokeThickness = 0.5
                }
            };
        }

        private void InitializeAllCharts()
        {
            var transparentRed = new SolidColorBrush(System.Windows.Media.Color.FromArgb(60, 255, 0, 0));

            var tempSections = new SectionsCollection
            {
                new AxisSection { Value = 23.5, SectionWidth = 100, Fill = transparentRed },
                new AxisSection { Value = -100, SectionWidth = 122.5, Fill = transparentRed }
            };
            var humSections = new SectionsCollection
            {
                new AxisSection { Value = 50, SectionWidth = 100, Fill = transparentRed },
                new AxisSection { Value = -100, SectionWidth = 140, Fill = transparentRed }
            };
            var dustSections = new SectionsCollection
            {
                new AxisSection { Value = 35, SectionWidth = 1000, Fill = transparentRed }
            };
            var co2Sections = new SectionsCollection
            {
                new AxisSection { Value = 1000, SectionWidth = 10000, Fill = transparentRed }
            };

            // X축 시간 라벨 제거
            var xAxis = new Axis
            {
                Title = "",
                Labels = null,
                LabelFormatter = null,
                ShowLabels = false
            };

            Action<LiveCharts.WinForms.CartesianChart, ChartValues<DateTimePoint>, string, Axis, double[]> setupChart =
                (chart, values, title, yAxis, thresholds) =>
                {
                    var series = new SeriesCollection
                    {
                        new LineSeries
                        {
                            Title = title,
                            Values = values,
                            PointGeometry = null,
                            StrokeThickness = 1.5,
                            Fill = Brushes.Transparent,
                            Stroke = Brushes.DodgerBlue
                        }
                    };

                    // 기준선들 (고정 표시)
                    foreach (var t in thresholds)
                    {
                        series.Add(new LineSeries
                        {
                            Title = $"기준선 ({t})",
                            Values = new ChartValues<DateTimePoint>
                            {
                                new DateTimePoint(DateTime.Now.AddHours(-1), t),
                                new DateTimePoint(DateTime.Now.AddHours(1), t)
                            },
                            Stroke = Brushes.IndianRed,
                            Fill = Brushes.Transparent,
                            StrokeDashArray = new DoubleCollection { 4 },
                            StrokeThickness = 1
                        });
                    }

                    chart.Series = series;
                    chart.AxisX.Clear();
                    chart.AxisY.Clear();
                    chart.AxisX.Add(xAxis);
                    chart.AxisY.Add(yAxis);
                    chart.LegendLocation = LegendLocation.Bottom;
                    chart.DisableAnimations = true;
                    chart.Hoverable = false;
                    chart.DataTooltip = null;
                };

            setupChart(chart_Overall_Temp, TempValues, "온도", CreateStyledYAxis("온도 (°C)"), new double[] { 22.5, 23.5 });
            setupChart(chart_Overall_Humidity, HumidityValues, "습도", CreateStyledYAxis("습도 (%)"), new double[] { 40, 50 });
            setupChart(chart_Overall_Dust, DustValues, "미세먼지", CreateStyledYAxis("PM2.5 (μg/m³)"), new double[] { 35 });
            setupChart(chart_Overall_CO2, CO2Values, "이산화탄소", CreateStyledYAxis("CO2 (ppm)"), new double[] { 1000 });

            try
            {
                setupChart(chart_Detail_Temp, TempValues, "온도", CreateStyledYAxis("온도 (°C)"), new double[] { 22.5, 23.5 });
                setupChart(chart_Detail_Humidity, HumidityValues, "습도", CreateStyledYAxis("습도 (%)"), new double[] { 40, 50 });
                setupChart(chart_Detail_Dust, DustValues, "미세먼지", CreateStyledYAxis("PM2.5 (μg/m³)"), new double[] { 35 });
                setupChart(chart_Detail_CO2, CO2Values, "이산화탄소", CreateStyledYAxis("CO2 (ppm)"), new double[] { 1000 });
            }
            catch (Exception ex)
            {
                Console.WriteLine("상세 차트 연결 실패: " + ex.Message);
            }
        }

        private void UpdateAxisLimits()
        {
            if (_timer == null || !_timer.Enabled) return;

            DateTime now = DateTime.Now;
            DateTime startTime = now.AddSeconds(-_currentMaxDurationSeconds);

            Action<Axis> setAxisLimits = (axis) =>
            {
                axis.MaxValue = now.Ticks;
                axis.MinValue = startTime.Ticks;
            };

            setAxisLimits(chart_Overall_Temp.AxisX[0]);
            setAxisLimits(chart_Overall_Humidity.AxisX[0]);
            setAxisLimits(chart_Overall_Dust.AxisX[0]);
            setAxisLimits(chart_Overall_CO2.AxisX[0]);

            try
            {
                setAxisLimits(chart_Detail_Temp.AxisX[0]);
                setAxisLimits(chart_Detail_Humidity.AxisX[0]);
                setAxisLimits(chart_Detail_Dust.AxisX[0]);
                setAxisLimits(chart_Detail_CO2.AxisX[0]);
            }
            catch { }
        }

        private void ApplyChartSettings(int newMaxDurationSeconds)
        {
            if (_timer == null) return;

            _currentMaxDurationSeconds = newMaxDurationSeconds;
            _maxChartPoints = newMaxDurationSeconds / _currentIntervalSeconds;

            while (TempValues.Count > _maxChartPoints) TempValues.RemoveAt(0);
            while (HumidityValues.Count > _maxChartPoints) HumidityValues.RemoveAt(0);
            while (DustValues.Count > _maxChartPoints) DustValues.RemoveAt(0);
            while (CO2Values.Count > _maxChartPoints) CO2Values.RemoveAt(0);

            UpdateAxisLimits();
        }

        private void btnSet30s_Click(object sender, EventArgs e) => ApplyChartSettings(30);
        private void btnSet60s_Click(object sender, EventArgs e) => ApplyChartSettings(60);
        private void btnSet180s_Click(object sender, EventArgs e) => ApplyChartSettings(180);
        private void btnSet600s_Click(object sender, EventArgs e) => ApplyChartSettings(600);
        private void btnSet1800s_Click(object sender, EventArgs e) => ApplyChartSettings(1800);
        private void btnSet3600s_Click(object sender, EventArgs e) => ApplyChartSettings(3600);
    }
}
