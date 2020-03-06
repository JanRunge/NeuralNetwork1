using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class OutputNeuron : Neuron
    {
        public OutputNeuron(Layer l) : base(l)
        {
        }

        public override void calculateError(double desired_result)
        {
            this.error = (this.output - desired_result) * myLayer.ActivationFunction.derivative(this.output);
        }
    }
}
