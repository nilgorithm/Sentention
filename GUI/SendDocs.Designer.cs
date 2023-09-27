namespace GUI
{
    partial class SendDocs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendDocs));
            dataGridView1 = new DataGridView();
            DocsCheckList = new CheckedListBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            pictureBox1 = new PictureBox();
            LbDocsCheckListDescription = new Label();
            LbMonthCalendarShowSelected = new Label();
            LbDocsCheckListShowSelected = new Label();
            LbMonthCalendardecription = new Label();
            monthCalendar = new MonthCalendar();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(501, 247);
            dataGridView1.TabIndex = 0;
            dataGridView1.Visible = false;
            // 
            // DocsCheckList
            // 
            DocsCheckList.CheckOnClick = true;
            DocsCheckList.FormattingEnabled = true;
            DocsCheckList.Items.AddRange(new object[] { "Акт-сверки", "Cчет на пени" });
            DocsCheckList.Location = new Point(0, 17);
            DocsCheckList.Name = "DocsCheckList";
            DocsCheckList.Size = new Size(295, 238);
            DocsCheckList.TabIndex = 1;
            DocsCheckList.Visible = false;
            DocsCheckList.SelectedIndexChanged += DocsCheckList_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(507, 7);
            button1.Name = "button1";
            button1.Size = new Size(85, 42);
            button1.TabIndex = 2;
            button1.Text = "Подтвердить выбор";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(507, 6);
            button2.Name = "button2";
            button2.Size = new Size(85, 40);
            button2.TabIndex = 3;
            button2.Text = "Отправить";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(507, 6);
            button3.Name = "button3";
            button3.Size = new Size(85, 43);
            button3.TabIndex = 4;
            button3.Text = "Подтвредить выбор";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(60, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(206, 205);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            // 
            // LbDocsCheckListDescription
            // 
            LbDocsCheckListDescription.AutoSize = true;
            LbDocsCheckListDescription.BackColor = Color.Transparent;
            LbDocsCheckListDescription.Location = new Point(0, 0);
            LbDocsCheckListDescription.Name = "LbDocsCheckListDescription";
            LbDocsCheckListDescription.Size = new Size(231, 15);
            LbDocsCheckListDescription.TabIndex = 7;
            LbDocsCheckListDescription.Text = "Выберите тип рассылаемых документов";
            // 
            // LbMonthCalendarShowSelected
            // 
            LbMonthCalendarShowSelected.AutoSize = true;
            LbMonthCalendarShowSelected.BackColor = Color.Transparent;
            LbMonthCalendarShowSelected.Location = new Point(0, 258);
            LbMonthCalendarShowSelected.Name = "LbMonthCalendarShowSelected";
            LbMonthCalendarShowSelected.Size = new Size(78, 15);
            LbMonthCalendarShowSelected.TabIndex = 9;
            LbMonthCalendarShowSelected.Text = "Вы выбрали:";
            // 
            // LbDocsCheckListShowSelected
            // 
            LbDocsCheckListShowSelected.AutoSize = true;
            LbDocsCheckListShowSelected.BackColor = SystemColors.Control;
            LbDocsCheckListShowSelected.Location = new Point(0, 55);
            LbDocsCheckListShowSelected.Name = "LbDocsCheckListShowSelected";
            LbDocsCheckListShowSelected.Size = new Size(78, 15);
            LbDocsCheckListShowSelected.TabIndex = 10;
            LbDocsCheckListShowSelected.Text = "Вы выбрали:";
            // 
            // LbMonthCalendardecription
            // 
            LbMonthCalendardecription.AutoSize = true;
            LbMonthCalendardecription.BackColor = SystemColors.Control;
            LbMonthCalendardecription.Location = new Point(0, 81);
            LbMonthCalendardecription.Name = "LbMonthCalendardecription";
            LbMonthCalendardecription.Size = new Size(221, 15);
            LbMonthCalendardecription.TabIndex = 7;
            LbMonthCalendardecription.Text = "Выберит дату для загрузки документов";
            // 
            // monthCalendar
            // 
            monthCalendar.Location = new Point(0, 93);
            monthCalendar.Name = "monthCalendar";
            monthCalendar.TabIndex = 8;
            monthCalendar.DateChanged += monthCalendar1_DateChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(0, 17);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(87, 19);
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "Акт-сверки";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(0, 35);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(97, 19);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "Cчет на пени";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // SendDocs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 284);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(LbMonthCalendardecription);
            Controls.Add(LbDocsCheckListShowSelected);
            Controls.Add(LbMonthCalendarShowSelected);
            Controls.Add(monthCalendar);
            Controls.Add(LbDocsCheckListDescription);
            Controls.Add(pictureBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(DocsCheckList);
            Controls.Add(dataGridView1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SendDocs";
            Text = "SendDocs";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private CheckedListBox DocsCheckList;
        private Button button1;
        private Button button2;
        private Button button3;
        private PictureBox pictureBox1;
        private Label LbDocsCheckListDescription;
        private Label label2;
        private Label LbMonthCalendarShowSelected;
        private Label LbDocsCheckListShowSelected;
        private Label LbMonthCalendardecription;
        private MonthCalendar monthCalendar;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
    }
}