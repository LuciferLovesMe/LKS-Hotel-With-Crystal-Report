using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timer_lks_1
{
    public partial class MasterEmployee : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        DataTable table;
        int id;
        public MasterEmployee()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loadgrid();
            loadcombo();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterEmployee master = new MasterEmployee();
            this.Hide();
            master.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            MasterFD master = new MasterFD();
            this.Hide();
            master.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            MasterItem master = new MasterItem();
            this.Hide();
            master.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            MasterRoomType master = new MasterRoomType();
            this.Hide();
            master.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            MasterRoom master = new MasterRoom();
            this.Hide();
            master.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool val()
        {
            if(txtusername.TextLength < 1 || txtname.TextLength < 1 || txtpass.TextLength < 1 || txtconf.TextLength < 1 || txtemail.TextLength < 1 || txtaddress.TextLength < 1 || pictureBox1.Image == null || dtdob.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All Fields Must be Filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(txtconf.TextLength < 8 || txtpass.TextLength < 8)
            {
                MessageBox.Show("Password and Confirm Password Must be at Least 8 Chars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtconf.Text != txtpass.Text)
            {
                MessageBox.Show("Confirm Password Doesn't Same With the Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool val_up()
        {
            if (txtusername.TextLength < 1 || txtname.TextLength < 1 || txtemail.TextLength < 1 || txtaddress.TextLength < 1 || pictureBox1.Image == null || dtdob.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All Fields Must be Filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(id == 0)
            {
                MessageBox.Show("Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select employee.*, job.name from employee join job on employee.jobId = job.id", connection);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[2].Visible = false;
        }

        
        void loadcombo()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from job", connection);
                DataTable data = new DataTable();
                adapter.Fill(data);
                comboBox1.DataSource = data;
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "id";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clear()
        {
            txtusername.Text = "";
            txtname.Text = "";
            txtpass.Text = "";
            txtconf.Text = "";
            txtaddress.Text = "";
            txtemail.Text = "";
            pictureBox1.Image = null;
            comboBox1.Text = "";
            dtdob.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.png;*.jpeg;*.bmp";
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Image image = Image.FromFile(ofd.FileName);
                Bitmap bitmap = (Bitmap)image;
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                MessageBox.Show("Something is Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                txtpass.PasswordChar = '\0';
                txtconf.PasswordChar = '\0';
            }
            else if(checkBox1.Checked == false)
            {
                txtconf.PasswordChar = '*';
                txtpass.PasswordChar = '*';
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from employee where name like '%"+textBox2.Text+"%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string pass = Enc.encrypt(txtpass.Text);
                ImageConverter converter = new ImageConverter();
                byte[] image = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("insert into employee(username, name, password, address, email, dateOfBirth, jobId, photo,created_at) values('" + txtusername.Text + "', '" + txtname.Text + "', '" + pass + "', '" + txtaddress.Text + "', '" + txtemail.Text + "', '" + dtdob.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + comboBox1.SelectedValue + "', @img, getdate())", connection);
                command.Parameters.AddWithValue("@img", image);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    clear();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            txtusername.Text = Convert.ToString(dataGridView1.SelectedRows[0].Cells[1].Value);
            txtname.Text = Convert.ToString(dataGridView1.SelectedRows[0].Cells[3].Value);
            txtaddress.Text = Convert.ToString(dataGridView1.SelectedRows[0].Cells[4].Value);
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value);
            txtemail.Text = Convert.ToString(dataGridView1.SelectedRows[0].Cells[8].Value);

            byte[] img = (byte[])dataGridView1.SelectedRows[0].Cells[7].Value;
            MemoryStream stream = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val_up())
            {
                ImageConverter converter = new ImageConverter();
                byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("update employee set username='" + txtusername.Text + "', name='" + txtname.Text + "', email='" + txtemail.Text + "', dateOfBirth='" + dtdob.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', jobId= " + comboBox1.SelectedValue + ", address='" + txtaddress.Text + "', photo=@img, updated_at = getdate() where id=" + id, connection);
                command.Parameters.AddWithValue("@img", img);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from employee where id=" + id, connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid();
                        clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Log Out ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string com = "select employee.*, job.name from employee join job on employee.jobId = job.id where job.name like '%" + comboBox2.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }
    }
}
