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
    public partial class MasterRoomType : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        DataTable table;
        int id;
        public MasterRoomType()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loadgrid();
        }

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from roomType", connection);
            table = new DataTable();
            adapter.Fill(table);

            dataGridView1.DataSource = table;
            dataGridView1.Columns[4].Visible = false;
        }

        bool val()
        {
            if(textBox2.TextLength<1 || textBox3.TextLength < 1 || numericUpDown1.Value < 1 || pictureBox1.Image == null)
            {
                MessageBox.Show("All Fields Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || numericUpDown1.Value < 1 || pictureBox1.Image == null)
            {
                MessageBox.Show("All Fields Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (id == 0)
            {
                MessageBox.Show("Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            textBox3.Text = "";
            pictureBox1.Image = null;
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)){
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from roomType where name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Images|*.jpg;*.png;*.jpeg;*.bmp";
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bmp = (Bitmap)Bitmap.FromFile(openFile.FileName);
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                ImageConverter converter = new ImageConverter();
                byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("insert into roomType(name, capacity, roomPrice, photo, created_at) values('"+textBox2.Text+"', "+numericUpDown1.Value+", "+Convert.ToInt32(textBox3.Text)+", @img, getdate())", connection);
                command.Parameters.AddWithValue("@img", img);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            byte[] img = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream stream = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val_up())
            {
                ImageConverter converter = new ImageConverter();
                byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("update roomType set name='"+textBox2.Text+"', capacity= "+numericUpDown1.Value+", roomPrice= "+textBox3.Text+", photo=@img, updated_at=getdate() where id=" + id, connection);
                command.Parameters.AddWithValue("@img", img);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from roomType where id=" + id, connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Success", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Select An Item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clear();
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
    }
}
