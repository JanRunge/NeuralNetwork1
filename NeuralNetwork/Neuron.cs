using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Neuron
    {
        public List<Neuron> inputs = new List<Neuron>();
        public List<double> weights = new List<double>();
        public double error;
        private double biasWeight;
        private Random r = new Random();
        public String name;


        public virtual double output
        {
            //the output is calculated through the inputs
            
            get {
                double sum = 0;
                for (int i = 0; i < inputs.Count; i++)
                {
                    sum+= (weights[i] * inputs[i].output);
                }
                return Sigmoid.output(sum + biasWeight);
            }
            set { }
        }
        public void addInput(Neuron n)
        {
            inputs.Add(n);
            weights.Add(-1);
        }
        public void randomizeWeights()
        {
            //@TODO make this dynamic
            for (int i=0; i< inputs.Count; i++)
            {
                weights[i] = r.NextDouble();

            }
            biasWeight = r.NextDouble();
            
        }

        public void adjustWeights()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                weights[i] += error * inputs[i].output;

            }
            biasWeight += error;
        }
    }
}
