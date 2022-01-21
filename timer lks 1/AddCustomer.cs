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
    public partial class AddCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        string gender;
        public AddCustomer()
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

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || radioButton1.Checked == false && radioButton2.Checked == false || dateTimePicker1.Value == null)
            {
                MessageBox.Show("All Fields Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(textBox2.TextLength < 16)
            {
                MessageBox.Show("NIK Must be 16 Chars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(textBox4.TextLength < 11)
            {
                MessageBox.Show("Phone Number at Least Has 11 Chars", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                int age = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                command = new SqlCommand("insert into customer(name, nik, email, gender, phoneNumber, dateOfBirth, age, created_at) values('" + textBox1.Text + "', "+Convert.ToInt64(textBox2.Text)+", '"+textBox3.Text+"', '"+gender+"', "+Convert.ToInt64(textBox4.Text)+", '"+dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss")+"', "+age+", getdate())", connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Reservation reservation = new Reservation();
                    this.Hide();
                    reservation.ShowDialog();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                gender = "Male";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                gender = "Female";
            }
        }
    }
}
