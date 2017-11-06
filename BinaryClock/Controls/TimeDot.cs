using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryClock.Controls
{
    public partial class TimeDot : UserControl
    {
        /// <summary>
        /// Sets the "light" to off (0) or on (1).
        /// </summary>
        public int Image
        {
            set => this.pbLight.Image = value == 0 ? Properties.Resources.unlit_button : Properties.Resources.lit_button;
        }

        public TimeDot()
        {
            this.InitializeComponent();
        }
    }
}
