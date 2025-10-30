namespace Project
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Overall = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart_Overall_Temp = new LiveCharts.WinForms.CartesianChart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart_Overall_Humidity = new LiveCharts.WinForms.CartesianChart();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chart_Overall_Dust = new LiveCharts.WinForms.CartesianChart();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chart_Overall_CO2 = new LiveCharts.WinForms.CartesianChart();
            this.tabPage_Temperature = new System.Windows.Forms.TabPage();
            this.chart_Detail_Temp = new LiveCharts.WinForms.CartesianChart();
            this.tabPage_Humidity = new System.Windows.Forms.TabPage();
            this.chart_Detail_Humidity = new LiveCharts.WinForms.CartesianChart();
            this.tabPage_Dust = new System.Windows.Forms.TabPage();
            this.chart_Detail_Dust = new LiveCharts.WinForms.CartesianChart();
            this.tabPage_CO2 = new System.Windows.Forms.TabPage();
            this.chart_Detail_CO2 = new LiveCharts.WinForms.CartesianChart();
            this.tabPage_Settings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSet3600s = new System.Windows.Forms.Button();
            this.btnSet30s = new System.Windows.Forms.Button();
            this.btnSet1800s = new System.Windows.Forms.Button();
            this.btnSet60s = new System.Windows.Forms.Button();
            this.btnSet600s = new System.Windows.Forms.Button();
            this.btnSet180s = new System.Windows.Forms.Button();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numPm25 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numHumidityMax = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numHumidityMin = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numTempMax = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numTempMin = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numCo2 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage_Overall.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage_Temperature.SuspendLayout();
            this.tabPage_Humidity.SuspendLayout();
            this.tabPage_Dust.SuspendLayout();
            this.tabPage_CO2.SuspendLayout();
            this.tabPage_Settings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPm25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHumidityMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHumidityMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTempMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTempMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCo2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Overall);
            this.tabControl1.Controls.Add(this.tabPage_Temperature);
            this.tabControl1.Controls.Add(this.tabPage_Humidity);
            this.tabControl1.Controls.Add(this.tabPage_Dust);
            this.tabControl1.Controls.Add(this.tabPage_CO2);
            this.tabControl1.Controls.Add(this.tabPage_Settings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(914, 562);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_Overall
            // 
            this.tabPage_Overall.Controls.Add(this.tableLayoutPanel1);
            this.tabPage_Overall.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Overall.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Overall.Name = "tabPage_Overall";
            this.tabPage_Overall.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Overall.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Overall.TabIndex = 0;
            this.tabPage_Overall.Text = "종합 현황";
            this.tabPage_Overall.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 525);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chart_Overall_Temp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 258);
            this.panel1.TabIndex = 4;
            // 
            // chart_Overall_Temp
            // 
            this.chart_Overall_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Temp.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Temp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Temp.Name = "chart_Overall_Temp";
            this.chart_Overall_Temp.Size = new System.Drawing.Size(444, 258);
            this.chart_Overall_Temp.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chart_Overall_Humidity);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(453, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(444, 258);
            this.panel2.TabIndex = 5;
            // 
            // chart_Overall_Humidity
            // 
            this.chart_Overall_Humidity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Humidity.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Humidity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Humidity.Name = "chart_Overall_Humidity";
            this.chart_Overall_Humidity.Size = new System.Drawing.Size(444, 258);
            this.chart_Overall_Humidity.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chart_Overall_Dust);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 264);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(444, 259);
            this.panel3.TabIndex = 6;
            // 
            // chart_Overall_Dust
            // 
            this.chart_Overall_Dust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Dust.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Dust.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Dust.Name = "chart_Overall_Dust";
            this.chart_Overall_Dust.Size = new System.Drawing.Size(444, 259);
            this.chart_Overall_Dust.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chart_Overall_CO2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(453, 264);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(444, 259);
            this.panel4.TabIndex = 7;
            // 
            // chart_Overall_CO2
            // 
            this.chart_Overall_CO2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_CO2.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_CO2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_CO2.Name = "chart_Overall_CO2";
            this.chart_Overall_CO2.Size = new System.Drawing.Size(444, 259);
            this.chart_Overall_CO2.TabIndex = 3;
            // 
            // tabPage_Temperature
            // 
            this.tabPage_Temperature.Controls.Add(this.chart_Detail_Temp);
            this.tabPage_Temperature.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Temperature.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Temperature.Name = "tabPage_Temperature";
            this.tabPage_Temperature.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Temperature.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Temperature.TabIndex = 1;
            this.tabPage_Temperature.Text = "온도";
            this.tabPage_Temperature.UseVisualStyleBackColor = true;
            // 
            // chart_Detail_Temp
            // 
            this.chart_Detail_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Detail_Temp.Location = new System.Drawing.Point(3, 4);
            this.chart_Detail_Temp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Detail_Temp.Name = "chart_Detail_Temp";
            this.chart_Detail_Temp.Size = new System.Drawing.Size(900, 525);
            this.chart_Detail_Temp.TabIndex = 0;
            // 
            // tabPage_Humidity
            // 
            this.tabPage_Humidity.Controls.Add(this.chart_Detail_Humidity);
            this.tabPage_Humidity.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Humidity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Humidity.Name = "tabPage_Humidity";
            this.tabPage_Humidity.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Humidity.TabIndex = 2;
            this.tabPage_Humidity.Text = "습도";
            this.tabPage_Humidity.UseVisualStyleBackColor = true;
            // 
            // chart_Detail_Humidity
            // 
            this.chart_Detail_Humidity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Detail_Humidity.Location = new System.Drawing.Point(0, 0);
            this.chart_Detail_Humidity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Detail_Humidity.Name = "chart_Detail_Humidity";
            this.chart_Detail_Humidity.Size = new System.Drawing.Size(906, 533);
            this.chart_Detail_Humidity.TabIndex = 0;
            // 
            // tabPage_Dust
            // 
            this.tabPage_Dust.Controls.Add(this.chart_Detail_Dust);
            this.tabPage_Dust.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Dust.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_Dust.Name = "tabPage_Dust";
            this.tabPage_Dust.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Dust.TabIndex = 3;
            this.tabPage_Dust.Text = "미세먼지";
            this.tabPage_Dust.UseVisualStyleBackColor = true;
            // 
            // chart_Detail_Dust
            // 
            this.chart_Detail_Dust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Detail_Dust.Location = new System.Drawing.Point(0, 0);
            this.chart_Detail_Dust.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Detail_Dust.Name = "chart_Detail_Dust";
            this.chart_Detail_Dust.Size = new System.Drawing.Size(906, 533);
            this.chart_Detail_Dust.TabIndex = 0;
            // 
            // tabPage_CO2
            // 
            this.tabPage_CO2.Controls.Add(this.chart_Detail_CO2);
            this.tabPage_CO2.Location = new System.Drawing.Point(4, 25);
            this.tabPage_CO2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage_CO2.Name = "tabPage_CO2";
            this.tabPage_CO2.Size = new System.Drawing.Size(906, 533);
            this.tabPage_CO2.TabIndex = 4;
            this.tabPage_CO2.Text = "이산화탄소";
            this.tabPage_CO2.UseVisualStyleBackColor = true;
            // 
            // chart_Detail_CO2
            // 
            this.chart_Detail_CO2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Detail_CO2.Location = new System.Drawing.Point(0, 0);
            this.chart_Detail_CO2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Detail_CO2.Name = "chart_Detail_CO2";
            this.chart_Detail_CO2.Size = new System.Drawing.Size(906, 533);
            this.chart_Detail_CO2.TabIndex = 0;
            // 
            // tabPage_Settings
            // 
            this.tabPage_Settings.Controls.Add(this.groupBox3);
            this.tabPage_Settings.Controls.Add(this.btnApplySettings);
            this.tabPage_Settings.Controls.Add(this.groupBox2);
            this.tabPage_Settings.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Settings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Settings.Name = "tabPage_Settings";
            this.tabPage_Settings.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Settings.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Settings.TabIndex = 5;
            this.tabPage_Settings.Text = "환경설정";
            this.tabPage_Settings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSet3600s);
            this.groupBox3.Controls.Add(this.btnSet30s);
            this.groupBox3.Controls.Add(this.btnSet1800s);
            this.groupBox3.Controls.Add(this.btnSet60s);
            this.groupBox3.Controls.Add(this.btnSet600s);
            this.groupBox3.Controls.Add(this.btnSet180s);
            this.groupBox3.Location = new System.Drawing.Point(434, 15);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(359, 127);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "차트 시간 범위";
            // 
            // btnSet3600s
            // 
            this.btnSet3600s.Location = new System.Drawing.Point(243, 80);
            this.btnSet3600s.Name = "btnSet3600s";
            this.btnSet3600s.Size = new System.Drawing.Size(88, 30);
            this.btnSet3600s.TabIndex = 22;
            this.btnSet3600s.Text = "1시간 보기";
            this.btnSet3600s.UseVisualStyleBackColor = true;
            this.btnSet3600s.Click += new System.EventHandler(this.btnSet3600s_Click);
            // 
            // btnSet30s
            // 
            this.btnSet30s.Location = new System.Drawing.Point(26, 35);
            this.btnSet30s.Name = "btnSet30s";
            this.btnSet30s.Size = new System.Drawing.Size(82, 30);
            this.btnSet30s.TabIndex = 17;
            this.btnSet30s.Text = "30초 보기";
            this.btnSet30s.UseVisualStyleBackColor = true;
            this.btnSet30s.Click += new System.EventHandler(this.btnSet30s_Click);
            // 
            // btnSet1800s
            // 
            this.btnSet1800s.Location = new System.Drawing.Point(135, 80);
            this.btnSet1800s.Name = "btnSet1800s";
            this.btnSet1800s.Size = new System.Drawing.Size(88, 30);
            this.btnSet1800s.TabIndex = 21;
            this.btnSet1800s.Text = "30분 보기";
            this.btnSet1800s.UseVisualStyleBackColor = true;
            this.btnSet1800s.Click += new System.EventHandler(this.btnSet1800s_Click);
            // 
            // btnSet60s
            // 
            this.btnSet60s.Location = new System.Drawing.Point(135, 35);
            this.btnSet60s.Name = "btnSet60s";
            this.btnSet60s.Size = new System.Drawing.Size(88, 30);
            this.btnSet60s.TabIndex = 18;
            this.btnSet60s.Text = "1분 보기";
            this.btnSet60s.UseVisualStyleBackColor = true;
            this.btnSet60s.Click += new System.EventHandler(this.btnSet60s_Click);
            // 
            // btnSet600s
            // 
            this.btnSet600s.Location = new System.Drawing.Point(26, 80);
            this.btnSet600s.Name = "btnSet600s";
            this.btnSet600s.Size = new System.Drawing.Size(85, 30);
            this.btnSet600s.TabIndex = 20;
            this.btnSet600s.Text = "10분 보기";
            this.btnSet600s.UseVisualStyleBackColor = true;
            this.btnSet600s.Click += new System.EventHandler(this.btnSet600s_Click);
            // 
            // btnSet180s
            // 
            this.btnSet180s.Location = new System.Drawing.Point(243, 35);
            this.btnSet180s.Name = "btnSet180s";
            this.btnSet180s.Size = new System.Drawing.Size(88, 30);
            this.btnSet180s.TabIndex = 19;
            this.btnSet180s.Text = "3분 보기";
            this.btnSet180s.UseVisualStyleBackColor = true;
            this.btnSet180s.Click += new System.EventHandler(this.btnSet180s_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnApplySettings.Location = new System.Drawing.Point(688, 480);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(201, 39);
            this.btnApplySettings.TabIndex = 18;
            this.btnApplySettings.Text = "설정 적용";
            this.btnApplySettings.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numPm25);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numHumidityMax);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numHumidityMin);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numTempMax);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numTempMin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numCo2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(18, 15); // <-- [수정] 위치 Y값을 15로 변경
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 371);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "알림 조건";
            // 
            // numPm25
            // 
            this.numPm25.DecimalPlaces = 1;
            this.numPm25.Location = new System.Drawing.Point(219, 319);
            this.numPm25.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numPm25.Name = "numPm25";
            this.numPm25.Size = new System.Drawing.Size(149, 25);
            this.numPm25.TabIndex = 17;
            this.numPm25.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(19, 319);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 25);
            this.label7.TabIndex = 16;
            this.label7.Text = "PM2.5 기준값 (μg/m³):";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numHumidityMax
            // 
            this.numHumidityMax.DecimalPlaces = 1;
            this.numHumidityMax.Location = new System.Drawing.Point(219, 267);
            this.numHumidityMax.Name = "numHumidityMax";
            this.numHumidityMax.Size = new System.Drawing.Size(149, 25);
            this.numHumidityMax.TabIndex = 15;
            this.numHumidityMax.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(19, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(185, 25);
            this.label6.TabIndex = 14;
            this.label6.Text = "습도 최대값 (%):";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numHumidityMin
            // 
            this.numHumidityMin.DecimalPlaces = 1;
            this.numHumidityMin.Location = new System.Drawing.Point(219, 215);
            this.numHumidityMin.Name = "numHumidityMin";
            this.numHumidityMin.Size = new System.Drawing.Size(149, 25);
            this.numHumidityMin.TabIndex = 13;
            this.numHumidityMin.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(19, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "습도 최소값 (%):";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTempMax
            // 
            this.numTempMax.DecimalPlaces = 1;
            this.numTempMax.Location = new System.Drawing.Point(219, 158);
            this.numTempMax.Name = "numTempMax";
            this.numTempMax.Size = new System.Drawing.Size(149, 25);
            this.numTempMax.TabIndex = 11;
            this.numTempMax.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(19, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "온도 최대값 (°C):";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTempMin
            // 
            this.numTempMin.DecimalPlaces = 1;
            this.numTempMin.Location = new System.Drawing.Point(219, 106);
            this.numTempMin.Name = "numTempMin";
            this.numTempMin.Size = new System.Drawing.Size(149, 25);
            this.numTempMin.TabIndex = 9;
            this.numTempMin.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(19, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "온도 최소값 (°C):";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numCo2
            // 
            this.numCo2.Location = new System.Drawing.Point(219, 52);
            this.numCo2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numCo2.Name = "numCo2";
            this.numCo2.Size = new System.Drawing.Size(149, 25);
            this.numCo2.TabIndex = 7;
            this.numCo2.Value = new decimal(new int[] {
            1700,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(19, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(185, 25);
            this.label8.TabIndex = 6;
            this.label8.Text = "CO₂ 기준값 (ppm):";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 562);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "클린룸 통합 모니터링";
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Overall.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabPage_Temperature.ResumeLayout(false);
            this.tabPage_Humidity.ResumeLayout(false);
            this.tabPage_Dust.ResumeLayout(false);
            this.tabPage_CO2.ResumeLayout(false);
            this.tabPage_Settings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPm25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHumidityMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHumidityMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTempMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTempMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCo2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Overall;
        private System.Windows.Forms.TabPage tabPage_Temperature;
        private System.Windows.Forms.TabPage tabPage_Humidity;
        private System.Windows.Forms.TabPage tabPage_Dust;
        private System.Windows.Forms.TabPage tabPage_CO2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LiveCharts.WinForms.CartesianChart chart_Overall_Temp;
        private LiveCharts.WinForms.CartesianChart chart_Overall_Humidity;
        private LiveCharts.WinForms.CartesianChart chart_Overall_Dust;
        private LiveCharts.WinForms.CartesianChart chart_Overall_CO2;
        private LiveCharts.WinForms.CartesianChart chart_Detail_Temp;
        private LiveCharts.WinForms.CartesianChart chart_Detail_Humidity;
        private LiveCharts.WinForms.CartesianChart chart_Detail_Dust;
        private LiveCharts.WinForms.CartesianChart chart_Detail_CO2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabPage tabPage_Settings;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnSet180s;
        private System.Windows.Forms.Button btnSet60s;
        private System.Windows.Forms.Button btnSet30s;
        private System.Windows.Forms.Button btnSet3600s;
        private System.Windows.Forms.Button btnSet1800s;
        private System.Windows.Forms.Button btnSet600s;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numCo2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnApplySettings;
        private System.Windows.Forms.NumericUpDown numPm25;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numHumidityMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numHumidityMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numTempMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numTempMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;

    }
}