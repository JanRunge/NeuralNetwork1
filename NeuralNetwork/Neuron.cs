using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Neuron
    {
        public double[] inputs = new double[2];
        public double[] weights = new double[2];
        public double error;
        private double biasWeight;
        private Random r = new Random();


        public double output
        {
            get { return Sigmoid.output(weights[0] * inputs[0] + weights[1] * inputs[1] + biasWeight); }
        }

        public void randomizeWeights()
        {
            weights[0] = r.NextDouble();
            weights[1] = r.NextDouble();
            biasWeight = r.NextDouble();
        }

        public void adjustWeights()
        {
            weights[0] += error * inputs[0];
            weights[1] += error * inputs[1];
            biasWeight += error;
        }
    }
}
