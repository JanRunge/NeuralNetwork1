using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        Network xornet;
        public Form1()
        {
            InitializeComponent();
            xornet = new XoRNetwork() ;
        }

        private void ButtonXOR_Click(object sender, EventArgs e)
        {
            int var1;
            int var2;
            Int32.TryParse(textBox1.Text, out var1);
            Int32.TryParse(textBox2.Text, out var2);
            double[] erg = xornet.calculateForInput(new double[2] { var1, var2 });


            textBox3.Text = Math.Round(erg[0]) + " (" + erg[0].ToString() + ")";

        }

        private void button1_Click(object sender, EventArgs e)//train
        {
            xornet.trainWithLogging(20000, 0.01);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            xornet.RandomizeAllWeights();
        }

        private void button3_Click(object sender, EventArgs e)//save
        {
            //xornet.Save("");
        }
    }
}
