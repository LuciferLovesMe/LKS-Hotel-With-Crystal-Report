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
    public partial class ReportGuest : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        public ReportGuest()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;

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

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var series in chart1.Series)
            {
                series.Points.Clear();
            }

            command = new SqlCommand("select month, count(*) as num from reservation where year = "+dateTimePicker1.Value.ToString("yyyy")+ " group by month order by case when month = 'January' then 1 when month = 'February' then 2 when month = 'March' then 3 when month = 'April' then 4 when month = 'May' then 5 when month = 'June' then 6 when month = 'July' then 7 when month = 'August' then 8 when month = 'September' then 9 when month = 'October' then 10 when month = 'November' then 11 when month = 'December' then 12 else null end", connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chart1.Series["Total"].Points.AddXY(reader["Month"].ToString(), Convert.ToInt32(reader["num"]));
                }
                connection.Close();
            }
            catch (Exception)
            {

                throw;
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
