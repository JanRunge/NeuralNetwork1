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
        }

        private void ButtonXOR_Click(object sender, EventArgs e)
        {
            int var1;
            int var2;
            Int32.TryParse(textBox1.Text, out var1);
            Int32.TryParse(textBox2.Text, out var2);
            xornet.hiddenNeuron1.inputs = new double[] { var1, var2 };
            xornet.hiddenNeuron2.inputs = new double[] { var1, var2 };

            xornet.outputNeuron.inputs = new double[] { xornet.hiddenNeuron1.output, xornet.hiddenNeuron2.output };
            textBox3.Text = Math.Round(xornet.outputNeuron.output) + " ("+ xornet.outputNeuron.output + ")";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            xornet.train();
        }
    }
}
