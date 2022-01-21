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
    public partial class MasterFD : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        int id;
        DataTable table;
        public MasterFD()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loaddata();
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

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All Field Must Be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void loaddata()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from foodsAndDrinks", connection);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            dataGridView1.Columns[4].Visible = false;
        }

        void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            pictureBox1.Image = null;
            comboBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.png;*jpeg;.*bmp";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image img = Image.FromFile(ofd.FileName);
                    Bitmap bmp = (Bitmap)img;
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                 
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from foodsAndDrinks where name like '%"+textBox3.Text+"%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                ImageConverter converter = new ImageConverter();
                byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("insert into foodsAndDrinks(name, price, type, photo, created_at) values ('"+textBox1.Text+"', '"+Convert.ToInt32(textBox2.Text)+"', '"+comboBox1.Text+"', @img, getdate())", connection);
                command.Parameters.AddWithValue("@img", img);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                clear();
                loaddata();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            byte[] bytea = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream stream = new MemoryStream(bytea);
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                ImageConverter converter = new ImageConverter();
                byte[] img = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                command = new SqlCommand("update foodsAndDrinks set name='"+textBox1.Text+"', type='"+comboBox1.Text+"', price='"+textBox2.Text+"', photo=@img, updated_at = getdate() where id ="+id, connection);
                command.Parameters.AddWithValue("@img", img);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                clear();
                loaddata();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to delete it ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                command = new SqlCommand("delete from foodsAndDrinks where id="+id, connection);
                
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                clear();
                loaddata();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear();
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string com = "select * from foodsAndDrinks where type like '%" + comboBox2.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }
    }
}
