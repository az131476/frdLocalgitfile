using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TestHardWare_WinForm_
{
    public partial class HardWare : UserControl
    {
        public HardWare()
        {
            InitializeComponent();
        }
        public AxDHHardWareLib.AxDHTestHardWare GetHardWare()
        {
            return axDHTestHardWare1;
        }
    }
}
