namespace WinComServer
{
    partial class UserControl1
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.axDHTestHardWare = new AxDHHardWareLib.AxDHTestHardWare();
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare)).BeginInit();
            this.SuspendLayout();
            // 
            // axDHTestHardWare
            // 
            this.axDHTestHardWare.Enabled = true;
            this.axDHTestHardWare.Location = new System.Drawing.Point(8, 6);
            this.axDHTestHardWare.Name = "axDHTestHardWare";
            this.axDHTestHardWare.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDHTestHardWare.OcxState")));
            this.axDHTestHardWare.Size = new System.Drawing.Size(204, 203);
            this.axDHTestHardWare.TabIndex = 0;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axDHTestHardWare);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(203, 201);
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxDHHardWareLib.AxDHTestHardWare axDHTestHardWare;
    }
}
