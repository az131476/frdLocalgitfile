using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinComServer
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
