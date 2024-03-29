﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;
using System.Text;
using OpenNetFolder;
using System.Diagnostics;

namespace GUI
{
    public partial class SendDocs : Form
    {


        static Dictionary<string, string> IdMails = new Dictionary<string, string>();
        static Dictionary<string, List<string>> Contracts = new Dictionary<string, List<string>>();
        public static Dictionary<string, string> DocPaths = new Dictionary<string, string>();

        static List<string> SelectedContracts = new List<string>();
        static List<string> SelectedCompanies = new List<string>();
        static List<string> DocTypes = new List<string>();

        public static DateTime Date;
        static string Entity;
        static string MaiL;

        public SendDocs(string[] args)
        {
            Entity = ParseInput(args);
            InitializeComponent();
            LbMonthCalendarShowSelected.Text = String.Format("Вы выбрали: {0}", DateTime.Now.ToLongDateString());
            this.StartPosition = FormStartPosition.Manual;
            OpenCRMConnection();
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            installUpdater();
        }

        private string ParseInput(string[] args)
        {
            
            string data = args[0].Split(':')[1];
            //string pattern = @"%7B(\w+\d+?[A-Z][^a-z%&_\/]+)%7D";
            string pattern = @"%7B(.*)%7D";

            //Regex rg = new Regex(pattern);

            var m = Regex.Match(data, pattern).Groups[1];
            string res = m.Value.ToString();
            //MessageBox.Show(res);

            //Debug.WriteLine(res);
            //Debug.WriteLine(data);
            //string[] result = Regex.Split(data, pattern,RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));
            /*foreach (string res in result)
            {
                MessageBox.Show(res);
            }*/
            /*MatchCollection matchedAuthors = rg.Matches(data);
            for (int count = 0; count < matchedAuthors.Count; count++)
            {
                MessageBox.Show(matchedAuthors[count].Value);
            }*/
            //return res;
            return res;
        }

        private void OpenCRMConnection()
        {
            //Пытаемся открыть подключение
            try
            {
                connectionInfo.CRMConnection.Open();
            }
            //Если подключение не открывается, выводим ошибку
            catch
            {
                //MessageBox.Show(connectionInfo.CRMConnectionString);
                MessageBox.Show("Ошибка подключения к базе данных CRM.\nПроверьте наличие интернет-соединения и VPN.");
            }
        }

        private bool HasRecords(DataTable dataSet)
        {
            if (dataSet != null)
            {
                return dataSet.AsEnumerable().Any(dt => dataSet.Rows.Count > 0);
            }
            return false;
        }

