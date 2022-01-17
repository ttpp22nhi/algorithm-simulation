using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Home : Form
    {
        Thread luong1; //thread thứ nhất
        public Home()
        {
            InitializeComponent();
        }

        private void formTrangChinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình?", "Chú ý!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (luong1 != null)
                {
                    if ((luong1.ThreadState & ThreadState.SuspendRequested) == ThreadState.SuspendRequested)
                    {
                        luong1.Resume();
                        luong1.Abort();
                    }
                    else if ((luong1.ThreadState & ThreadState.Suspended) == ThreadState.Suspended)
                    {
                        luong1.Resume();
                        luong1.Abort();
                    }
                    else
                    {
                        luong1.Abort();
                    }
                }
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

        private void sort_button_Click(object sender, EventArgs e)
        {
            formTrangChinh a = new formTrangChinh();
            a.Show();
            this.Hide();
        }
    }
    
}
