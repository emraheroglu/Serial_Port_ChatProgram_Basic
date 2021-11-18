using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPortChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var seriPort in SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(seriPort);
            }
            comboBoxPorts.SelectedIndex = 0;
            buttonDisconnect.Enabled = false;
            buttonSend.Enabled = false;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBoxPorts.Text;
            serialPort1.BaudRate = 9600; //rastgele baudrate değeri girilmez, belirli değerleri mevcut.
            serialPort1.Parity = Parity.Even;
            serialPort1.StopBits = StopBits.One;
            serialPort1.DataBits = 8;

            try
            {
                serialPort1.Open();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Seri Port Bağlantısı Yapılamadı..\n Hata: {exception.Message}",
                    "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
                buttonSend.Enabled = true;
            }

        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Enabled = true;
                buttonDisconnect.Enabled = false;
                buttonSend.Enabled = false;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(textBoxSend.Text);
                textBoxSend.Clear();
            }
        }

        public delegate void veriGoster(String str);

        public void textBoxYaz(String str)
        {
            textBoxReceived.Text += str;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String gelenVeri = serialPort1.ReadExisting();
            textBoxReceived.Invoke(new veriGoster(textBoxYaz),gelenVeri);
        }
    }
}
