using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.Net.Mail;

namespace Pfe
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtEmail.Text = "Email";
            txtPassword.UseSystemPasswordChar = false;
            txtPassword.Text = "Password";
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Email")
                txtEmail.Text = "";
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text == "")
            {
                txtEmail.Text = "Email";
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
                txtPassword.Text = "";
            txtPassword.UseSystemPasswordChar = true;
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        public bool checkemail()
        {
            try
            {
                new MailAddress(txtEmail.Text);
            }
            catch (Exception)
            {
                lblError.Text="Inavalid email!";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtEmail.Focus();
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string query = "select password from Users where Email=@email";
            SqlCommand cmd = new SqlCommand(query);
            if(txtEmail.Text!="" && txtPassword.Text != "")
            {
                if (checkemail())
                {
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    dt= Program.GetData(cmd);
                    if (dt.Rows.Count > 0)
                    {
                        if (txtPassword.Text==dt.Rows[0][0].ToString())
                        {
                            this.Hide();
                            FrmHome frm = new FrmHome();
                            frm.ShowDialog();
                            if (frm.IsDisposed)
                                return;
                            else
                                this.Show();
                        }
                        else
                        {
                            lblError.Text = "Invalid Password!";
                            txtPassword.Text = "";
                            txtPassword.Focus();
                        }
                    }
                    else
                    {
                        lblError.Text = "Invalid email";
                        txtEmail.Text = "";
                        txtPassword.Text = "";
                        txtEmail.Focus();
                    }
                }
            }
            

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