        private static DataTable GetMails(List<string> SelectedItems)
        {
            var filteredDict = IdMails.Where(kvp => SelectedItems.Contains(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            DataTable Table = new DataTable();
            Table.Columns.Add("mail", typeof(string));

            foreach (var item in filteredDict)
            {
                DataRow dr = Table.NewRow();
                item.Value.Split(';').Distinct<string>().ToList().ForEach(ml => Table.Rows.Add(dr["mail"] = ml));
            }
            return Table;
        }

        private void MainLoad()
        {
            //MessageBox.Show(await CreateDocumentRequest.MakeRequest(TypeOfDocs[0], entity));

            dataGridView1.Visible = true;
            button1.Enabled = true;
            button1.Visible = true;


            string firstSqlExp = "select Account.AccountId, EMailAddress1, EMailAddress2, EMailAddress3 from Account where Account.AccountId = '@client'";
            firstSqlExp = firstSqlExp.Replace("@client", Entity);
            System.Data.SqlClient.SqlDataAdapter CRMfirstAdapter = new System.Data.SqlClient.SqlDataAdapter(firstSqlExp, connectionInfo.CRMConnection);
            DataTable RegardingObjId = new DataTable();
            try
            {
                CRMfirstAdapter.Fill(RegardingObjId);
            }
            catch
            {
                MessageBox.Show("Ошибка получения ID");
                return;
            }

            if (!HasRecords(RegardingObjId))
            {
                MessageBox.Show("Записи не обнаружены");
                return;
            }

            foreach (DataRow row in RegardingObjId.Rows)
            {
                IdMails.Add("'" + row[0].ToString() + "'", $"{row[1]};{row[2]};{row[3]}");
            }

            // 2-ой запрос 
            string SecondSqlExp = "select leasing_name as \"Договора\", leasing_customeridName as \"Компания\", leasing_contract.leasing_customerid, leasing_contractid, quote.leasing_brendidname + ' ' + quote.leasing_modelidname as \"ПЛ\" from leasing_contract inner join quote with(nolock) on quote.quoteid = leasing_contract.gpbl_actual_quote where leasing_contract.statecode = 0 and leasing_percent_of_avanspayment >= 10 and leasing_contract.leasing_customerid in (@client)";
            SecondSqlExp = SecondSqlExp.Replace("@client", String.Join(",", IdMails.Keys.ToList()));
            System.Data.SqlClient.SqlDataAdapter CRMsecondAdapter = new System.Data.SqlClient.SqlDataAdapter(SecondSqlExp, connectionInfo.CRMConnection);
            DataTable ContractsTable = new DataTable();
            //Заполняем таблицу с данными
            CRMsecondAdapter.Fill(ContractsTable);

            foreach (DataRow row in ContractsTable.Rows)
            {
                Contracts.Add(row[0].ToString(), new List<string> { row[2].ToString(), row[3].ToString() });
            }
            ContractsTable.Columns.Remove("leasing_customerid");
            ContractsTable.Columns.Remove("leasing_contractid");
            //заполняем форму 

            DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Выбор",
                Width = 30,
                Name = "checkBoxColumn",
                DataPropertyName = "IsSelected"
            };
            dataGridView1.Columns.Insert(0, dataGridViewCheckBoxColumn);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.DataSource = ContractsTable;
            dataGridView1.Columns[1].Width = 110;
            dataGridView1.Columns[2].Width = 140;
            dataGridView1.Columns[3].Width = 160;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //var kaka = from CheckBox ch in СhooseTypeOfDocuments.Controls where ch.Checked select ch;
            List<string> ContractsIds = new List<string>();

            StringBuilder BuildMessage = new StringBuilder();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxColumn"].Value);
                if (isSelected)
                {
                    //Debug.WriteLine(isSelected);
                    string IterCompany = row.Cells["Компания"].Value.ToString();
                    string IterContract = row.Cells["Договора"].Value.ToString();

                    if (BuildMessage.ToString().Contains(IterCompany))
                    {
                        BuildMessage.Append(IterContract + "; ");
                    }
                    else
                    {
                        BuildMessage.Append(@"</B></b></H3><b></b>" + IterCompany + ": " + IterContract + "; ");
                    }


                    SelectedContracts.Add(IterContract);
                    SelectedCompanies.Add(IterCompany);
                    ContractsIds.Add("'" + Contracts[IterContract][0] + "'");
                }
            }
            MaiL = BuildMessage.ToString();
            if (ContractsIds.Count > 0)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Visible = false;
                pictureBox1.Visible = true;
                button1.Enabled = false;
                button1.Visible = false;
                button2.Visible = true;

                await DiskConnection.Main(args: new string[1] { connectionInfo.NetFolderCredentials });
                //Debug.WriteLine(string.Join(", ",Contracts.Keys.ToArray()));

                DocPaths = await CreateDocumentRequest.MakeRequest(DocTypes, Contracts, SelectedContracts);


                pictureBox1.Visible = false;

                dataGridView1.Visible = true;
                dataGridView1.DataSource = GetMails(ContractsIds);
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[1].Width = 420;

                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Отсутствует выбор");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> MailList = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["checkBoxColumn"].Value);
                if (isSelected)
                {
                    MailList.Add(row.Cells["mail"].Value.ToString());
                }
            }
            //List<CheckBox> list = new List<CheckBox>();
            //СhooseTypeOfDocuments.Controls.ForEach(ch => if ch.list.Add(ch));

