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
    public partial class RequestAddItem : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        int id_items, id_reser;
        public RequestAddItem()
        {
            InitializeComponent();

            loaditems();
            loadroom();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            CheckIn check = new CheckIn();
            this.Hide();
            check.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            RequestAddItem request = new RequestAddItem();
            this.Hide();
            request.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            ReportCheckIn report = new ReportCheckIn();
            this.Hide();
            report.ShowDialog();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            ReportGuest report = new ReportGuest();
            this.Hide();
            report.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loaditems()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from item", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox2.DataSource = table;
            comboBox2.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        void loadroom()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select roomNumber from room where status='unavail'", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "roomNumber";
        }

        void countsub()
        {
            try
            {
                connection.Open();
                command = new SqlCommand("select * from item where name = '" + comboBox2.Text + "'", connection);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    id_items = Convert.ToInt32(reader["id"]);
                    lblprice.Text = reader["requestPrice"].ToString();
                    lblsub.Text = Convert.ToString(numericUpDown1.Value * Convert.ToInt32(reader["requestPrice"]));
                }
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            lblprice.Text = "0";
            lblsub.Text = "0";
            numericUpDown1.ResetText();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rowCount = dataGridView1.Rows.Add();
            dataGridView1.Rows[rowCount].Cells[0].Value = id_items;
            dataGridView1.Rows[rowCount].Cells[1].Value = comboBox2.Text;
            dataGridView1.Rows[rowCount].Cells[2].Value = numericUpDown1.Value;
            dataGridView1.Rows[rowCount].Cells[3].Value = lblprice.Text;
            dataGridView1.Rows[rowCount].Cells[4].Value = lblsub.Text;
        }

        private void label21_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Log Out ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                main.ShowDialog();
                this.Hide();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure ?", "alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand sql = new SqlCommand("select reservationRoom.id from reservationRoom join room on reservationRoom.roomId = room.id where room.roomNumber = " + Convert.ToInt32(comboBox1.Text), connection);
            connection.Open();
            SqlDataReader sqlData = sql.ExecuteReader();
            sqlData.Read();
            if (sqlData.HasRows)
            {
                id_reser = Convert.ToInt32(sqlData["id"]);
            }
            connection.Close();
            if(comboBox1.Text != null || dataGridView1.RowCount >= 2)
            {
                for(int i = 0; i < dataGridView1.RowCount -1; i++)
                {
                    command = new SqlCommand("insert into reservationRequestItem(reservationRoomId, itemId, qty, totalPrice) values(" + id_reser + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) + ")", connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
