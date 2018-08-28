using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Neuron
    {
        public Neuron[] inputs = new Neuron[2];
        public double[] weights = new double[2];
        public double error;
        private double biasWeight;
        private Random r = new Random();
        public String name;


        public virtual double output
        {
            //the output is calculated through the inputs
            get { return Sigmoid.output(weights[0] * inputs[0].output + weights[1] * inputs[1].output + biasWeight); }
            set { }
        }

        public void randomizeWeights()
        {
            weights[0] = r.NextDouble();
            weights[1] = r.NextDouble();
            biasWeight = r.NextDouble();
        }

        public void adjustWeights()
        {
            weights[0] += error * inputs[0].output;
            weights[1] += error * inputs[1].output;
            biasWeight += error;
        }
    }
}
