using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Pfe
{
    public partial class UcCategory : UserControl
    {
        public UcCategory()
        {
            InitializeComponent();
        }

        private void UcCategory_Load(object sender, EventArgs e)
        {
            serch();
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnClear, "Clear all field");
            ToolTip1.SetToolTip(this.btnSearch, "Search");
            ToolTip1.SetToolTip(this.txtId, "Search for category by id");
            ToolTip1.SetToolTip(this.btnAddImage, "add or change image");
        }
        byte[] bytes;
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bytes = File.ReadAllBytes(dialog.FileName);
                picBoxCategory.ImageLocation =dialog.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtName.Text != "" && picBoxCategory.Image!=null)
            {
                string q = "select * from category where name like @nm";
                SqlCommand cmde = new SqlCommand(q);
                cmde.Parameters.AddWithValue("@nm", txtName.Text);
                DataTable dt = Program.GetData(cmde);
                if (dt.Rows.Count > 0)
                {
                    lblError.Text = "That name is taken. Try another.";
                    return;
                }
                else
                {
                    string query = "insert into category values(@name,@image)";
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@image", bytes);
                    Program.SetData(cmd);
                    lblMessage.Text = "category has been added";
                    clearField();
                }

            }
            else
            {
                lblError.Text = "please fill out all the fields";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (txtId.Text != "")
            {
                DataTable dt = new DataTable();
                string query = "select name,image from category where name like @name";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@name", txtId.Text);
                dt = Program.GetData(cmd);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0][0].ToString();
                    byte[] img = (byte[])(dt.Rows[0][1]);
                    if (img == null)
                    {
                        picBoxCategory.Image = null;
                    }
                    else
                    {
                        bytes = img;
                        MemoryStream ms = new MemoryStream(img);
                        picBoxCategory.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    lblError.Text = "category not found";
                    clearField();
                    return;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearField();
            lblMessage.Text =lblError.Text= "";
        }
        private void clearField()
        {
            txtId.Text = txtName.Text= "";
            picBoxCategory.Image = null;
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtId.Text != "")
            {
                string query = "delete from category where name=@nm";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@nm", txtName.Text);
                Program.SetData(cmd);
                lblMessage.Text = "category has been deleted";
                clearField();
            }
            else
            {
                lblError.Text = "enter the name to delete a category";
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if(txtName.Text != "" && bytes.Length != 0)
            {
                string q = "select * from category where name like @nm";
                SqlCommand cmde = new SqlCommand(q);
                cmde.Parameters.AddWithValue("@nm", txtName.Text);
                DataTable dt = Program.GetData(cmde);
                if (dt.Rows.Count > 0)
                {
                    lblError.Text = "That name is taken. Try another.";
                    return;
                }
                else
                {
                    string query = "update category set name=@name,image=@image where categoryId=@id";
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.Parameters.AddWithValue("@name",txtName.Text);
                    cmd.Parameters.AddWithValue("@image",bytes);
                    Program.SetData(cmd);
                    lblMessage.Text = "category has been updated";
                    clearField();

                }
            }
            else
            {
                lblError.Text = "please fill out all the fields";
            }
        }

        public void serch()
        {
            AutoCompleteStringCollection serch = new AutoCompleteStringCollection();
            string q = "select name from category order by name asc";
            SqlCommand cmde = new SqlCommand(q);
            DataTable dt = Program.GetData(cmde);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    serch.Add(r[0].ToString());
                }
                txtId.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtId.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtId.AutoCompleteCustomSource = serch;
            }
        }
    }
}
