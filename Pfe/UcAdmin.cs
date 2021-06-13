using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;

namespace Pfe
{
    public partial class UcAdmin : UserControl
    {
        public UcAdmin()
        {
            InitializeComponent();
        }

        private void UcAdmin_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnClear, "Clear all field");
            ToolTip1.SetToolTip(this.btnSearch, "Search");
            ToolTip1.SetToolTip(this.txtId, "Search for admin by id");
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearField();
            lblMessage.Text = "";
        }

        private void clearField()
        {
            txtId.Text = txtLname.Text = txtFname.Text = txtEmail.Text = txtPassword.Text = lblError.Text = "";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtFname.Text == "" || txtLname.Text == "" || txtEmail.Text == "" || txtPassword.Text == "")
            {
                lblError.Text = "please fill out all the fields";
                return;
            }
            if (checkemail())
            {
                string q = "select * from users where email like @em";
                SqlCommand cmde = new SqlCommand(q);
                cmde.Parameters.AddWithValue("@em", txtEmail.Text);
                DataTable dt=Program.GetData(cmde);
                if (dt.Rows.Count > 0)
                {
                    lblError.Text = "That email is taken. Try another.";
                    return;
                }
                else
                {
                    try
                    {
                        string query = "insert into users values(@fname,@lname,@email,@password)";
                        SqlCommand cmd = new SqlCommand(query);
                        cmd.Parameters.AddWithValue("@fname", txtFname.Text.Trim());
                        cmd.Parameters.AddWithValue("@lname", txtLname.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                        Program.SetData(cmd);
                        lblMessage.Text = "an admin has been added";
                        clearField();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        throw;
                    }
                }
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
                lblError.Text = "Invalid email";
                return false;
            }
            return true;
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text != "")
            {
                if (checkemail())
                {
                    lblError.Text = "";
                }
            }else
                lblError.Text = "";

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtId.Text=="" ||txtFname.Text == "" || txtLname.Text == "" || txtEmail.Text == "" || txtPassword.Text == "")
            {
                lblError.Text = "please fill out all the fields";
                return;
            }
            if (checkemail())
            {
                string q = "select * from users where email like @em";
                SqlCommand cmde = new SqlCommand(q);
                cmde.Parameters.AddWithValue("@em", txtEmail.Text);
                DataTable dt = Program.GetData(cmde);
                if (dt.Rows.Count > 0)
                {
                    lblError.Text = "That email is taken. Try another.";
                    return;
                }
                else
                {
                    string query = "update users set firstname = @fname, lastname = @lname, Email = @email, password = @password where userId = @id";
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Parameters.AddWithValue("@fname", txtFname.Text);
                    cmd.Parameters.AddWithValue("@lname", txtLname.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    Program.SetData(cmd);
                    lblMessage.Text = "admin has been updated";
                    clearField();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string query = "delete from users where userId=@id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@id", txtId.Text);
            Program.SetData(cmd);
            lblMessage.Text = "admin has been deleted";
            clearField();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable(); 
            string query = "select firstname,lastname,Email,password from users where userId=@id ";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@id", txtId.Text);
            dt = Program.GetData(cmd);
            if (dt.Rows.Count > 0)
            {
                txtFname.Text = dt.Rows[0][0].ToString();
                txtLname.Text = dt.Rows[0][1].ToString();
                txtEmail.Text = dt.Rows[0][2].ToString();
                txtPassword.Text = dt.Rows[0][3].ToString();
                lblMessage.Text = "";
            }
            else
            {
                lblError.Text = "admin not found";
                clearField();
            }

        }

    }
}
