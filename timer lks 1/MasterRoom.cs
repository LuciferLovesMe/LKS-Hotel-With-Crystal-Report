using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timer_lks_1
{
    public partial class MasterRoom : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        DataTable table;
        int id;
        public MasterRoom()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loadcombo();
            loadgrid();
            loadsort();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
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

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select room.*,roomType.name from room join roomType on room.roomTypeId = roomType.id", connection);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        void loadcombo()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from roomType", connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            comboBox1.DataSource = data;
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        bool val()
        {
            if(comboBox1.Text.Length < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1)
            {
                MessageBox.Show("Fill All Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool val_up()
        {
            if (comboBox1.Text.Length < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1)
            {
                MessageBox.Show("Fill All Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(id == 0)
            {
                MessageBox.Show("Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        int getnum()
        {
            command = new SqlCommand("select top (1) id from room order by id desc", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int roomnum;
            if (reader.HasRows)
            {
                roomnum = Convert.ToInt32(reader["id"]) + 1;
            }
            else
            {
                roomnum = 1;
            }
            connection.Close();

            return roomnum;
        }

        void loadsort()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from roomType", connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            comboBox2.DataSource = data;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
        }

        void clear()
        {
            textBox2.Text = "";
            comboBox1.Text = "";
            textBox3.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select room.*,roomType.name from room join roomType on room.roomTypeId = roomType.id where room.roomNumber like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("insert into room(roomTypeId, roomNumber, roomFloor, description, status, created_at) values (" + comboBox1.SelectedValue + ", " + getnum() + ", " + Convert.ToInt32(textBox2.Text) + ", '" + textBox3.Text + "', 'avail', getdate())", connection);
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
            if (val_up())
            {
                command = new SqlCommand("update room set roomTypeId= "+comboBox1.SelectedValue+", roomFloor= "+Convert.ToInt32(textBox2.Text)+", description='"+textBox3.Text+"', updated_at = getdate() where id ="+id, connection);
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

        private void button6_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from room where id =" + id, connection);
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
                MessageBox.Show("Please Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

        private void button3_Click(object sender, EventArgs e)
        {
            string com = "select room.*,roomType.name from room join roomType on room.roomTypeId = roomType.id where roomtype.name like '%" + comboBox2.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }
    }
}
