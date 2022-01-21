using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timer_lks_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            custom();
        }

        private void custom()
        {
            panel2.Visible = false;
        }

        void hide()
        {
            if (panel2.Visible == true)
                panel2.Visible = false;
        }

        void show(Panel panel)
        {
            if (panel.Visible == false)
            {
                hide();
                panel.Visible = true;
            }
            else
                panel.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            show(panel2);
        }
    }
}
