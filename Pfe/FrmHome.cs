using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pfe
{
    public partial class FrmHome : Form
    {
     

        public FrmHome()
        {
            InitializeComponent();
            ucItem1.Hide();
            ucAdmin1.Hide();
            ucCategory1.Hide();
        }

        private void FrmHome_Load(object sender, EventArgs e)
        {
            panelMenu.Visible = false;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            if (panelMenu.Visible == true)
            {
                panelMenu.Visible = false;
            }
            else
            {
                panelMenu.Visible = true;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panelMenu.Visible == true)
            {
                panelMenu.Visible = false;
            }
            ucItem1.Hide();
            ucCategory1.Hide();
            ucAdmin1.Show();
            

        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            ucItem1.Hide();
            ucAdmin1.Hide();
            ucCategory1.Show();
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            ucAdmin1.Hide();
            ucCategory1.Hide();
            ucItem1.Show();
        }
    }
}
