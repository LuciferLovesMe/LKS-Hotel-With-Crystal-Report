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
    public partial class Reservation : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        DataTable table = new DataTable();
        
        public Reservation()
        {
            InitializeComponent();
            loadgrid();

            string name = Model.name;
            string[] getname = name.Split(' ');
            lbladmin.Text = getname[0];
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

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from customer", connection);
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            this.Hide();
            add.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            UserSelected.id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            UserSelected.name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(UserSelected.id != 0)
            {
                SelectRoom select = new SelectRoom();
                this.Hide();
                select.ShowDialog();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from customer where name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.data(com);
        }
    }
}
