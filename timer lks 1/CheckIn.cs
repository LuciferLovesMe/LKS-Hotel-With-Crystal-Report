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
    public partial class CheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        int id_cust;
        public CheckIn()
        {
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadCust()
        {
            command = new SqlCommand("select * from customer where id =" + id_cust, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                lblname.Text = Convert.ToString(reader["name"]);
                lblnik.Text = reader["nik"].ToString();
                lblemail.Text = reader["email"].ToString();
                if(reader["gender"].ToString() == "Male")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }
                lblphone.Text = reader["phoneNumber"].ToString();
                lbldate.Text = reader["dateOfBirth"].ToString();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("select * from reservation where bookingcode = '" + textBox1.Text + "'", connection);
            connection.Open();
            SqlDataReader reader1 = com.ExecuteReader();
            reader1.Read();
            if (reader1.HasRows)
            {
                connection.Close();
                SqlDataAdapter adapter = new SqlDataAdapter("select reservationRoom.id, reservationRoom.roomPrice,room.*, roomType.* from reservationRoom join reservation on reservationRoom.reservationId = reservation.id join room on reservationRoom.roomId = room.id join roomType on room.roomTypeId = roomType.id where reservation.bookingCode = '" + textBox1.Text + "'", connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[7].Visible = false;

                command = new SqlCommand("select reservation.customerId from reservation where bookingCode = '" + textBox1.Text + "'", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        id_cust = Convert.ToInt32(reader["customerId"]);
                    }
                    connection.Close();
                }
                catch (Exception)
                {
                    throw;
                }

                loadCust();

            }
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                command = new SqlCommand("update reservationRoom set checkInDateTime=getDate() where id=" + Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value), connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Check In Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
