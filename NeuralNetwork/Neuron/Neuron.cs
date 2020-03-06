using Newtonsoft.Json;
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
        public double bias;


        public Layer myLayer;
        public String name;
        public double output;

        public abstract void calculateError(double desired_result);

        public virtual void fire()
        {
            double sum = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += (inputs[i].weight * inputs[i].input.output);
            }
            output = myLayer.ActivationFunction.calculate(sum + bias);
        }

        public void addInput(Neuron n)
        {
            new Axon(n, this);
        }

        public Neuron(Layer l)
        {
            myLayer = l;
        }

        public void randomizeWeights()
        {
            for (int i=0; i< inputs.Count; i++)
            {
                inputs[i].weight = Global.random.NextDouble()-0.5f;

            }
            bias = Global.random.NextDouble() - 0.5f;
        }

        public virtual void adjustWeights()
        {
            foreach (Axon a in this.inputs)
            {
                a.weight -= myLayer.getLearningRate() * error*a.input.output;
            }
            bias -= myLayer.getLearningRate() * error;
        }
    }
}
