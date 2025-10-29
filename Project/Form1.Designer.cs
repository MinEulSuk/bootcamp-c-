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
            this.label2 = new System.Windows.Forms.Label();
            this.btnSet180s = new System.Windows.Forms.Button();
            this.btnSet60s = new System.Windows.Forms.Button();
            this.btnSet30s = new System.Windows.Forms.Button();
            this.btnApplyChanges = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTargetChart = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnSet600s = new System.Windows.Forms.Button();
            this.btnSet1800s = new System.Windows.Forms.Button();
            this.btnSet3600s = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
            this.panel1.Size = new System.Drawing.Size(444, 238);
            this.panel1.TabIndex = 4;
            // 
            // chart_Overall_Temp
            // 
            this.chart_Overall_Temp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Temp.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Temp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Temp.Name = "chart_Overall_Temp";
            this.chart_Overall_Temp.Size = new System.Drawing.Size(444, 238);
            this.chart_Overall_Temp.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chart_Overall_Humidity);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(453, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(444, 238);
            this.panel2.TabIndex = 5;
            // 
            // chart_Overall_Humidity
            // 
            this.chart_Overall_Humidity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Humidity.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Humidity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Humidity.Name = "chart_Overall_Humidity";
            this.chart_Overall_Humidity.Size = new System.Drawing.Size(444, 238);
            this.chart_Overall_Humidity.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chart_Overall_Dust);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 244);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(444, 238);
            this.panel3.TabIndex = 6;
            // 
            // chart_Overall_Dust
            // 
            this.chart_Overall_Dust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_Dust.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_Dust.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_Dust.Name = "chart_Overall_Dust";
            this.chart_Overall_Dust.Size = new System.Drawing.Size(444, 238);
            this.chart_Overall_Dust.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chart_Overall_CO2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(453, 244);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(444, 238);
            this.panel4.TabIndex = 7;
            // 
            // chart_Overall_CO2
            // 
            this.chart_Overall_CO2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_Overall_CO2.Location = new System.Drawing.Point(0, 0);
            this.chart_Overall_CO2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chart_Overall_CO2.Name = "chart_Overall_CO2";
            this.chart_Overall_CO2.Size = new System.Drawing.Size(444, 238);
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
            this.tabPage_Settings.Controls.Add(this.btnSet3600s);
            this.tabPage_Settings.Controls.Add(this.btnSet1800s);
            this.tabPage_Settings.Controls.Add(this.btnSet600s);
            this.tabPage_Settings.Controls.Add(this.label2);
            this.tabPage_Settings.Controls.Add(this.btnSet180s);
            this.tabPage_Settings.Controls.Add(this.btnSet60s);
            this.tabPage_Settings.Controls.Add(this.btnSet30s);
            this.tabPage_Settings.Controls.Add(this.btnApplyChanges);
            this.tabPage_Settings.Controls.Add(this.label1);
            this.tabPage_Settings.Controls.Add(this.cmbTargetChart);
            this.tabPage_Settings.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Settings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Settings.Name = "tabPage_Settings";
            this.tabPage_Settings.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage_Settings.Size = new System.Drawing.Size(906, 533);
            this.tabPage_Settings.TabIndex = 5;
            this.tabPage_Settings.Text = "환경설정";
            this.tabPage_Settings.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "차트 최대 표시 설정";
            // 
            // btnSet180s
            // 
            this.btnSet180s.Location = new System.Drawing.Point(409, 247);
            this.btnSet180s.Name = "btnSet180s";
            this.btnSet180s.Size = new System.Drawing.Size(75, 23);
            this.btnSet180s.TabIndex = 12;
            this.btnSet180s.Text = "3분 보기";
            this.btnSet180s.UseVisualStyleBackColor = true;
            this.btnSet180s.Click += new System.EventHandler(this.btnSet180s_Click);
            // 
            // btnSet60s
            // 
            this.btnSet60s.Location = new System.Drawing.Point(298, 247);
            this.btnSet60s.Name = "btnSet60s";
            this.btnSet60s.Size = new System.Drawing.Size(75, 23);
            this.btnSet60s.TabIndex = 11;
            this.btnSet60s.Text = "1분 보기";
            this.btnSet60s.UseVisualStyleBackColor = true;
            this.btnSet60s.Click += new System.EventHandler(this.btnSet60s_Click);
            // 
            // btnSet30s
            // 
            this.btnSet30s.Location = new System.Drawing.Point(183, 247);
            this.btnSet30s.Name = "btnSet30s";
            this.btnSet30s.Size = new System.Drawing.Size(82, 23);
            this.btnSet30s.TabIndex = 10;
            this.btnSet30s.Text = "30초 보기";
            this.btnSet30s.UseVisualStyleBackColor = true;
            this.btnSet30s.Click += new System.EventHandler(this.btnSet30s_Click);
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Location = new System.Drawing.Point(409, 71);
            this.btnApplyChanges.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(75, 22);
            this.btnApplyChanges.TabIndex = 6;
            this.btnApplyChanges.Text = "적용";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "그래프 설정";
            // 
            // cmbTargetChart
            // 
            this.cmbTargetChart.FormattingEnabled = true;
            this.cmbTargetChart.Items.AddRange(new object[] {
            "라인 차트",
            "컬럼 차트",
            "계단식 라인"});
            this.cmbTargetChart.Location = new System.Drawing.Point(167, 70);
            this.cmbTargetChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTargetChart.Name = "cmbTargetChart";
            this.cmbTargetChart.Size = new System.Drawing.Size(206, 23);
            this.cmbTargetChart.TabIndex = 0;
            // 
            // btnSet600s
            // 
            this.btnSet600s.Location = new System.Drawing.Point(183, 303);
            this.btnSet600s.Name = "btnSet600s";
            this.btnSet600s.Size = new System.Drawing.Size(85, 23);
            this.btnSet600s.TabIndex = 14;
            this.btnSet600s.Text = "10분 보기";
            this.btnSet600s.UseVisualStyleBackColor = true;
            this.btnSet600s.Click += new System.EventHandler(this.btnSet600s_Click);
            // 
            // btnSet1800s
            // 
            this.btnSet1800s.Location = new System.Drawing.Point(298, 303);
            this.btnSet1800s.Name = "btnSet1800s";
            this.btnSet1800s.Size = new System.Drawing.Size(88, 23);
            this.btnSet1800s.TabIndex = 15;
            this.btnSet1800s.Text = "30분 보기";
            this.btnSet1800s.UseVisualStyleBackColor = true;
            this.btnSet1800s.Click += new System.EventHandler(this.btnSet1800s_Click);
            // 
            // btnSet3600s
            // 
            this.btnSet3600s.Location = new System.Drawing.Point(409, 303);
            this.btnSet3600s.Name = "btnSet3600s";
            this.btnSet3600s.Size = new System.Drawing.Size(88, 23);
            this.btnSet3600s.TabIndex = 16;
            this.btnSet3600s.Text = "1시간 보기";
            this.btnSet3600s.UseVisualStyleBackColor = true;
            this.btnSet3600s.Click += new System.EventHandler(this.btnSet3600s_Click);
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
            this.tabPage_Settings.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTargetChart;
        private System.Windows.Forms.Button btnApplyChanges;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btnSet180s;
        private System.Windows.Forms.Button btnSet60s;
        private System.Windows.Forms.Button btnSet30s;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSet3600s;
        private System.Windows.Forms.Button btnSet1800s;
        private System.Windows.Forms.Button btnSet600s;
    }
}