using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Garant_R_NEW
{
    public partial class Form3 : Form
    {
        Form1 F1;
        public Form3(Form1 _F1)
        {
            F1 = _F1;
            InitializeComponent();
        }

        

        private bool settingsSaved { get; set; } = false;
        private bool fireFlag { get; set; } = false;
        private bool startFlag { get; set; } = false;

        private void button2_Click(object sender, EventArgs e)
        {
            string stringRelay = "";
            bool triggerFlag = false;
            string paramEv = "";
            string paramNum = "";
            string paramIsh = "";
            string paramTn = "";
                       

            if (comboBox2.Text == "Пожар")
                if (!fireFlag)
                {
                    stringRelay += comboBox2.Text + " ";
                    triggerFlag = true;
                    fireFlag = true;
                }
                else
                {
                    MessageBox.Show("Алгоритм \"Пожар\" уже задан");
                }
            else if (comboBox2.Text == "Пуск")
                if (!startFlag)
                {
                    stringRelay += comboBox2.Text + " ";
                    triggerFlag = true;
                    startFlag = true;
                }
                else
                {
                    MessageBox.Show("Алгоритм \"Пуск\" уже задан");
                }
            else
            {
                MessageBox.Show("Не выбран триггер!");
            }
            
            
            if (triggerFlag)
            {
                string s1 = stringRelay.Trim();
                int relayCount = s1.Split(new string[] { " " }, StringSplitOptions.None).Count() - 2;
                stringRelay += relayCount.ToString() + " ";

                if (toggleSwitch1.Checked)
                {
                    stringRelay += "ON ";
                }
                else
                {
                    stringRelay += "OFF ";
                }

                stringRelay += textBox1.Text;

                comboBox1.Items.Add(s1);
                MessageBox.Show("Событие успешно добавлено!");
            }
            settingsSaved = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (F1.port.IsOpen)
            {
                /*
                string fireString = "";
                string startString = "";
                foreach (String item in comboBox1.Items)
                {
                    if (comboBox1.SelectedItem.ToString().Contains("Внимание"))
                    {

                    }
                }
                */
                
                string tempString = comboBox1.Items.Count.ToString();
                F1.sendCommand("actionsCount " + tempString);
                int tempi = 0;
                foreach (String item in comboBox1.Items)
                {
                    comboBox1.SelectedIndex = tempi;
                    F1.sendCommand("Action" + tempi.ToString() + " " + comboBox1.SelectedItem);
                    tempi++;
                }
                settingsSaved = true;
            }
            else
            {
                MessageBox.Show("Порт закрыт. Откройте порт и попробуйте снова.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                comboBox1.Items.Remove(comboBox1.SelectedItem);
                comboBox1.Text = "";
            }
            settingsSaved = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 32)
            {
                e.Handled = true;
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!settingsSaved)
            {
                DialogResult dialogResult = MessageBox.Show("Данные не сохранены! Вы уверены, что хотите закрыть форму настройки реле?", "Подтверждение", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
