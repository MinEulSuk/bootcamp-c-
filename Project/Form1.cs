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

        // --- ▼ [수정] EWMA 관련 필드 제거 ---
        // private double _ewmaTemp = -999;
        // ... (ewmaHumidity, ewmaDust, ewmaCO2)
        // --- ▲ [수정] EWMA 관련 필드 제거 ---

        // --- ▼ 설정값 필드 ---
        // --- [수정] EWMA 설정 필드 제거 ---
        // private bool _showEwma = false;
        // private double _ewmaAlpha = 0.3;
        private float _co2Threshold = 1700;
        private float _tempMin = 18;
        private float _tempMax = 26;
        private float _humidityMin = 30;
        private float _humidityMax = 75;
        private float _pm25Threshold = 10;
        // --- ▲ 설정값 필드 ---

        private bool _isTempWarningActive = false;
        private bool _isHumidityWarningActive = false;
        private bool _isDustWarningActive = false;
        private bool _isCo2WarningActive = false;

        public ChartValues<DateTimePoint> TempValues { get; set; }
        public ChartValues<DateTimePoint> HumidityValues { get; set; }
        public ChartValues<DateTimePoint> DustValues { get; set; }
        public ChartValues<DateTimePoint> CO2Values { get; set; }

        // --- ▼ [수정] EWMA 차트 값 제거 ---
        // public ChartValues<DateTimePoint> EwmaTempValues { get; set; }
        // ... (EwmaHumidityValues, EwmaDustValues, EwmaCO2Values)
        // --- ▲ [수정] EWMA 차트 값 제거 ---

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

            // --- [수정] EWMA 차트 값 초기화 제거 ---

            InitializeChartMappers();

            // 설정 탭 UI에 기본값 로드 및 이벤트 연결
            LoadSettingsToUI();
            try
            {
                // [수정] trackBarAlpha 이벤트 연결 제거
                if (this.btnApplySettings != null)
                {
                    this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 탭 컨트롤이 Form1.Designer.cs에 추가되었는지 확인하세요.\n" + ex.Message);
            }


            _timer = new Timer();
            _timer.Interval = _currentIntervalSeconds * 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            InitializeAllCharts();
            ApplyChartSettings(30);
        }

        // --- ▼ 설정 UI 관련 메서드 ---

        /// <summary>
        /// 현재 설정 필드 값을 설정 탭의 UI 컨트롤에 로드합니다.
        /// </summary>
        private void LoadSettingsToUI()
        {
            try
            {
                // --- [수정] EWMA 컨트롤 로직 제거 ---
                // if (chkShowEwma != null)
                //    chkShowEwma.Checked = _showEwma;
                // if (trackBarAlpha != null)
                // ...
                // if (lblAlphaValue != null)
                // ...
                // --- ▲ [수정] ---

                if (numCo2 != null)
                    numCo2.Value = (decimal)_co2Threshold;

                if (numTempMin != null)
                    numTempMin.Value = (decimal)_tempMin;

                if (numTempMax != null)
                    numTempMax.Value = (decimal)_tempMax;

                if (numHumidityMin != null)
                    numHumidityMin.Value = (decimal)_humidityMin;

                if (numHumidityMax != null)
                    numHumidityMax.Value = (decimal)_humidityMax;

                if (numPm25 != null)
                    numPm25.Value = (decimal)_pm25Threshold;
            }
            catch (Exception ex)
            {
                Console.WriteLine("설정 UI 로드 실패: " + ex.Message);
            }
        }

        /// <summary>
        /// "설정 적용" 버튼 클릭 이벤트 핸들러
        /// </summary>
        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            try
            {
                // UI 컨트롤의 값을 private 필드로 저장
                // --- [수정] EWMA 컨트롤 로직 제거 ---
                // _showEwma = chkShowEwma.Checked;
                // _ewmaAlpha = trackBarAlpha.Value / 100.0;
                // --- ▲ [수정] ---

                if (numCo2 != null)
                    _co2Threshold = (float)numCo2.Value;
                if (numTempMin != null)
                    _tempMin = (float)numTempMin.Value;
                if (numTempMax != null)
                    _tempMax = (float)numTempMax.Value;
                if (numHumidityMin != null)
                    _humidityMin = (float)numHumidityMin.Value;
                if (numHumidityMax != null)
                    _humidityMax = (float)numHumidityMax.Value;
                if (numPm25 != null)
                    _pm25Threshold = (float)numPm25.Value;

                // 변경된 설정을 차트에 즉시 반영 (임계선)
                InitializeAllCharts();

                MessageBox.Show("설정이 적용되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정 적용 실패: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- [수정] trackBarAlpha_Scroll 메서드 제거 ---

        // --- ▲ 설정 UI 관련 메서드 ---


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_dbManager == null) return;

            SensorData data = _dbManager.GetLatestSensorData();
            if (data != null)
            {
                DateTime now = DateTime.Now;
                // [수정] UpdateStatusUI가 먼저 호출되도록 순서 유지 (경고 판단)
                UpdateStatusUI(data, now);
                UpdateCharts(data, now);
            }

            UpdateAxisLimits();
        }

        private void UpdateCharts(SensorData data, DateTime now)
        {
            if (data.Temperature.HasValue) TempValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Temperature.Value)));
            if (data.Humidity.HasValue) HumidityValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Humidity.Value)));
            if (data.Pm2_5.HasValue) DustValues.Add(new DateTimePoint(now, Convert.ToDouble(data.Pm2_5.Value)));
            if (data.Co2Ppm.HasValue) CO2Values.Add(new DateTimePoint(now, Convert.ToDouble(data.Co2Ppm.Value)));

            // --- [수정] EWMA 값 차트 데이터에 추가 로직 제거 ---

            // 원본 데이터 정리
            while (TempValues.Count > _maxChartPoints) TempValues.RemoveAt(0);
            while (HumidityValues.Count > _maxChartPoints) HumidityValues.RemoveAt(0);
            while (DustValues.Count > _maxChartPoints) DustValues.RemoveAt(0);
            while (CO2Values.Count > _maxChartPoints) CO2Values.RemoveAt(0);

            // --- [수정] EWMA 데이터 정리 로직 제거 ---
        }

        private void UpdateStatusUI(SensorData data, DateTime now)
        {
            bool isAnyWarning = false;

            // [수정] 온도 (원본 값 기준)
            if (data.Temperature.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Temperature.Value);
                // --- [수정] EWMA 계산 로직 제거 ---

                if (currentValue < _tempMin || currentValue > _tempMax)
                {
                    if (panel1 != null) panel1.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isTempWarningActive)
                    {
                        _isTempWarningActive = true;
                        string msg = currentValue < _tempMin ? "온도 하한선 이탈" : "온도 상한선 초과";
                        _dbManager?.LogWarning("온도 경고", (float)currentValue, msg); // [수정] (EWMA) 제거
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    if (panel1 != null) panel1.BackColor = System.Drawing.Color.White;
                    if (_isTempWarningActive)
                    {
                        _isTempWarningActive = false;
                        _dbManager?.LogCommand("FAN_OFF");
                    }
                }
            }

            // [수정] 습도 (원본 값 기준)
            if (data.Humidity.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Humidity.Value);
                // --- [수정] EWMA 계산 로직 제거 ---

                if (currentValue < _humidityMin || currentValue > _humidityMax)
                {
                    if (panel2 != null) panel2.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isHumidityWarningActive)
                    {
                        _isHumidityWarningActive = true;
                        string msg = currentValue < _humidityMin ? "습도 하한선 이탈" : "습도 상한선 초과";
                        _dbManager?.LogWarning("습도 경고", (float)currentValue, msg); // [수정] (EWMA) 제거
                    }
                }
                else
                {
                    if (panel2 != null) panel2.BackColor = System.Drawing.Color.White;
                    if (_isHumidityWarningActive) _isHumidityWarningActive = false;
                }
            }

            // [수정] 미세먼지 (원본 값 기준)
            if (data.Pm2_5.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Pm2_5.Value);
                // --- [수정] EWMA 계산 로직 제거 ---

                if (currentValue >= _pm25Threshold)
                {
                    if (panel3 != null) panel3.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isDustWarningActive)
                    {
                        _isDustWarningActive = true;
                        _dbManager?.LogWarning("미세먼지 경고", (float)currentValue, "기준치 초과"); // [수정] (EWMA) 제거
                    }
                }
                else
                {
                    if (panel3 != null) panel3.BackColor = System.Drawing.Color.White;
                    if (_isDustWarningActive) _isDustWarningActive = false;
                }
            }

            // [수정] CO2 (원본 값 기준)
            if (data.Co2Ppm.HasValue)
            {
                double currentValue = Convert.ToDouble(data.Co2Ppm.Value);
                // --- [수정] EWMA 계산 로직 제거 ---

                if (currentValue >= _co2Threshold)
                {
                    if (panel4 != null) panel4.BackColor = System.Drawing.Color.IndianRed;
                    isAnyWarning = true;
                    if (!_isCo2WarningActive)
                    {
                        _isCo2WarningActive = true;
                        _dbManager?.LogWarning("CO2 경고", (float)currentValue, "기준치 초과"); // [수정] (EWMA) 제거
                        _dbManager?.LogCommand("FAN_ON");
                    }
                }
                else
                {
                    if (panel4 != null) panel4.BackColor = System.Drawing.Color.White;
                    if (_isCo2WarningActive) _isCo2WarningActive = false;
                }
            }

            // ... (폼 타이틀 업데이트 로직은 동일) ...
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
            var xAxis = new Axis
            {
                Title = "",
                Labels = null,
                LabelFormatter = null,
                ShowLabels = false
            };

            // [수정] setupChart 람다에서 ewmaValues 파라미터 제거
            Action<LiveCharts.WinForms.CartesianChart, ChartValues<DateTimePoint>, string, Axis, double[]> setupChart =
                (chart, values, title, yAxis, thresholds) =>
                {
                    if (chart == null) return;

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

                    // --- [수정] EWMA 시리즈 추가 로직 제거 ---

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

            // [수정] setupChart 호출 시 EWMA 값 전달 제거
            setupChart(chart_Overall_Temp, TempValues, "온도", CreateStyledYAxis("온도 (°C)"), new double[] { _tempMin, _tempMax });
            setupChart(chart_Overall_Humidity, HumidityValues, "습도", CreateStyledYAxis("습도 (%)"), new double[] { _humidityMin, _humidityMax });
            setupChart(chart_Overall_Dust, DustValues, "미세먼지", CreateStyledYAxis("PM2.5 (μg/m³)"), new double[] { _pm25Threshold });
            setupChart(chart_Overall_CO2, CO2Values, "이산화탄소", CreateStyledYAxis("CO2 (ppm)"), new double[] { _co2Threshold });

            setupChart(chart_Detail_Temp, TempValues, "온도", CreateStyledYAxis("온도 (°C)"), new double[] { _tempMin, _tempMax });
            setupChart(chart_Detail_Humidity, HumidityValues, "습도", CreateStyledYAxis("습도 (%)"), new double[] { _humidityMin, _humidityMax });
            setupChart(chart_Detail_Dust, DustValues, "미세먼지", CreateStyledYAxis("PM2.5 (μg/m³)"), new double[] { _pm25Threshold });
            setupChart(chart_Detail_CO2, CO2Values, "이산화탄소", CreateStyledYAxis("CO2 (ppm)"), new double[] { _co2Threshold });
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

            if (chart_Overall_Temp?.AxisX?.Count > 0) setAxisLimits(chart_Overall_Temp.AxisX[0]);
            if (chart_Overall_Humidity?.AxisX?.Count > 0) setAxisLimits(chart_Overall_Humidity.AxisX[0]);
            if (chart_Overall_Dust?.AxisX?.Count > 0) setAxisLimits(chart_Overall_Dust.AxisX[0]);
            if (chart_Overall_CO2?.AxisX?.Count > 0) setAxisLimits(chart_Overall_CO2.AxisX[0]);

            if (chart_Detail_Temp?.AxisX?.Count > 0) setAxisLimits(chart_Detail_Temp.AxisX[0]);
            if (chart_Detail_Humidity?.AxisX?.Count > 0) setAxisLimits(chart_Detail_Humidity.AxisX[0]);
            if (chart_Detail_Dust?.AxisX?.Count > 0) setAxisLimits(chart_Detail_Dust.AxisX[0]);
            if (chart_Detail_CO2?.AxisX?.Count > 0) setAxisLimits(chart_Detail_CO2.AxisX[0]);
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

            // --- [수정] EWMA 데이터 정리 로직 제거 ---

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