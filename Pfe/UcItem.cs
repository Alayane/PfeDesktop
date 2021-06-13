using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Pfe
{
    public partial class UcItem : UserControl
    {
        public UcItem()
        {
            InitializeComponent();
        }
        private void clearField()
        {
            txtId.Text = txtName.Text = lblError.Text = txtPrice.Text = txtDes.Text = txtQte.Text = "";
            cbCategory.SelectedIndex = -1;
            picBoxItem.Image = null;
            
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            clearField();
            lblMessage.Text = "";
        }

        private void UcItem_Load(object sender, EventArgs e)
        {
            loadData();

            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnClear, "Clear all field");
            ToolTip1.SetToolTip(this.btnSearch, "Search");
            ToolTip1.SetToolTip(this.txtId, "Search for item by name");
            ToolTip1.SetToolTip(this.btnAddImage, "add or change image");

        }

        public void loadData()
        {
            DataTable dt = new DataTable();
            string query = "select categoryId,name from category";
            SqlCommand cmd = new SqlCommand(query);
            dt = Program.GetData(cmd);
            cbCategory.DataSource = dt;
            cbCategory.DisplayMember = "name";
            cbCategory.ValueMember = "categoryId";
            cbCategory.SelectedIndex = -1;
            serch();
        }

        byte[] bytes;
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files(*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bytes = File.ReadAllBytes(dialog.FileName);
                picBoxItem.ImageLocation = dialog.FileName;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtName.Text != "" && bytes.Length != 0 && txtPrice.Text !="" && cbCategory.SelectedIndex != -1 )
            {
                string q = "select * from items where itemName like @nm";
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
                    string query = "insert into items values(@name,@des,@qte,@price,@category,@img)";
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@des", txtDes.Text);
                    cmd.Parameters.AddWithValue("@qte", txtQte.Text);
                    cmd.Parameters.AddWithValue("@price",Decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@category", cbCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@img", bytes);
                    Program.SetData(cmd);
                    lblMessage.Text = "item has been added";
                    clearField();
                }
            }
            else
            {
                lblError.Text = "please fill out all the fields";
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (txtId.Text!="")
            {
                DataTable dt = new DataTable();
                string query = "select * from items where itemName like @name";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@name", txtId.Text);
                dt = Program.GetData(cmd);
                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0][1].ToString();
                    txtDes.Text = dt.Rows[0][2].ToString();
                    txtQte.Text = dt.Rows[0][3].ToString();
                    txtPrice.Text = dt.Rows[0][4].ToString();
                    cbCategory.SelectedValue= dt.Rows[0][5];
                    byte[] img = (byte[])(dt.Rows[0][6]);
                    if (img == null)
                    {
                        picBoxItem.Image = null;
                    }
                    else
                    {
                       
                        bytes = img;
                        MemoryStream ms = new MemoryStream(img);
                        picBoxItem.Image = Image.FromStream(ms);
                    }
                    lblMessage.Text = "";
                }else
                {
                    clearField();
                    lblError.Text = "item not found";
                }
            }
            else
            {
                clearField();
                lblError.Text = "enter item name for search";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtId.Text != "")
            {
                string query = "delete from items where itemName=@name";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@name", txtId.Text);
                Program.SetData(cmd);
                lblMessage.Text = "item has been deleted";
                clearField();
            }
            else
            {
                lblError.Text = "search for the item first";
                return;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (txtName.Text != "" && picBoxItem.Image!=null && txtPrice.Text != "" && cbCategory.SelectedIndex != -1)
            {
                string q = "select * from items where name like @nm";
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
                    string query = "update items set itemName=@name, description=@des,quantity=@qte,price=@price,category=@cat,image=@img where itemName=@id";
                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@des", txtDes.Text);
                    cmd.Parameters.AddWithValue("@qte", txtQte.Text);
                    cmd.Parameters.AddWithValue("@price",Decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@cat", cbCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@img", bytes);
                    Program.SetData(cmd);
                    lblMessage.Text = "item has been updated";
                    clearField();

                }
            }
            else
            {
                lblError.Text = "please fill out all the fields";
                return;
            }
            
        }

        private void serch()
        {
            AutoCompleteStringCollection serch = new AutoCompleteStringCollection();
            string q = "select itemName from items order by itemName asc";
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
