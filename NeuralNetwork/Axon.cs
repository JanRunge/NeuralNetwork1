using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Axon
    {
        public Neuron input;
        public double weight;
        public Neuron output;
        public double weightdelta;

        public Axon(Neuron input, Neuron output)
        {
            this.input = input;
            this.output = output;
            input.outputs.Add(this);
            output.inputs.Add(this);
        }
    }
}
