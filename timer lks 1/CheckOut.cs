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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        int id_reserroom, id_item;
        public CheckOut()
        {
            InitializeComponent();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];

            loadroom();
            loadstatus();
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
            check.ShowDialog();
            this.Hide();
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

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadroom()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select roomNumber from room where status='unavail'", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "roomNumber";
        }

        void loaditems()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from reservationRequestItem join item on reservationRequestItem.itemId = item.id where reservationRequestItem.reservationRoomid=" + id_reserroom, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox2.DataSource = table;
            comboBox2.DisplayMember = "name";
        }

        void loadstatus()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from itemStatus", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox3.DataSource = table;
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "id";
        }

        void loadFD()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from FDCheckOut where reservationRoomID = " + id_reserroom, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView2.DataSource = table;

            getotalfd();
        }

        void clear()
        {
            comboBox2.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            numericUpDown1.Value = 0;
            lblcompen.Text = "0";
            lblsub.Text = "0";

        }

        void getCom()
        {
            if(comboBox2.Text.Length > 1)
            {
                command = new SqlCommand("select compensationFee from item where name = '" + comboBox2.Text + "'", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    lblcompen.Text = reader["compensationFee"].ToString();
                    connection.Close();
                }
                catch (Exception)
                {

                    throw;
                }

            }
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
                    id_item = Convert.ToInt32(reader["id"]);
                    lblsub.Text = Convert.ToString(numericUpDown1.Value * Convert.ToInt32(reader["requestPrice"]));
                    connection.Close();
                }
                connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gettotal()
        {
            int total = 0;
            for (int i =0; i < dataGridView1.RowCount -1; i++)
            {
                total += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
                total += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }

            lblsubitem.Text = total.ToString();
        }

        void getcompe()
        {
            int comp = 0;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                comp += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }

            lblcomp.Text = comp.ToString();
        }

        void getotalfd()
        {
            int total = 0;
            for (int i = 0; i < dataGridView2.RowCount -1; i++)
            {
                total += Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value);
            }

            lbltotalfd.Text = total.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            command = new SqlCommand("select reservationRoom.id from reservationRoom join room on reservationRoom.roomId = room.id where room.roomNumber=" + Convert.ToInt32(comboBox1.Text) + " order by id desc", connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    id_reserroom = Convert.ToInt32(reader["id"]);
                    connection.Close();

                }
                connection.Close();
            }
            catch (Exception)
            {

                throw;
            }

            loaditems();
            loadFD();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text != "Good")
            {
                getCom();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowCount = dataGridView1.Rows.Add();
            dataGridView1.Rows[rowCount].Cells[0].Value = id_item;
            dataGridView1.Rows[rowCount].Cells[1].Value = comboBox2.Text;
            dataGridView1.Rows[rowCount].Cells[2].Value = numericUpDown1.Value;
            if(comboBox3.Text != "Good")
            {
                dataGridView1.Rows[rowCount].Cells[3].Value = Convert.ToInt32(lblcompen.Text) * numericUpDown1.Value;
                dataGridView1.Rows[rowCount].Cells[4].Value = (Convert.ToInt64(lblsub.Text) + Convert.ToInt32(lblcompen.Text)) * numericUpDown1.Value;

            }
            else
            {
                dataGridView1.Rows[rowCount].Cells[3].Value = 0;
                dataGridView1.Rows[rowCount].Cells[4].Value = Convert.ToInt64(lblsub.Text);

            }
            dataGridView1.Rows[rowCount].Cells[5].Value = comboBox3.SelectedValue;

            gettotal();
            getcompe();
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {

            for(int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                command = new SqlCommand("insert into reservationCheckOut(reservationRoomID, itemId, itemStatusId, qty, totalCharge) values("+id_reserroom+", "+Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value)+", "+Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value)+", "+Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value)+", "+Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value)+")", connection);
                SqlCommand sql = new SqlCommand("update room set status = 'avail' where roomNumber=" + Convert.ToInt32(comboBox1.Text), connection);
                SqlCommand update = new SqlCommand("update reservationRoom set checkOutDateTime=getDate() where id=" + id_reserroom, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    sql.ExecuteNonQuery();
                    update.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadroom();
                }
                catch (Exception)
                {

                    throw;
                }
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView2.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    SqlCommand command = new SqlCommand("delete from FDcheckout where id = " + dataGridView2.SelectedRows[0].Cells[0].Value, connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Successfully Deleted", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadFD();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure ?", "alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    gettotal();
                    getcompe();
                }
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Text.Length > 1)
            {
                command = new SqlCommand("select * from item where name = '" + comboBox2.Text + "'", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        lblcompen.Text = reader["compensationFee"].ToString();
                        connection.Close();
                    }
                    connection.Close();
                }
                catch (Exception)
                {

                    throw;
                }

                countsub();
            }
        }
    }
}
