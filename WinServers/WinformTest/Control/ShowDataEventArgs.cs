using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
    public class ShowDataEventArgs:EventArgs
    {
        public float[] pfltDats { get; set; }
        public int nReceiveCount { get; set; }

        public ShowDataEventArgs(float[] pfltDats, int nReceiveCount)
        {
            this.pfltDats = pfltDats;
            this.nReceiveCount = nReceiveCount;
        }
    }
}
