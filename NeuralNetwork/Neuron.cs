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

        private double pOutput;
        public double gamma;
        private double biasweight;
        
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
                return pOutput;
            }
            set { }
        }
        public abstract void calculateError(double desired_result);
        public void fire()
        {
            double sum = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += (inputs[i].weight * inputs[i].input.output);
            }
            pOutput = Math.Tanh(sum);
        }
        public void addInput(Neuron n)
        {
            new Axon(n, this);

        }
        public void randomizeWeights()
        {
            //@TODO make this dynamic
            for (int i=0; i< inputs.Count; i++)
            {
                inputs[i].weight = r.NextDouble()-0.5;

            }
        }

        public virtual void adjustWeights()
        {
            foreach (Axon a in this.inputs)
            {
                a.weight -= 0.5*(a.weightdelta);
                a.weightdelta = 0;
            }
        }
        //derivate TanH
        public double TanHDer(double value)
        {
            return 1 - (value * value);
        }
    }
}
