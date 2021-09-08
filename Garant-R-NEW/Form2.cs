using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace Garant_R_NEW
{
    public partial class Form2 : Form
    {
        Form1 F1;
        string accessPass = "";
        public Form2(Form1 F1_)
        {
            F1 = F1_;
            F1.Owner = this;
            InitializeComponent();

            //Получаем список доступных COM-портов
            String[] comNames = SerialPort.GetPortNames();
            foreach (string currentPort in comNames)
            {
                comList.Items.Add(currentPort);
            }

            if (F1.port.IsOpen)
            {
                comList.Text = F1.port.PortName;
                label2.BackColor = Color.LimeGreen;
                openPort.Text = "Закрыть";
                baudRateList.Text = F1.port.BaudRate.ToString();
                BufferClass.DataBuffer1 = true;
                textBox1.Enabled = false;
            }

            //Подписываемся на событие ValueChanged статического класса
            BufferClass1.ValueChanged += (sender1, e1) =>
            {
                accessPass = BufferClass1.PasswordChecked;
                if (accessPass == "checkedPass true")
                {
                    BufferClass.DataBuffer1 = true;
                    try
                    {
                        //Запись последнего открытого COM-порта в файл
                        StreamWriter SW = new StreamWriter(new FileStream(Directory.GetCurrentDirectory() + "/com_number.txt", FileMode.OpenOrCreate, FileAccess.Write));
                        SW.WriteLine(comList.Text);
                        SW.Close();
                    }
                    catch
                    { }
                                 

                    this.Close();
                }
                else
                {
                    F1.port.Close();
                    MessageBox.Show("Пароль неверный!");
                }
            };

            this.BringToFront();
        }

        private void openPort_Click(object sender, EventArgs e)
        {
            if (!F1.port.IsOpen)
            {
                try
                {
                    F1.port.ReadBufferSize = 2000000;
                    F1.port.PortName = comList.Text;
                    F1.port.BaudRate = Convert.ToInt32(baudRateList.Text);
                    F1.port.Parity = Parity.None;
                    F1.port.DataBits = 8;
                    F1.port.StopBits = StopBits.One; 
                    F1.port.StopBits = StopBits.One; 
                    F1.port.Encoding = Encoding.GetEncoding(1251);
                    F1.port.Open();
                    F1.alarmStorage = 0;
                    F1.bosFaultStorage = 0;
                    F1.burFaultStorage = 0;
                    F1.autoStorage = 0;
                    F1.voltStorage = 0;
                    F1.port.DataReceived += new
                        SerialDataReceivedEventHandler(F1.DataReceivedHandler);

                    if (F1.port.IsOpen)
                    {
                        BufferClass.DataBuffer = textBox1.Text;
                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть COM-порт");
                }
            }
            else
            {
                try
                {
                    F1.port.Close();
                    if (!F1.port.IsOpen)
                    {
                        label2.BackColor = Color.Tomato;
                        openPort.Text = "Открыть";
                        BufferClass.DataBuffer1 = false;
                        textBox1.Enabled = true;
                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно закрыть COM-порт");
                }
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((F1.port.IsOpen != true) || (BufferClass1.PasswordChecked != "checkedPass true"))
            {
                Application.Exit();
            }
            else
            {
                F1.Show();
                F1.Enabled = true;
            }
        }
    }
}
