using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace WindowsFormsApplication1
{
    public partial class ConnectSet : Office2007Form
    {
        public ConnectSet()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            SocketClient client = new SocketClient();
            string msg = "";
            string freq = comboBoxFreq.SelectedItem.ToString();



            msg = "$100" + "|" + freq + "|";
            client.ClientSendMsg(msg);
        }
    }
}
