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
    public partial class ReportCheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command = new SqlCommand();
        public ReportCheckIn()
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

        private void button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from room join roomType on room.roomTypeId = roomType.id join reservationRoom on reservationRoom.roomId = room.id join reservation on reservationRoom.reservationId = reservation.id where reservationRoom.checkInDateTime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and reservationRoom.checkOutDateTime <= '" + Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            dataGridView1.Columns[10].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                application.Application.Workbooks.Add(Type.Missing);
                for(int i = 0; i< dataGridView1.ColumnCount; i++)
                {
                    application.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for(int i =0; i < dataGridView1.RowCount; i++)
                {
                    for(int j=0; j < dataGridView1.ColumnCount; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                application.Columns.AutoFit();
                application.Visible = true;
            }
         }
    }
}
