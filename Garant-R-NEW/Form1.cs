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
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;

namespace Garant_R_NEW
{
    public partial class Form1 : Form
    {
        public SerialPort port = new SerialPort();
        public int alarmStorage, bosFaultStorage, burFaultStorage, autoStorage, voltStorage;


        string passTaken = "";

        char[] h = { '0', '0' };
        char[] m = { '0', '0' };
        char[] s = { '0', '0' };

        int h_value, m_value, s_value;
        string buffer = "";
        string str = "";

        bool archiveReading = false;
        bool bosReading = false;

        bool automationBlocked = false;
        bool automationProlonged = false;
        bool fire = false;
        bool start = false;
        bool attention = false;
        bool bosBroken = false;
        bool buBroken = false;
        bool transceiverBroken = false;
        bool memoryBroken = false;
        bool soundOn = false;
        bool testOn = false;
        bool btBroken = false;
        bool btConnected = false;
        bool supply_1_Low = false;
        bool supply_1_High = false;
        bool supply_2_Low = false;
        bool supply_2_High = false;

        public Form1()
        {
            InitializeComponent();

            transceiverIndicator_new.Text = "";
            memoryIndicator_new.Text = "";
            btIndicator_new.Text = "";
            testIndicator_new.Text = "";
            soundIndicator_new.Text = "";
            supplyIndicator_1_new.Text = "";
            supplyIndicator_2_new.Text = "";
            attentionIndicator_new.Text = "";
            fireIndicator_new.Text = "";
            startIndicator_new.Text = "";
            bosFaultIndicator_new.Text = "";
            buFaultIndicator_new.Text = "";
            autoIndicator_new.Text = "";
            blockAutoIndicator_new.Text = "";

            //Подписываемся на событие ValueChanged статического класса
            BufferClass.ValueChanged += (sender1, e1) =>
            {
                passTaken = BufferClass.DataBuffer;
                sendCommand("checkPass " + passTaken);
            };

            BufferClass.ValueChanged1 += (sender1, e1) =>
            {
                if (BufferClass.DataBuffer1)
                {
                    BB_green(btIndicator_new);
                    btIndicator_new.Text = "CОЕДИНЕНИЕ В НОРМЕ";

                }
                else
                {
                    BB_yellow(btIndicator_new);
                    btIndicator_new.Text = "СОЕДИНЕНИЕ НЕ УСТАНОВЛЕНО";
                }
            };

            if (this.Height > SystemInformation.VirtualScreen.Height)
            {
                /*
                tableLayoutPanel1.Height = (SystemInformation.VirtualScreen.Height * 9) / 10 - 419;
                dataFromPort3.Height = (SystemInformation.VirtualScreen.Height * 9) / 10 - 287;
                dataFromPort4.Height = (SystemInformation.VirtualScreen.Height * 9) / 10 - 344;
                dataFromPort2.Height = (SystemInformation.VirtualScreen.Height * 9) / 10 - 515;*/
                this.Height = (SystemInformation.VirtualScreen.Height * 9) / 10;
                this.Width = this.Width + 18;
                this.Top = 0;
            }

            lbCOMinfo.Text = port.PortName + " " + port.BaudRate + " bps";
        }

        private void DataFromPort_TextChanged(object sender, EventArgs e)
        {
            dataFromPort.SelectionStart = dataFromPort.Text.Length;
            dataFromPort.ScrollToCaret();
            dataFromPort.Refresh();
        }

        private void DataFromPort2_TextChanged(object sender, EventArgs e)
        {
            dataFromPort2.SelectionStart = dataFromPort2.Text.Length;
            dataFromPort2.ScrollToCaret();
            dataFromPort2.Refresh();
        }

        private void DataFromPort3_TextChanged(object sender, EventArgs e)
        {
            dataFromPort3.SelectionStart = dataFromPort3.Text.Length;
            dataFromPort3.ScrollToCaret();
            dataFromPort3.Refresh();
        }

