using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    abstract class Neuron
    {
        public List<Axon> inputs = new List<Axon>();
        public List<Axon> outputs = new List<Axon>();
        public double error;
        private double biasWeight;
        private Random r;
        public String name;

        public Neuron(Random r)
        {
            this.r = r;
        }
        public virtual double output
        {
            //the output is calculated through the inputs
            
            get {
                double sum = 0;
                for (int i = 0; i < inputs.Count; i++)
                {
                    sum+= (inputs[i].weight * inputs[i].input.output);
                }
                return Sigmoid.output(sum + biasWeight);
            }
            set { }
        }
        public abstract void calculateError(double desired_result);
        public void addInput(Neuron n)
        {
            new Axon(n, this);

        }
        public void randomizeWeights()
        {
            //@TODO make this dynamic
            for (int i=0; i< inputs.Count; i++)
            {
                inputs[i].weight = r.NextDouble();

            }
            biasWeight = r.NextDouble();
            
        }

        public void adjustWeights()
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                inputs[i].weight += error * inputs[i].input.output;

            }
            biasWeight += error;
        }
    }
}
