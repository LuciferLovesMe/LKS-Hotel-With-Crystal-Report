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
    public partial class MasterItem : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        DataTable table;
        SqlCommand command;
        int id;

        public MasterItem()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loadgrid();
        }

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from item", connection);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 ||textBox4.TextLength < 1)
            {
                MessageBox.Show("All Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1)
            {
                MessageBox.Show("All Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if(id == 0)
            {
                MessageBox.Show("Please Select An Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from item where name like '%"+textBox1.Text+"%'";
            dataGridView1.DataSource = Command.data(com);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("insert into item(name, requestPrice, compensationFee, created_at) values('" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', getdate())", connection);
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
                loadgrid();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val_up())
            {
                command = new SqlCommand("update item set name='"+textBox2.Text+"', requestPrice='"+textBox3.Text+"', compensationFee='"+textBox4.Text+"', updated_at = getdate() where id="+id, connection);
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
                loadgrid();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from item where id=" + id, connection);
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
                    loadgrid();

                }
            }
            else
            {
                MessageBox.Show("Please Select An Item", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
