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
            xornet = new Network();
            xornet.MakeXOR();
        }

        private void ButtonXOR_Click(object sender, EventArgs e)
        {
            int var1;
            int var2;
            Int32.TryParse(textBox1.Text, out var1);
            Int32.TryParse(textBox2.Text, out var2);
            xornet.InputNeurons[0].output = var1;
            xornet.InputNeurons[1].output = var2;



             textBox3.Text = Math.Round(xornet.outputNeurons[0].output) + " ("+ xornet.outputNeurons[0].output + ")";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            xornet.trainXOR();
        }
    }
}
