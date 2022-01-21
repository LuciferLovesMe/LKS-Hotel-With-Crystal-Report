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
using CrystalDecisions.CrystalReports.Engine;

namespace timer_lks_1
{
    public partial class SelectRoom : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        int id_reservation;
        public SelectRoom()
        {
            InitializeComponent();
            loadcombo();

            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            getcode();
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadcombo()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from roomType", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
        }

        bool val()
        {
            if(dateTimePicker2.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("Check in Date Must be Under Check Out Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(txtstaying.TextLength < 1)
            {
                MessageBox.Show("Staying must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(dataGridView2.RowCount < 1){
                MessageBox.Show("Please Select A Room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        string getcode()
        {
            string code = "";
            command = new SqlCommand("select top (1) * from reservation order by id desc", connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    int id = Convert.ToInt32(reader["id"]) + 1;
                    code = "BK" +id.ToString();
                }
                else
                {
                    code = "BK0001";
                }
                connection.Close();
                lblcode.Text = code;
                return code;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1)
            {
                MessageBox.Show("Please Select A Room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int rowCount = dataGridView2.Rows.Add();
                dataGridView2.Rows[rowCount].Cells[0].Value = textBox2.Text;
                dataGridView2.Rows[rowCount].Cells[1].Value = textBox3.Text;
                dataGridView2.Rows[rowCount].Cells[2].Value = textBox4.Text;
                dataGridView2.Rows[rowCount].Cells[4].Value = textBox5.Text;
                dataGridView2.Rows[rowCount].Cells[3].Value = textBox6.Text;
            }
        }

        private void txtstaying_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                

                command = new SqlCommand("insert into reservation(datetime, employeeId, customerId, bookingCode, month, year) values(getdate(), " + Model.id + ", " + UserSelected.id + ", '" + getcode() + "', '" + dateTimePicker1.Value.ToString("MM") + "', '" + dateTimePicker2.Value.ToString("yyyy") + "')", connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                SqlCommand sql1 = new SqlCommand("select top (1) * from reservation order by id desc", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = sql1.ExecuteReader();
                    reader.Read();
                    id_reservation = Convert.ToInt32(reader["id"]);
                    connection.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    int roomprice = (int)(Convert.ToInt64(dataGridView2.Rows[i].Cells[3].Value) * Convert.ToInt64(txtstaying.Text));
                    SqlCommand sqlCommand = new SqlCommand("insert into reservationRoom values(" + id_reservation + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value) + ", getdate(), " + Convert.ToInt32(txtstaying.Text) + ", " + roomprice + ", '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "')", connection);
                    SqlCommand sql = new SqlCommand("update room set status='unavail' where id=" + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value), connection);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sql.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ReportDocument rd = new Cr2();

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("Room Number", typeof(int));
                dt.Columns.Add("Room Floor", typeof(int));
                dt.Columns.Add("Price", typeof(int));
                dt.Columns.Add("Description", typeof(string));
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    dt.Rows.Add(dataGridView2.Rows[i].Cells[1].Value, dataGridView2.Rows[i].Cells[2].Value, dataGridView2.Rows[i].Cells[3].Value, dataGridView2.Rows[i].Cells[4].Value);
                }
                ds.Tables.Add(dt);
                ds.WriteXmlSchema("report.xml");

                ShowReport sr = new ShowReport();
                Cr2 cr = new Cr2();
                cr.SetDataSource(ds);
                sr.crystalReportViewer1.ReportSource = cr;
                sr.Show();

                //RequestAddItem requestAdd = new RequestAddItem();
                //this.Hide();
                //requestAdd.ShowDialog();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("Select room.*, roomType.roomPrice from room join roomType on room.roomTypeId = roomType.id where room.status = 'avail' and roomType.name = '" + comboBox1.Text + "'", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void label18_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
            UserSelected.id = 0;
            UserSelected.name = "";
        }
    }
}
