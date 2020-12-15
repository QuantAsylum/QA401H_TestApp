using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QA401H_TestApp
{
    public partial class Form1 : Form
    {
        bool Stress = false;

        public Form1()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 2;

            QA401.SetRootUrl("http://localhost:9401");
        }

        private void ApplySettings()
        {
            QA401.SetDefaults();

            //QA401.MaxInput = checkBox3.Checked ? 26 : 6;
            QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
            QA401.WindowType = comboBox1.Text;

            QA401.Gen1Enable = checkBox1.Checked;
            QA401.Gen1Freq = Convert.ToDouble(numericUpDown2.Value);
            QA401.RoundFrequencies = checkBox2.Checked;
            QA401.Gen1Amp = Convert.ToDouble(numericUpDown1.Value);
        }

        // Measure RMS in 20 to 20kHz bw
        private void button1_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                ApplySettings();

                QA401.DoAcquisition();
                QA401.GetRmsDbv(20, 20000, out left, out right, false);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        // Measure RMS 900 - 1100 Hz bw
        private void Button3_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                ApplySettings();

                QA401.DoAcquisition();
                QA401.GetRmsDbv(900, 1100, out left, out right, false);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        // Measure peak amplitude
        private void Button17_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                ApplySettings();

                QA401.DoAcquisition();
                QA401.GetPeakDbv(20, 20000, out left, out right);
                Console.WriteLine("Highest Peak 20-20 kHz  Left: {0:0.00}   Right:{1:0.00}", left, right);
                QA401.GetPeakDbv(980, 1020, out left, out right);
                Console.WriteLine("Highest Peak 980-1020 Hz  Left: {0:0.00}   Right:{1:0.00}", left, right);
                QA401.GetPeakDbv(1980, 2020, out left, out right);
                Console.WriteLine("Highest Peak 1980-2020 Hz  Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        // Measure tone at 90 Hz with AWeighting. This should be -20 dB below generator level
        private void button7_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                ApplySettings();
                // Override default generator settings
                QA401.SetGen1(90, 0, true);

                QA401.DoAcquisition();
                QA401.GetRmsDbv(20, 20000, out left, out right, true);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        // Measure rms noise floor with atten on
        private void button5_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                QA401.SetDefaults();
                QA401.MaxInput = 26;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);

                QA401.DoAcquisition();
                QA401.GetRmsDbv(20, 20000, out left, out right);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        // Measure noise floor with atten off
        private void button2_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                QA401.SetDefaults();

                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.DoAcquisition();
                QA401.GetRmsDbv(20, 20000, out left, out right);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                QA401.SetDefaults();
                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.SetGen1(1000, 0, true);
                QA401.SetGen2(2000, -40, true);

                QA401.DoAcquisition();

                QA401.GetThdDb(1000, 20000, out left, out right);
                Console.WriteLine("Left (dB): {0:0.00}   Right (dB):{1:0.00}", left, right);
                QA401.GetThdPct(1000, 20000, out left, out right);
                Console.WriteLine("Left (%): {0:0.000}   Right (%):{1:0.000}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {

                QA401.SetDefaults();
                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.SetGen1(1000, -10, true);
                QA401.SetGen2(5000, -70, true);

                QA401.DoAcquisition();

                QA401.GetThdnDb(1000, 20, 20000, out left, out right);
                Console.WriteLine("Left (dB): {0:0.00}   Right (dB):{1:0.00}", left, right);
                QA401.GetThdnPct(1000, 20, 20000, out left, out right);
                Console.WriteLine("Left (%): {0:0.000}   Right (%):{1:0.000}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }


        // Get version
        private void button8_Click(object sender, EventArgs e)
        {
            double version;

            try
            {
                version = QA401.Version;
                Console.WriteLine($"Version: {version:0.000}");
            }
            catch
            {

            }

        }

        // Check if connected
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (QA401.IsConnected == true)
                {
                    Console.WriteLine("Connected");
                    return;
                }
            }
            catch
            {

            }

            Console.WriteLine("Not connected");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            QA401.RoundFrequencies = true;
            QA401.RoundFrequencies = false;
            QA401.RoundFrequencies = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                QA401.SetDefaults();
                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.SetGen1(1000, 0, true);

                QA401.DoAcquisition();

                QA401.GetPhaseDegrees(out left, out right);
                Console.WriteLine("Left (deg): {0:0.00}   Right (deg):{1:0.00}", left, right);
                QA401.GetPhaseSeconds(out left, out right);
                Console.WriteLine("Left (usec): {0:0.000}   Right (usec):{1:0.000}", left * 1e6, right * 1e6);
            }
            catch
            {

            }
        }

        // Get freq series
        private void button12_Click(object sender, EventArgs e)
        {
            double[] left, right;

            try
            {
                QA401.SetDefaults();
                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.SetGen1(1050, 0, true);

                QA401.DoAcquisition();

                QA401.GetFrequencyData(out left, out right, out double dx);

                double maxL = left.Max();
                int indexOfMaxL = Array.IndexOf(left, maxL);
                double freqOfMaxL = indexOfMaxL * dx;

                double maxR = right.Max();
                int indexOfMaxR = Array.IndexOf(right, maxR);
                double freqOfMaxR = indexOfMaxR * dx;

                Console.WriteLine("Left Peak: {0:0.00}   Right Peak:{1:0.00}", freqOfMaxL, freqOfMaxR);
            }
            catch
            {

            }

        }

        // Enable stress test. This will repeatedly hammer the QA401 app with a THD 
        // test. Run overnight to check for leaks, ensure server keeps running, etc
        // This button will enable a timer that kicks off the test in a separate task
        private void button13_Click(object sender, EventArgs e)
        {
            Stress = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Stress)
            {
                timer1.Enabled = false;

                var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        double version = QA401.Version;
                        QA401.SetDefaults();
                        QA401.MaxInput = 6;
                        QA401.BufferSize = 32768;
                        QA401.SetGen1(1000, 0, true);
                        QA401.SetGen2(2000, -40, true);

                        QA401.DoAcquisition();

                        QA401.GetThdDb(1000, 20000, out double left, out double right);
                        Console.WriteLine("Left (dB): {0:0.00}   Right (dB):{1:0.00}", left, right);
                        QA401.GetThdPct(1000, 20000, out left, out right);
                        Console.WriteLine("Left (%): {0:0.000}   Right (%):{1:0.000}", left, right);

                        pictureBox1.Invoke((MethodInvoker)delegate
                        {
                            pictureBox1.Image = QA401.GetGraph();
                            if (Stress)
                                timer1.Enabled = true;
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }

        // Stress off
        private void button14_Click(object sender, EventArgs e)
        {
            Stress = false;
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    QA401.SetDefaults();
                    QA401.MaxInput = 6;
                    QA401.BufferSize = (int)Math.Pow(2, 18);
                    QA401.SetGen1(1050, 0, true);

                    QA401.DoAcquisition();
                }
                catch
                {

                }
            });

        }

        private void CheckBox3_Click(object sender, EventArgs e)
        {
            try
            {
                QA401.MaxInput = checkBox3.Checked ? 26 : 6;
            }
            catch
            {

            }
        }

        /// <summary>
        /// Performs an acquisition with user-supplied data. This data will be sent out of the DAC instead of the 
        /// normally generated data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {

            double[] left, right;

            try
            {
                QA401.SetDefaults();
                QA401.MaxInput = checkBox3.Checked ? 26 : 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);

                left = new double[QA401.BufferSize];
                right = new double[QA401.BufferSize];

                // Generate a Math.Sqrt(2) peak signal. This is 0 dBV
                for (int i = 0; i < QA401.BufferSize; i++)
                {
                    left[i] = Math.Sqrt(2) * Math.Sin(2 * Math.PI * 1000 * (double)i / 48000);
                    right[i] = -left[i];
                }

                QA401.DoAcquisition(left, right);

                // Find frequency peak
                QA401.GetPeakDbv(20, 20000, out double ampLeft, out double ampRight);
                Console.WriteLine("FREQ DATA: Left Peak dBv: {0:0.00}   Right Peak dBV:{1:0.00}", ampLeft, ampRight);

                // Pull over time series and look for max. Should be 1.41 volts
                QA401.GetTimeData(out double[] leftTime, out double[] rightTime, out double dx);
                Console.WriteLine("TIME DATA: Left Peak volts: {0:0.00}   Right Peak volts:{1:0.00}", leftTime.Max(), rightTime.Max());
            }
            catch
            {

            }
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            double[] left, right;

            try
            {
                QA401.SetDefaults();
                QA401.SampleRate = 192000;
                QA401.MaxInput = checkBox3.Checked ? 26 : 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);

                left = new double[QA401.BufferSize];
                right = new double[QA401.BufferSize];

                // Generate a Math.Sqrt(2) peak signal. This is 0 dBV
                for (int i = 0; i < QA401.BufferSize; i++)
                {
                    left[i] = Math.Sqrt(2) * Math.Sin(2 * Math.PI * 1000 * (double)i / 48000);
                    right[i] = -left[i];
                }

                QA401.DoAcquisition(left, right);

                // Find frequency peak
                QA401.GetPeakDbv(20, 20000, out double ampLeft, out double ampRight);
                Console.WriteLine("FREQ DATA: Left Peak dBv: {0:0.00}   Right Peak dBV:{1:0.00}", ampLeft, ampRight);

                // Pull over time series and look for max. Should be 1.41 volts
                QA401.GetTimeData(out double[] leftTime, out double[] rightTime, out double dx);
                Console.WriteLine("TIME DATA: Left Peak volts: {0:0.00}   Right Peak volts:{1:0.00}", leftTime.Max(), rightTime.Max());
            }
            catch
            {

            }
        }

        //private void button10_Click_1(object sender, EventArgs e)
        //{
        //    RoundTripBase64();
        //}

        /// <summary>
        /// Unreferenced test code to show how to go from array of doubles to base64 and back.
        /// A single double gives a string 12 bytes long, two doubles gives 24, etc
        /// </summary>
        private void RoundTripBase64()
        {
            const int length = 2;

            // Create an array of doubles
            double[] d = new double[length];
            for (int i = 0; i < length; i++)
                d[i] = i;

            // convert array of doubles to array of bytes
            var byteArray = new byte[d.Length * sizeof(double)];
            Buffer.BlockCopy(d, 0, byteArray, 0, byteArray.Length);

            // Convert byte arrta to base 64
            string s = Convert.ToBase64String(byteArray);
            Debug.WriteLine($"Length: {s.Length}");

            // Done! We went from array of doubles to base64 string. 

            // Now reverse. Convert string to byte array
            byteArray = Convert.FromBase64String(s);

            // Convert byte array to double array
            Buffer.BlockCopy(byteArray, 0, d, 0, byteArray.Length);
        }

        // SNR
        private void button18_Click(object sender, EventArgs e)
        {
            double left, right;

            try
            {
                QA401.SetDefaults();

                QA401.MaxInput = 6;
                QA401.BufferSize = Convert.ToInt32(comboBox2.Text);
                QA401.SetGen1(1050, 0, true);
                QA401.DoAcquisition();
                QA401.GetSnrDb(1050, 100, 20000, out left, out right);
                Console.WriteLine("Left: {0:0.00}   Right:{1:0.00}", left, right);

                pictureBox1.Image = QA401.GetGraph();
            }
            catch
            {

            }
        }
    }
}