        private void DataFromPort4_TextChanged(object sender, EventArgs e)
        {
            dataFromPort4.SelectionStart = dataFromPort4.Text.Length;
            dataFromPort4.ScrollToCaret();
            dataFromPort4.Refresh();
        }

        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Enabled = false;
            Form2 F2 = new Form2(this);
            F2.Show();
        }

        //Обработчкик события получения нового сообщение из COM-порта
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (archiveReading == true)
            {
                System.Threading.Thread.Sleep(100);
                string indata = (sender as SerialPort).ReadExisting();
                buffer += indata;
                while (buffer.Contains("\r"))
                {
                    str = buffer.Remove(buffer.IndexOf("\r"));
                    buffer = buffer.Remove(0, buffer.IndexOf("\r") + 1);
                    this.BeginInvoke(new SetTextDeleg(si_DataReceived1), new object[] { str });
                }
            }
            else
            {
                System.Threading.Thread.Sleep(100);
                string indata = (sender as SerialPort).ReadExisting();
                buffer += indata;
                while (buffer.Contains("\r"))
                {
                    str = buffer.Remove(buffer.IndexOf("\r"));
                    buffer = buffer.Remove(0, buffer.IndexOf("\r") + 1);
                    this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { str });
                }
            }
        }

        //Делегат от COM-порта к интерфейсу
        private void si_DataReceived(string data)
        {
            parcer(data);123
        }

        private void si_DataReceived1(string data)
        {
            parcerArchive(data);
        }

        private delegate void SetTextDeleg(string text);

        /// <summary>
        /// Отправка команд на COM-порт
        /// </summary>
        /// <param name="comm"></param>
        public void sendCommand(string comm)
        {
            try
            {
                port.Write(comm + "\r\n");
            }
            catch
            {
                MessageBox.Show("Не удалось отправить команду!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendCommand("Test");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendCommand("Sound");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendCommand("resAddr " + setID.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                sendCommand("wrAddr " + setID.Text + " " + setRoomNumber.Text + " " + setChannelNumber.Text + " " + fireDetectorsClass.Text.Remove(0, 1));
            }
            catch
            {
                MessageBox.Show("Параметры не заданы!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendCommand("ClrAddrBos");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendCommand("SetAddrBos");
        }

        private void getTime_Click(object sender, EventArgs e)
        {
            //DateTime localTime = DateTime.Now;
            //dataFromPort2.Text += "[" + localTime.ToString() + "]: ";
            //dataFromPort2.Text += "Дата " + date + "\r\n";
            sendCommand("Date");
            sendCommand("Time");
        }

        private void setDate_Click(object sender, EventArgs e)
        {
            sendCommand("Date " + date.Value.Day.ToString() + " " + date.Value.Month.ToString() + " " + date.Value.Year.ToString().Remove(0, 2));
            sendCommand("Time " + date.Value.Hour.ToString() + " " + date.Value.Minute.ToString() + " " + date.Value.Second.ToString());
        }

        private void blockAuto_Click(object sender, EventArgs e)
        {
            if (automationBlocked)
            {
                sendCommand("UnlockAuto");
            }
            else
            {
                sendCommand("LockAuto");
            }
        }

        private void commandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendCommand("Help");
        }

        public void timeParcer(string data)
        {
            data.CopyTo(data.LastIndexOf(":") - 5, h, 0, 2);
            data.CopyTo(data.LastIndexOf(":") - 2, m, 0, 2);
            data.CopyTo(data.LastIndexOf(":") + 1, s, 0, 2);

            string h_str = new string(h);
            string m_str = new string(m);
            string s_str = new string(s);

            h_value = Convert.ToInt16(h_str);
            m_value = Convert.ToInt16(m_str);
            s_value = Convert.ToInt16(s_str);

            if (data.Contains("Время устройства -"))
            {
                if (s_value == 59)
                {
                    s_value = 0;
                    if (m_value == 59)
                    {
                        m_value = 0;
                        if (h_value == 23)
                        {
                            h_value = 0;

                        }
                        else
                        {
                            m_value += 1;
                        }
                    }
                    else
                    {
                        m_value += 1;
                    }
                }
                else
                {
                    s_value += 1;
                }
            }

            string time = "";

            if (h_value < 10)
            {
                time += "0" + h_value.ToString();
            }
            else
            {
                time += h_value.ToString();
            }

            time += ":";

            if (m_value < 10)
            {
                time += "0" + m_value.ToString();
            }
            else
            {
                time += m_value.ToString();
            }

            time += ":";

            if (s_value < 10)
            {
                time += "0" + s_value.ToString();
            }
            else
            {
                time += s_value.ToString();
            }

            timeLabel.Text = "Время устройства - " + time;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeParcer(timeLabel.Text);
        }

        private void setStartDelayTime_Click(object sender, EventArgs e)
        {
            sendCommand("SetDelayStart " + startDelayTime.Text);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            sendCommand("Reset");
        }

        private void setNewPassword_Click(object sender, EventArgs e)
        {
            sendCommand("Change_pass " + newPassword.Text);
        }

        private void startAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Подтверждение пуска", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                sendCommand("Start_all");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form4 F4 = new Form4(this);
            F4.Show();
            /*
            Form3 F3 = new Form3(this);
            F3.Show();
            */
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataFromPort4.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            sendCommand("Read_ev 1");
            //askArchiveForPeriod " + 
            //dateTimePicker1.Value.Day.ToString() + " " + dateTimePicker1.Value.Month.ToString() + " " + dateTimePicker1.Value.Year.ToString() + " " + dateTimePicker1.Value.Hour.ToString() + " " + dateTimePicker1.Value.Minute.ToString() + " " +
            //dateTimePicker2.Value.Day.ToString() + " " + dateTimePicker2.Value.Month.ToString() + " " + dateTimePicker2.Value.Year.ToString() + f" " + dateTimePicker2.Value.Hour.ToString() + " " + dateTimePicker2.Value.Minute.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            sendCommand("Read_ev 0");
        }

        private void saveAllMessages_Click(object sender, EventArgs e)
        {
            //sendCommand("Save_all");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter SW = new StreamWriter(new FileStream(Directory.GetCurrentDirectory() + "/Archive " +
                    DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + " " +
                    DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                    ".txt", FileMode.Append, FileAccess.Write));
                SW.WriteLine(dataFromPort4.Text);
                SW.Close();
            }
            catch
            { }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.Enabled = false;
            Form2 F2 = new Form2(this);
            F2.Show();
            F2.BringToFront();
        }

        private void readAllMessages_Click(object sender, EventArgs e)
        {
            sendCommand("Read_ev 2");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                sendCommand("Wr_button " + comboBox1.Text.ToString());
            }
            else
            {
                MessageBox.Show("Выберите ячейку памяти");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            sendCommand("SaveDatabase");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            sendCommand("ReadDatabase");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверенны, что хотите очистить?", "Очистка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                sendCommand("ClrDatabase");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sendCommand("SaveSerial " + textBox1.Text + " " + textBox2.Text);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            sendCommand("Read_button");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            sendCommand("ReadNoBos");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            sendCommand("Date " + DateTime.Now.Day.ToString() + " " + DateTime.Now.Month.ToString() + " " + DateTime.Now.Year.ToString().Remove(0, 2));
            sendCommand("Time " + DateTime.Now.Hour.ToString() + " " + DateTime.Now.Minute.ToString() + " " + DateTime.Now.Second.ToString());
        }

        private void newPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void руководствоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string fp = "manual.pdf";
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = fp;
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }
            catch
            {
                MessageBox.Show("Отсутствует файл справки");
            }

        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                sendCommand("Clear_button " + comboBox1.Text);
            }
            else
            {
                MessageBox.Show("Выберите ячейку памяти");
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                sendCommand("setBurConnection " + textBox3.Text);
            }
            else
            {
                MessageBox.Show("Введите серийный номер БУР исп. А");
            }
        }

        private void setDetectionAlgorithmX_Click(object sender, EventArgs e)
        {
            if (cbRegimSrabotka.Text != "")
            {
                switch (cbRegimSrabotka.Text)
                {
                    case "A":
                        sendCommand("setDetectionAlgorithm 0");
                        break;
                    case "B":
                        sendCommand("setDetectionAlgorithm 1");
                        break;
                    case "C":
                        sendCommand("setDetectionAlgorithm 2");
                        break;
                    default:
                        MessageBox.Show("Нет такого алгоритма, выберите существующий");
                        break;
                }
            }
            else
                MessageBox.Show("Выберите режим сработки");
        }

        private void setStartAlgorithmXY_Click(object sender, EventArgs e)
        {
            if (rbTemp.Checked == true && rbDelay.Checked == true)
            {
                sendCommand("setStartAlgorithm 0 0");
            }
            else if (rbTemp.Checked == true && rbLikeAuto.Checked == true)
            {
                sendCommand("setStartAlgorithm 0 1");
            }
            else if (rbBos.Checked == true && rbDelay.Checked == true)
            {
                sendCommand("setStartAlgorithm 1 0");
            }
            else if (rbBos.Checked == true && rbLikeAuto.Checked == true)
            {
                sendCommand("setStartAlgorithm 1 1");
            }
            else
                MessageBox.Show("Выберите Режим ручного пуска и Выбор задержки");
        }

        private void setAutoAlgorithmX_Click(object sender, EventArgs e)
        {
            if (rbBlockAuto.Checked == true)
                sendCommand("setAutoAlgorithm 0");
            else
                sendCommand("setAutoAlgorithm 1");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            sendCommand("Date");
            sendCommand("Time");
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            sendCommand("Read_all");
        }

        private void parcerArchive(string data)
        {
            if (data.Contains("Ev"))
            {
                data = data.Remove(0, 3);
                DateTime localTime = DateTime.Now;
                dataFromPort4.Text += "[" + localTime.ToString() + "]: ";
                dataFromPort4.Text += data.Trim() + "\r\n";
            }
            else
            {
                parcer(data);
            }
            if (data.Contains("Передача архива завершена"))
            {
                archiveReading = false;
            }
        }

        //public string DataForArchive(string data)
        //{
        //    string textToArhcive = "";
        //    if (data.Contains("checkedPass true"))
        //    {
        //        BufferClass1.PasswordChecked = "checkedPass true";
        //        if (data.Contains("Начало передачи архива"))
        //        {
        //            archiveReading = true;
        //            textToArhcive = data.Trim();
        //            if (data.Contains("Ev"))
        //            {
        //                data = data.Remove(0, 3);
        //                DateTime localTime = DateTime.Now;
        //                textToArhcive += "[" + localTime.ToString() + "]: ";
        //                textToArhcive += data.Trim() + "\r\n";
        //            }
        //            else if (data.Contains("Передача архива завершена"))
        //            {
        //                archiveReading = false;
        //            }
        //        }
        //    }
        //    return textToArhcive;
        //}

        private void parcer(string data_fromPort)
        {
            DateTime localTime = DateTime.Now;

            if (!data_fromPort.Contains("Ev") && !data_fromPort.Contains("Текущее состояние направления")
                && !data_fromPort.Contains("sound") && !data_fromPort.Contains("checkedPass")
                && !data_fromPort.Contains("Ошибка записи ключа"))
            {
                dataFromPort.Text += "[" + localTime.ToString() + "]: ";
                dataFromPort.Text += data_fromPort + "\r\n";

            }

            if (!data_fromPort.Contains("Ev") && !data_fromPort.Contains("checkedPass"))
                try
                {
                    //Запись полученного сообщения в файл
                    StreamWriter SW2 = new StreamWriter(new FileStream(Directory.GetCurrentDirectory() + "/Archive/Archive (" + localTime.ToString("ddMMyy") + ").txt", FileMode.Append));
                    SW2.WriteLine("[" + localTime.ToString() + "]: ");
                    SW2.WriteLine(data_fromPort + "\r\n");
                    SW2.Close();
                    SW2.Dispose();
                }
                catch
                { }

            string textToData2 = "";
            string textToData3 = "";
            string textToData4 = "";

            if (data_fromPort.Contains("checkedPass true"))
            {
                BufferClass1.PasswordChecked = "checkedPass true";
                this.Enabled = true;
                this.Show();
                this.WindowState = FormWindowState.Normal;
                btIndicator_new.Text = "СОЕДИНЕНИЕ В НОРМЕ";
                BB_green(btIndicator_new);

            }
            else if (data_fromPort.Contains("checkedPass false"))
            {
                BufferClass1.PasswordChecked = "checkedPass false";
                this.Enabled = false;
            }
            // Сообщения от БУР - НАСТРОЙКА СИСТЕМЫ
            else if (data_fromPort.Contains("Русский установлен"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Серийный номер не задан, ожидание серийного номера"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Включен БУ С.Н. "))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Дежурный режим, изменение сетевых параметров невозможно"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("ID сети - "))
            {
                textToData2 = data_fromPort.Trim();
                setID.Text = data_fromPort.Remove(data_fromPort.IndexOf("ID сети - "), 10);
            }
            else if (data_fromPort.Contains("Номер направления - "))
            {
                textToData2 = data_fromPort.Trim();
                setRoomNumber.Text = data_fromPort.Remove(data_fromPort.IndexOf("Номер направления - "), 20);
            }
            else if (data_fromPort.Contains("Рабочий канал - "))
            {
                textToData2 = data_fromPort.Trim();
                setChannelNumber.Text = data_fromPort.Remove(data_fromPort.IndexOf("Рабочий канал - "), 16);
            }
            else if (data_fromPort.Contains("Ожидание изменения сетевых параметров"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Установлено время - "))
            {
                textToData2 = data_fromPort.Trim();
                timeParcer(data_fromPort);
                timer1.Stop();
                timer1.Interval = 1000;
                timer1.Start();

            }
            else if (data_fromPort.Contains("Установлена дата - "))
            {
                textToData2 = data_fromPort.Trim();
                //Распарсить дату в панель состояний
            }
            else if (data_fromPort.Contains("Параметры имеют недопустимые значения"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Присвоены:"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Параметры уже заданы"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Зарегистрирован БОС С.Н. "))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Нет подтверждения регистрации от БОС С.Н. "))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Класс пожарных извещателей установлен - "))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Время задержки пуска установлено - "))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Пароль изменен"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Зарегистрирован новый ключ"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Ключ номер"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Запись базы данных БОС"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Чтение базы данных БОС"))
            {
                textToData2 = data_fromPort.Trim();
                bosReading = true;
            }
            else if (data_fromPort.Contains("БОС") && bosReading)
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Завершено"))
            {
                textToData2 = data_fromPort.Trim();
                bosReading = false;
            }
            else if (data_fromPort.Contains("Очистка базы данных БОС"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Установлена связь с БУР"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Ошибка записи ключа"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Дата"))
            {
                textToData2 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Время"))
            {
                textToData2 = data_fromPort.Trim();
            }
            // Конец сообщений от БУР - НАСТРОЙКА СИСТЕМЫ
            // Сообщения от БУР - УПРАВЛЕНИЕ СИСТЕМОЙ
            else if (data_fromPort.Contains("Внимание от пожарных извещателей"))
            {
                attention = true;
                attentionIndicator_new.Text = "ВНИМАНИЕ";
                BB_red(attentionIndicator_new);

                fire = false;
                fireIndicator_new.Text = "";
                BB_gray(fireIndicator_new);

                start = false;
                startIndicator_new.Text = "";
                BB_gray(startIndicator_new);

                textToData3 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Конец внимания от пожарных извещателей"))
            {
                textToData3 = data_fromPort.Trim();

                if (!attention && !fire && !start)
                {
                    attention = false;
                    attentionIndicator_new.Text = "ОБСТАНОВКА В НОРМЕ";
                    BB_green(attentionIndicator_new);

                    fire = false;
                    fireIndicator_new.Text = "";
                    BB_gray(fireIndicator_new);

                    start = false;
                    startIndicator_new.Text = "";
                    BB_gray(startIndicator_new);
                }
            }
            else if (data_fromPort.Contains("Изменилась обстановка БОС С.Н. "))
            {
                textToData3 = data_fromPort.Trim();
            }
            else if (data_fromPort.Contains("Автоматика заблокирована"))
            {
                textToData3 = data_fromPort.Trim();
                automationBlocked = true;
                blockAuto.Text = "Разблокировать автоматику";
                autoIndicator_new.Text = "АВТОМАТИКА ЗАБЛОКИРОВАНА";
                BB_yellow(autoIndicator_new);
                blockAutoIndicator_new.Text = "АВТОМАТИКА ЗАБЛОКИРОВАНА";
                BB_yellow(blockAutoIndicator_new);
            }
            else if (data_fromPort.Contains("Автоматика разблокирована"))
            {
                textToData3 = data_fromPort.Trim();
                automationBlocked = false;
                blockAuto.Text = "Заблокировать автоматику";
                if (automationProlonged)
                {
                    autoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ (+120 СЕК)";
                    BB_yellow(autoIndicator_new);
                    blockAutoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ (+120 СЕК)";
                    BB_yellow(blockAutoIndicator_new);
                }
                else
                {
                    autoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ";
                    BB_green(autoIndicator_new);
                    blockAutoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ";
                    BB_green(blockAutoIndicator_new);
                }
            }
            // Конец сообщений от БУР - УПРАВЛЕНИЕ СИСТЕМОЙ
            // Сообщения от БУР - ДИАГНОСТИКА СИСТЕМЫ
            else if (data_fromPort.Contains("Текущее состояние направления"))
            {
                string strDirStatus = data_fromPort.Remove(0, 30);
                //ОБСТАНОВКА
                string dirSituation = strDirStatus.Remove(strDirStatus.IndexOf(" "));
                strDirStatus = strDirStatus.Remove(0, strDirStatus.IndexOf(" ") + 1);
                int i = Convert.ToInt32(dirSituation);


                if ((i & 0x03) == 0)
                {
                    //MessageBox.Show("Введите серийный номер БУР исп. А");
                }


                if (dirSituation == "0") //Норма?
                {
                    attention = false;
                    attentionIndicator_new.Text = "ОБСТАНОВКА В НОРМЕ";
                    BB_green(attentionIndicator_new);

                    fire = false;
                    fireIndicator_new.Text = "";
                    BB_gray(fireIndicator_new);

                    start = false;
                    startIndicator_new.Text = "";
                    BB_gray(startIndicator_new);
                }
                else if (dirSituation == "1") //Внимание?
                {
                    attention = true;
                    attentionIndicator_new.Text = "ВНИМАНИЕ";
                    BB_red(attentionIndicator_new);

                    fire = false;
                    fireIndicator_new.Text = "";
                    BB_gray(fireIndicator_new);

                    start = false;
                    startIndicator_new.Text = "";
                    BB_gray(startIndicator_new);
                }
                else if (dirSituation == "2") //Пожар?
                {
                    attention = true;
                    attentionIndicator_new.Text = "";
                    BB_gray(attentionIndicator_new);

                    fire = true;
                    fireIndicator_new.Text = "ПОЖАР";
                    BB_red(fireIndicator_new);

                    start = false;
                    startIndicator_new.Text = "";
                    BB_gray(startIndicator_new);
                }
                else if (dirSituation == "3") //Пуск?
                {
                    attention = true;
                    attentionIndicator_new.Text = "";
                    BB_gray(attentionIndicator_new);

                    fire = true;
                    fireIndicator_new.Text = "";
                    BB_gray(fireIndicator_new);

                    start = true;
                    startIndicator_new.Text = "СТАРТ";
                    BB_red(startIndicator_new);
                }
                //АВТОМАТИКА
                string dirStatus = strDirStatus.Remove(strDirStatus.IndexOf(" "));
                strDirStatus = strDirStatus.Remove(0, strDirStatus.IndexOf(" ") + 1);
                i = Convert.ToInt32(dirStatus);


                if ((i & 32) > 0)
                {
                    automationBlocked = true;
                    autoIndicator_new.Text = "АВТОМАТИКА ЗАБЛОКИРОВАНА";
                    BB_yellow(autoIndicator_new);
                    blockAutoIndicator_new.Text = "АВТОМАТИКА ЗАБЛОКИРОВАНА";
                    BB_yellow(blockAutoIndicator_new);
                    blockAuto.Text = "Разблокировать автоматику";
                }
                else if ((i & 2) > 0)
                {
                    automationProlonged = true;
                    autoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ (+120 СЕК)";
                    BB_yellow(autoIndicator_new);
                    blockAutoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ (+120 СЕК)";
                    BB_yellow(blockAutoIndicator_new);
                }
                else
                {
                    autoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ";
                    BB_green(autoIndicator_new);
                    blockAutoIndicator_new.Text = "АВТОМАТИКА В НОРМЕ";
                    BB_green(blockAutoIndicator_new);
                    automationProlonged = false;
                    automationBlocked = false;
                }
                //ЗВУК
                if ((i & 1) > 0)
                {
                    soundIndicator_new.Text = "ЗВУК ОТКЛЮЧЕН";
                    soundOn = false;
                    BB_yellow(soundIndicator_new);
                }
                else
                {
                    soundIndicator_new.Text = "ЗВУК ВКЛЮЧЕН";
                    soundOn = true;
                    BB_green(soundIndicator_new);
                }
                //ОШИБКИ
                string dirErrors = strDirStatus;
                strDirStatus = "";
                i = Convert.ToInt32(dirErrors);

                if (((i & 1) > 0) || ((i & 2) > 0))
                {
                    bosBroken = true;
                    BB_yellow(bosFaultIndicator_new);
                    bosFaultIndicator_new.Text = "НЕИСПРАВНОСТЬ БОС";
                }
                else
                {
                    bosBroken = false;
                    BB_green(bosFaultIndicator_new);
                    bosFaultIndicator_new.Text = "БОС ИСПРАВНЫ";
                }
                if (((i & 4) > 0) || ((i & 8) > 0) || ((i & 16) > 0) || ((i & 32) > 0) || ((i & 64) > 0) || ((i & 128) > 0))
                {
                    buBroken = true;
                    BB_yellow(buFaultIndicator_new);
                    buFaultIndicator_new.Text = "НЕИСПРАВНОСТЬ БУР";
                }
                else
                {
                    buBroken = false;
                    BB_green(buFaultIndicator_new);
                    buFaultIndicator_new.Text = "БУР ИСПРАВЕН";
                }
                if ((i & 64) > 0)
                {
                    supply_1_High = true;
                    supplyIndicator_1_new.Text = "ПИТАНИЕ 1 НЕИСПРАВНО";
                    BB_yellow(supplyIndicator_1_new);
                }
                else
                {
                    supply_1_High = false;
                    supply_1_Low = false;
                    supplyIndicator_1_new.Text = "ПИТАНИЕ 1 В НОРМЕ";
                    BB_green(supplyIndicator_1_new);
                }
                if ((i & 128) > 0)
                {
                    supply_2_High = true;
                    supplyIndicator_2_new.Text = "ПИТАНИЕ 2 НЕИСПРАВНО";
                    BB_yellow(supplyIndicator_2_new);

                    //supply_1_Low = true;
                    //supplyIndicator_1_new.Text = "Напряжение понижено";
                    //BB_yellow(supplyIndicator_1_new);

                    //supply_2_Low = true;
                    //supplyIndicator_2_new.Text = "Напряжение понижено";
                    //BB_yellow(supplyIndicator_2_new);
                }
                else
                {
                    supply_2_High = false;
                    supply_2_Low = false;
                    supplyIndicator_2_new.Text = "ПИТАНИЕ 2 В НОРМЕ";
                    BB_green(supplyIndicator_2_new);
                }
            }
            else if (data_fromPort.Contains("Трансивер неисправен"))
            {
                transceiverBroken = true;
                transceiverIndicator_new.Text = "ТРАНСИВЕР НЕИСПРАВЕН";
                BB_yellow(transceiverIndicator_new);
            }
            else if (data_fromPort.Contains("Трансивер в норме"))
            {
                transceiverBroken = false;
                transceiverIndicator_new.Text = "ТРАНСИВЕР В НОРМЕ";
                BB_green(transceiverIndicator_new);
            }
            else if (data_fromPort.Contains("Микросхема памяти не готова к работе"))
            {
                memoryBroken = true;
                memoryIndicator_new.Text = "ПАМЯТЬ НЕИСПРАВНА";
                BB_yellow(memoryIndicator_new);
            }
            else if (data_fromPort.Contains("Микросхема памяти в норме"))
            {
                memoryBroken = false;
                memoryIndicator_new.Text = "ПАМЯТЬ В НОРМЕ";
                BB_green(memoryIndicator_new);
            }
            else if (data_fromPort.Contains("Время - "))
            {
                timeParcer(data_fromPort);
                timer1.Stop();
                timer1.Interval = 1000;
                timer1.Start();
            }
            else if (data_fromPort.Contains("BlueTooth на борту"))
            {
                btBroken = false;
                btConnected = false;
                btIndicator_new.Text = "СОЕДИНЕНИЕ НЕ УСТАНОВЛЕНО";
                BB_green(btIndicator_new);
            }
            else if (data_fromPort.Contains("BlueTooth соединение установлено"))
            {
                btBroken = false;
                btConnected = true;
                btIndicator_new.Text = "BLUETOOTH В НОРМЕ";
                BB_green(btIndicator_new);
            }
            else if (data_fromPort.Contains("sound on"))
            {

                soundIndicator_new.Text = "ЗВУК ВКЛЮЧЕН";
                soundOn = true;
                BB_green(soundIndicator_new);
            }
            else if (data_fromPort.Contains("sound off"))
            {
                soundIndicator_new.Text = "ЗВУК ОТКЛЮЧЕН";
                soundOn = false;
                BB_yellow(soundIndicator_new);
            }
            else if (data_fromPort.Contains("Начат тест"))
            {

                testIndicator_new.Text = "ТЕСТИРОВАНИЕ";
                testOn = true;
                BB_yellow(testIndicator_new);
            }
            else if (data_fromPort.Contains("Начало передачи архива"))
            {
                textToData4 = data_fromPort.Trim();
                archiveReading = true;
            }
            else if (data_fromPort.Contains("Тест завершен"))
            {
                testIndicator_new.Text = "";
                testOn = false;
                BB_gray(testIndicator_new);
            }
            // Конец сообщений от БУР - ДИАГНОСТИКА СИСТЕМЫ
            // Cообщения от БУР - АРХИВ
            else if (data_fromPort.Contains("Ev"))
            {
                data_fromPort = data_fromPort.Remove(0, 3);
                textToData4 = data_fromPort.Trim();
            }
            // Конец сообщений от БУР - АРХИВ
            if (textToData2 != "")
            {
                dataFromPort2.Text += "[" + localTime.ToString() + "]: ";
                dataFromPort2.Text += textToData2 + "\r\n";
            }

            if (textToData3 != "")
            {
                dataFromPort3.Text += "[" + localTime.ToString() + "]: ";
                dataFromPort3.Text += textToData3 + "\r\n";
            }

            if (textToData4 != "")
            {
                dataFromPort4.Text += "[" + localTime.ToString() + "]: ";
                dataFromPort4.Text += textToData4 + "\r\n";
            }
        }

        void BB_green(Beauty_Button_1 BB)
        {
            BB.ForeColor = Color.Black;
            BB.leftColor = Color.Lime;
            BB.rightColor = Color.PaleGreen;
            BB.Invalidate();
        }

        void BB_yellow(Beauty_Button_1 BB)
        {
            BB.ForeColor = Color.Black;
            BB.leftColor = Color.Gold;
            BB.rightColor = Color.Yellow;
            BB.Invalidate();
        }

        void BB_red(Beauty_Button_1 BB)
        {
            BB.ForeColor = Color.White;
            BB.leftColor = Color.FromArgb(221, 03, 37);
            BB.rightColor = Color.FromArgb(245, 74, 48);
            BB.Invalidate();
        }

        void BB_gray(Beauty_Button_1 BB)
        {
            BB.ForeColor = Color.Black;
            BB.leftColor = Color.LightGray;
            BB.rightColor = Color.Gainsboro;
            BB.Invalidate();
        }
    }
}