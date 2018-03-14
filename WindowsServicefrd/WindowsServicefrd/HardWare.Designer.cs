namespace WindowsServicefrd
{
    partial class HardWare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HardWare));
            this.axDHTestHardWare1 = new AxDHHardWareLib.AxDHTestHardWare();
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare1)).BeginInit();
            this.SuspendLayout();
            // 
            // axDHTestHardWare1
            // 
            this.axDHTestHardWare1.Enabled = true;
            this.axDHTestHardWare1.Location = new System.Drawing.Point(15, 3);
            this.axDHTestHardWare1.Name = "axDHTestHardWare1";
            this.axDHTestHardWare1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDHTestHardWare1.OcxState")));
            this.axDHTestHardWare1.Size = new System.Drawing.Size(192, 192);
            this.axDHTestHardWare1.TabIndex = 0;
            // 
            // HardWare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axDHTestHardWare1);
            this.Name = "HardWare";
            this.Size = new System.Drawing.Size(219, 204);
            ((System.ComponentModel.ISupportInitialize)(this.axDHTestHardWare1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDHHardWareLib.AxDHTestHardWare axDHTestHardWare1;
    }
}
