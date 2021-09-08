namespace Garant_R_NEW
{
    partial class Form2
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
            this.comBox = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.baudRateList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openPort = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comList = new System.Windows.Forms.ComboBox();
            this.comBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // comBox
            // 
            this.comBox.Controls.Add(this.textBox1);
            this.comBox.Controls.Add(this.label5);
            this.comBox.Controls.Add(this.baudRateList);
            this.comBox.Controls.Add(this.label4);
            this.comBox.Controls.Add(this.label2);
            this.comBox.Controls.Add(this.openPort);
            this.comBox.Controls.Add(this.label3);
            this.comBox.Controls.Add(this.label1);
            this.comBox.Controls.Add(this.comList);
            this.comBox.Location = new System.Drawing.Point(16, 15);
            this.comBox.Margin = new System.Windows.Forms.Padding(4);
            this.comBox.Name = "comBox";
            this.comBox.Padding = new System.Windows.Forms.Padding(4);
            this.comBox.Size = new System.Drawing.Size(461, 177);
            this.comBox.TabIndex = 1;
            this.comBox.TabStop = false;
            this.comBox.Text = "Работа с COM-портом";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 42);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(269, 22);
            this.textBox1.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 22);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Введите пароль:";
            // 
            // baudRateList
            // 
            this.baudRateList.FormattingEnabled = true;
            this.baudRateList.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.baudRateList.Location = new System.Drawing.Point(8, 140);
            this.baudRateList.Margin = new System.Windows.Forms.Padding(4);
            this.baudRateList.Name = "baudRateList";
            this.baudRateList.Size = new System.Drawing.Size(269, 24);
            this.baudRateList.TabIndex = 10;
            this.baudRateList.Text = "115200";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Скорость:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Tomato;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(292, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 124);
            this.label2.TabIndex = 8;
            // 
            // openPort
            // 
            this.openPort.Location = new System.Drawing.Point(327, 42);
            this.openPort.Margin = new System.Windows.Forms.Padding(4);
            this.openPort.Name = "openPort";
            this.openPort.Size = new System.Drawing.Size(123, 124);
            this.openPort.TabIndex = 6;
            this.openPort.Text = "Открыть";
            this.openPort.UseVisualStyleBackColor = true;
            this.openPort.Click += new System.EventHandler(this.openPort_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 17);
            this.label3.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Выберите COM-порт:";
            // 
            // comList
            // 
            this.comList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.comList.FormattingEnabled = true;
            this.comList.Location = new System.Drawing.Point(8, 91);
            this.comList.Margin = new System.Windows.Forms.Padding(4);
            this.comList.Name = "comList";
            this.comList.Size = new System.Drawing.Size(269, 24);
            this.comList.TabIndex = 0;
            this.comList.Text = "Не выбран";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 203);
            this.Controls.Add(this.comBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "Настройка соединения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.comBox.ResumeLayout(false);
            this.comBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox comBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button openPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comList;
        private System.Windows.Forms.ComboBox baudRateList;
    }
}