            if (MailList.Count > 0)
            {

                button2.Enabled = false;
                dataGridView1.Visible = false;
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();

                string Perenos = "</B></b></H3></H3><b></b>";
                string Greetings = @"<p><span style=""color:#002e67""><H3><B> Добрый день, Уважаемый Лизингополучатель! </B></b></H3><b></b></p>";
                string End = $@"В случае возникновения вопросов свяжитесь с нами по телефону: 8-800-302-25-21 {Perenos} График работы: {Perenos} • Понедельник - четверг с 7:00 ч. до 18:00 ч. по МСК; {Perenos} • Пятница с 7:00 ч. до 16:45 ч. по МСК; {Perenos} • Суббота и воскресенье выходной. {Perenos} </p> <p> С уважением, {Perenos} Ваша компания ООО «Газпромбанк Автолизинг»";

                if (SelectedContracts.Count > 1)
                {
                    string mail = $"{Greetings} {Perenos} Направляем Вам {string.Join(", ", DocTypes.ToArray())} по договорам лизинга: {MaiL} </B></b></H3><b></b> (см.вложение). {Perenos} <p> {End} </p>";
                    pictureBox1.Visible = true;
                    Task.Run(() =>
                    MailEvents.SendMail($"{string.Join(", ", DocTypes.ToArray())} для «{String.Join(", ", SelectedCompanies.Distinct())}» от ООО Газпромбанк Автолизинг", mail, String.Join(";", MailList))
                    );
                }
                else
                {
                    string mail = $"{Greetings} {Perenos} Направляем Вам {string.Join(", ", DocTypes.ToArray())} по договору лизинга: {MaiL} </B></b></H3><b></b> (см.вложение). {Perenos} <p> {End} </p>";
                    pictureBox1.Visible = true;
                    Task.Run(() =>
                    MailEvents.SendMail($"{string.Join(", ", DocTypes.ToArray())} для «{String.Join(", ", SelectedCompanies.Distinct())}» от ООО Газпромбанк Автолизинг", mail, String.Join(";", MailList))
                    );
                }
            }
            else
            {
                MessageBox.Show("Отсутствует выбор");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //DocTypes = DocsCheckList.CheckedItems.Cast<string>().ToList();
            CheckVersionPO();
            //Debug.WriteLine(String.Format("#{0}#", Application.ProductName));
            Date = monthCalendar.SelectionStart;
            if (radioButton1.Checked)
            {
                DocTypes.Add("Акт-сверки");
            }
            else if (radioButton2.Checked)
            {
                DocTypes.Add("Cчет на пени");
            }
            else
            {
                MessageBox.Show("Отсутствует выбор");
                return;
            }
            button3.Enabled = false;
            button3.Visible = false;
            monthCalendar.Visible = false;
            //DocsCheckList.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            LbDocsCheckListDescription.Visible = false;
            LbDocsCheckListShowSelected.Visible = false;
            LbMonthCalendarShowSelected.Visible = false;
            LbMonthCalendardecription.Visible = false;
            MainLoad();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            LbMonthCalendarShowSelected.Text = String.Format("Вы выбрали: {0}", e.Start.ToLongDateString());
        }

        private void DocsCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LbDocsCheckListShowSelected.Text = String.Format("Вы выбрали: {0}", String.Join(", ", DocsCheckList.CheckedItems.Cast<string>().ToList()));
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LbDocsCheckListShowSelected.Text = String.Format("Вы выбрали: {0}", radioButton1.Text);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LbDocsCheckListShowSelected.Text = String.Format("Вы выбрали: {0}", radioButton2.Text);
        }

        private static void installUpdater()
        {
            try
            {
                string curFile = @"C:\Users\@user\AppData\Roaming\ProgramUpdater\ProgramUpdater.exe".Replace("@user", Environment.UserName.ToString());
                if (!File.Exists(curFile))
                {
                    Directory.CreateDirectory(@"C:\Users\@user\AppData\Roaming\ProgramUpdater".Replace("@user", Environment.UserName.ToString()));
                    //Объявляем переменную с путем к обновленной версии
                    //Создаем подключение
                    string DKSConnectionString = "Data Source=DTC01-SQLTST01\\TEST;Initial Catalog = klecov;User ID=kmpl;Password = 6fR2zvmf5TSp?SG;MultipleActiveResultSets = True";

                    System.Data.SqlClient.SqlConnection DKSConnection = new System.Data.SqlClient.SqlConnection(DKSConnectionString);
                    DKSConnection.Open();

                    string newVersionPath = string.Empty;

                    //Получаем путь к актуальной версии ПО
                    System.Data.SqlClient.SqlCommand GetPathCMD = DKSConnection.CreateCommand();

                    GetPathCMD.CommandText = "SELECT ProductPath FROM klecov.dbo.DKSProjectsVersions WHERE ProductName = '@programName'".Replace("@programName", "ProgramUpdater");

                    newVersionPath = GetPathCMD.ExecuteScalar().ToString();

                    //Объявляем массив со всеми файлами в папке с обновленной версией
                    string[] newVersionFilePaths = Directory.GetFiles(newVersionPath);

                    //Перебираем все файлы
                    foreach (var newVersionFileName in newVersionFilePaths)
                    {
                        //Копируем файлы в путь, переданный в данный метод
                        File.Copy(newVersionFileName.ToString(), @"C:\Users\@user\AppData\Roaming\ProgramUpdater\".Replace("@user", Environment.UserName.ToString()) + newVersionFileName.ToString().Replace(newVersionPath, ""), true);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void CheckVersionPO()
        {
            string? SoftwareVersion = new SqlExecuter(query: Queries.ActualProductVersion, Replacement: string.Empty).ReturnStrRes();
            //Debug.WriteLine(SoftwareVersion);
            
            //Debug.WriteLine(SoftwareVersion);
            if (System.Windows.Forms.Application.ProductVersion.ToString() != SoftwareVersion)
            {
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.FileName = @"C:\Users\@user\AppData\Roaming\ProgramUpdater\ProgramUpdater.exe".Replace("@user", Environment.UserName.ToString());
                proc.StartInfo.Arguments = "@path @program".Replace("@path", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString()).Replace("@program", Application.ProductName.ToString());
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                proc.Start();
                Application.Exit();
                Application.Exit();

            }
        }
    }

}
