namespace TestHardWare_WinForm_
{
    partial class TestHardWare
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestHardWare));
            this.axDHTestHardWare = new AxDHHardWareLib.AxDHTestHardWare();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboFullValue = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboMeasureType = new System.Windows.Forms.ComboBox();
            this.comboFreq = new System.Windows.Forms.ComboBox();
            this.comboChan = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textStrainElast = new System.Windows.Forms.TextBox();
            this.textStrainPosion = new System.Windows.Forms.TextBox();
            this.textStrainSenseCoef = new System.Windows.Forms.TextBox();
            this.textStrainLead = new System.Windows.Forms.TextBox();
            this.textStrainGauge = new System.Windows.Forms.TextBox();
            this.comboBrigeType = new System.Windows.Forms.ComboBox();
            this.comboStrainType = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonInit = new System.Windows.Forms.Button();
            this.buttonBalance = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonStartSample = new System.Windows.Forms.Button();
            this.buttonStopSample = new System.Windows.Forms.Button();
            this.hardWare2 = new TestHardWare_WinForm_.HardWare();
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare)).BeginInit();
            this.groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // axDHTestHardWare
            // 
            this.axDHTestHardWare.Enabled = true;
            this.axDHTestHardWare.Location = new System.Drawing.Point(11, 470);
            this.axDHTestHardWare.Name = "axDHTestHardWare";
            this.axDHTestHardWare.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDHTestHardWare.OcxState")));
            this.axDHTestHardWare.Size = new System.Drawing.Size(192, 74);
            this.axDHTestHardWare.TabIndex = 0;
            this.axDHTestHardWare.Visible = false;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label11);
            this.groupBox.Controls.Add(this.comboFullValue);
            this.groupBox.Controls.Add(this.label10);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.comboMeasureType);
            this.groupBox.Controls.Add(this.comboFreq);
            this.groupBox.Controls.Add(this.comboChan);
            this.groupBox.Location = new System.Drawing.Point(3, 1);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(200, 132);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "通道信息";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 6;
            this.label11.Text = "量程：";
            // 
            // comboFullValue
            // 
            this.comboFullValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFullValue.FormattingEnabled = true;
            this.comboFullValue.Location = new System.Drawing.Point(100, 101);
            this.comboFullValue.Name = "comboFullValue";
            this.comboFullValue.Size = new System.Drawing.Size(90, 20);
            this.comboFullValue.TabIndex = 5;
            this.comboFullValue.SelectedIndexChanged += new System.EventHandler(this.comboFullValue_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "采样频率：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "测量类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "通道列表：";
            // 
            // comboMeasureType
            // 
            this.comboMeasureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMeasureType.FormattingEnabled = true;
            this.comboMeasureType.Location = new System.Drawing.Point(100, 74);
            this.comboMeasureType.Name = "comboMeasureType";
            this.comboMeasureType.Size = new System.Drawing.Size(90, 20);
            this.comboMeasureType.TabIndex = 1;
            this.comboMeasureType.SelectedIndexChanged += new System.EventHandler(this.comboMeasureType_SelectedIndexChanged);
            // 
            // comboFreq
            // 
            this.comboFreq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFreq.FormattingEnabled = true;
            this.comboFreq.Location = new System.Drawing.Point(100, 20);
            this.comboFreq.Name = "comboFreq";
            this.comboFreq.Size = new System.Drawing.Size(90, 20);
            this.comboFreq.TabIndex = 1;
            this.comboFreq.SelectedIndexChanged += new System.EventHandler(this.comboFreq_SelectedIndexChanged);
            // 
            // comboChan
            // 
            this.comboChan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboChan.FormattingEnabled = true;
            this.comboChan.Location = new System.Drawing.Point(100, 48);
            this.comboChan.Name = "comboChan";
            this.comboChan.Size = new System.Drawing.Size(90, 20);
            this.comboChan.TabIndex = 0;
            this.comboChan.SelectedIndexChanged += new System.EventHandler(this.comboChan_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textStrainElast);
            this.groupBox2.Controls.Add(this.textStrainPosion);
            this.groupBox2.Controls.Add(this.textStrainSenseCoef);
            this.groupBox2.Controls.Add(this.textStrainLead);
            this.groupBox2.Controls.Add(this.textStrainGauge);
            this.groupBox2.Controls.Add(this.comboBrigeType);
            this.groupBox2.Controls.Add(this.comboStrainType);
            this.groupBox2.Location = new System.Drawing.Point(3, 153);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 205);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "应变应力子参数设置";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 180);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "弹性模量：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "泊松比：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "灵敏度系数：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "导线电阻：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "应变计阻值：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "桥路方式：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "应变应力类型：";
            // 
            // textStrainElast
            // 
            this.textStrainElast.Location = new System.Drawing.Point(100, 177);
            this.textStrainElast.Name = "textStrainElast";
            this.textStrainElast.Size = new System.Drawing.Size(90, 21);
            this.textStrainElast.TabIndex = 5;
            this.textStrainElast.Leave += new System.EventHandler(this.textStrainElast_Leave);
            // 
            // textStrainPosion
            // 
            this.textStrainPosion.Location = new System.Drawing.Point(100, 150);
            this.textStrainPosion.Name = "textStrainPosion";
            this.textStrainPosion.Size = new System.Drawing.Size(90, 21);
            this.textStrainPosion.TabIndex = 5;
            this.textStrainPosion.Leave += new System.EventHandler(this.textStrainPosion_Leave);
            // 
            // textStrainSenseCoef
            // 
            this.textStrainSenseCoef.Location = new System.Drawing.Point(100, 123);
            this.textStrainSenseCoef.Name = "textStrainSenseCoef";
            this.textStrainSenseCoef.Size = new System.Drawing.Size(90, 21);
            this.textStrainSenseCoef.TabIndex = 4;
            this.textStrainSenseCoef.Leave += new System.EventHandler(this.textStrainSenseCoef_Leave);
            // 
            // textStrainLead
            // 
            this.textStrainLead.Location = new System.Drawing.Point(100, 96);
            this.textStrainLead.Name = "textStrainLead";
            this.textStrainLead.Size = new System.Drawing.Size(90, 21);
            this.textStrainLead.TabIndex = 3;
            this.textStrainLead.Leave += new System.EventHandler(this.textStrainLead_Leave);
            // 
            // textStrainGauge
            // 
            this.textStrainGauge.Location = new System.Drawing.Point(100, 69);
            this.textStrainGauge.Name = "textStrainGauge";
            this.textStrainGauge.Size = new System.Drawing.Size(90, 21);
            this.textStrainGauge.TabIndex = 2;
            this.textStrainGauge.Leave += new System.EventHandler(this.textStrainGauge_Leave);
            // 
            // comboBrigeType
            // 
            this.comboBrigeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBrigeType.FormattingEnabled = true;
            this.comboBrigeType.Location = new System.Drawing.Point(100, 43);
            this.comboBrigeType.Name = "comboBrigeType";
            this.comboBrigeType.Size = new System.Drawing.Size(90, 20);
            this.comboBrigeType.TabIndex = 1;
            this.comboBrigeType.SelectedIndexChanged += new System.EventHandler(this.comboBrigeType_SelectedIndexChanged);
            // 
            // comboStrainType
            // 
            this.comboStrainType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStrainType.FormattingEnabled = true;
            this.comboStrainType.Location = new System.Drawing.Point(100, 17);
            this.comboStrainType.Name = "comboStrainType";
            this.comboStrainType.Size = new System.Drawing.Size(90, 20);
            this.comboStrainType.TabIndex = 0;
            this.comboStrainType.SelectedIndexChanged += new System.EventHandler(this.comboStrainType_SelectedIndexChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(238, 10);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1205, 676);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(5, 365);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 5;
            this.buttonInit.Text = "建立连接";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // buttonBalance
            // 
            this.buttonBalance.Location = new System.Drawing.Point(5, 394);
            this.buttonBalance.Name = "buttonBalance";
            this.buttonBalance.Size = new System.Drawing.Size(75, 23);
            this.buttonBalance.TabIndex = 6;
            this.buttonBalance.Text = "平衡";
            this.buttonBalance.UseVisualStyleBackColor = true;
            this.buttonBalance.Click += new System.EventHandler(this.buttonBalance_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(5, 424);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 7;
            this.buttonClear.Text = "清零";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonStartSample
            // 
            this.buttonStartSample.Location = new System.Drawing.Point(103, 365);
            this.buttonStartSample.Name = "buttonStartSample";
            this.buttonStartSample.Size = new System.Drawing.Size(75, 23);
            this.buttonStartSample.TabIndex = 8;
            this.buttonStartSample.Text = "启动采样";
            this.buttonStartSample.UseVisualStyleBackColor = true;
            this.buttonStartSample.Click += new System.EventHandler(this.buttonStartSample_Click);
            // 
            // buttonStopSample
            // 
            this.buttonStopSample.Location = new System.Drawing.Point(103, 393);
            this.buttonStopSample.Name = "buttonStopSample";
            this.buttonStopSample.Size = new System.Drawing.Size(75, 23);
            this.buttonStopSample.TabIndex = 9;
            this.buttonStopSample.Text = "停止采样";
            this.buttonStopSample.UseVisualStyleBackColor = true;
            this.buttonStopSample.Click += new System.EventHandler(this.buttonStopSample_Click);
            // 
            // hardWare2
            // 
            this.hardWare2.Location = new System.Drawing.Point(8, 550);
            this.hardWare2.Name = "hardWare2";
            this.hardWare2.Size = new System.Drawing.Size(195, 135);
            this.hardWare2.TabIndex = 11;
            // 
            // TestHardWare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 697);
            this.Controls.Add(this.hardWare2);
            this.Controls.Add(this.buttonStopSample);
            this.Controls.Add(this.buttonStartSample);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonBalance);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.axDHTestHardWare);
            this.Name = "TestHardWare";
            this.Text = "TestHardWare";
            this.Load += new System.EventHandler(this.TestHardWare_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDHHardWareLib.AxDHTestHardWare axDHTestHardWare;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboMeasureType;
        private System.Windows.Forms.ComboBox comboChan;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textStrainElast;
        private System.Windows.Forms.TextBox textStrainPosion;
        private System.Windows.Forms.TextBox textStrainSenseCoef;
        private System.Windows.Forms.TextBox textStrainLead;
        private System.Windows.Forms.TextBox textStrainGauge;
        private System.Windows.Forms.ComboBox comboBrigeType;
        private System.Windows.Forms.ComboBox comboStrainType;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.Button buttonBalance;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonStartSample;
        private System.Windows.Forms.Button buttonStopSample;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboFreq;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboFullValue;
        private HardWare hardWare2;
    }
}

