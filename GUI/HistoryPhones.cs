using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class HistoryPhones : Form
    {
        public HistoryPhones(string[] args)
        {
            string data = args[0].Split(':')[1];
            InitializeComponent();
            pictureBox1.Visible = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x: Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            StartInitializeTasks(data);

        }

        private async void StartInitializeTasks(string args)
        {
            Task t0 = Task.Run(openAsteriskConnection);
            try
            {
                await t0;
                Init(args);
                pictureBox1.Visible = false;

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных MooxAgent.\nПроверьте наличие интернет-соединения и VPN.");
            }
        }

        private void openAsteriskConnection()
        {
            try
            {
                connectionInfo.AsteriskConnection.Open();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения к базе данных MooxAgent.\nПроверьте наличие интернет-соединения и VPN.");
            }
        }

        private void Init(string phone)
        {
            DataGridViewLinkColumn dgvLink = new DataGridViewLinkColumn();
            dgvLink.DataPropertyName = "Ссылка";
            dgvLink.Name = "Ссылка";
            dataGridView1.Columns.Add(dgvLink);
            dataGridView1.DataSource = GetData(phone.Substring(phone.Length - 10, 10));

            dataGridView1.Columns[0].Width = 123;
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[2].Width = 160;
            dataGridView1.Columns[3].Width = 160;
            dataGridView1.Columns[4].Width = 120;

        }

        private DataTable GetData(string phone)
        {
            string SqlExpression = "SELECT calldate as 'Дата звонка', duration as 'Длит.', src as 'Кто звонил', dst as 'Кому звонил', cnam as 'ФИО', CONCAT('http://10.10.0.23:3000/download?filename=', uniqueid)  as 'Ссылка' FROM asteriskcdrdb.cdr WHERE year(calldate) >= 2022 and (src like '%@client%' or dst like '%@client%') order by calldate desc;"; //and(cnum = @manager or dst = @manager)
            SqlExpression = SqlExpression.Replace("@client", phone);
            MySqlDataAdapter AsteriskCallsAdapter = new MySqlDataAdapter(SqlExpression, connectionInfo.AsteriskConnection);
            DataTable AsteriskCallsTable = new DataTable();

            AsteriskCallsAdapter.Fill(AsteriskCallsTable);

            return AsteriskCallsTable;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                _ = Process.Start(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
