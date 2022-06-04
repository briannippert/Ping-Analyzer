using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    public partial class Form1 : Form
    {
        private string text;
        private List<double> time = new List<double>();
        private double count;
        private int total = 1000;
        private double min;
        private double max;
        private double average;
        private double standardDev;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

                count = 0;
                min = 0;
                max = 0;
                average = 0;
                standardDev = 0;
                time.Clear();
                text = "";
                chart1.Series["Ping"].Points.Clear();
            try
            {
                string path;
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == DialogResult.OK)
                {
                    lblFile.Text = file.FileName;
                    path = file.FileName;
                    text = File.ReadAllText(path);
                    extractData(text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR!");
            }
        }

        private void extractData(string input)
        {
            Regex r1 = new Regex(@"time=(.*)ms");
            r1.Matches(input);
            foreach (Match m in r1.Matches(input))
            {
                time.Add(Convert.ToDouble(m.Groups[1].ToString()));
            }
            count = time.Count;
            makeGraph();
            generateStatistics();
        }

        private void makeGraph()
        {
            int count = 1;
            double sum = 0;
            foreach (double num in time)
            {
                sum += num;
                chart1.Series["Ping"].Points.AddXY(count,num);
                count++;
            }
            average = sum / count;
            average = Math.Round(average,2);
        }
        private void generateStatistics()
        {
            time.Sort();
            min = time[0];
            time.Reverse();
            max = time[0];
            double sumOfSquaresOfDifferences = time.Select(val => (val - average) * (val - average)).Sum();
            standardDev = Math.Round(Math.Sqrt(sumOfSquaresOfDifferences / time.Count),2);
            txtMax.Text = max.ToString() + " ms";
            txtMin.Text = min.ToString() + " ms";
            txtMean.Text = average.ToString()+ " ms";
            txtStdDev.Text = standardDev.ToString() + " ms";
            txtPacketLoss.Text = ((1 - (count / 1000)) * 100).ToString() + "%";


        }
    }
}
