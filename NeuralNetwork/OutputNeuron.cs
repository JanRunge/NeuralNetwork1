using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class OutputNeuron : Neuron
    {
        public OutputNeuron() : base()
        {

        }
        public override void calculateError(double desired_result)
        {
            this.error = Sigmoid.derivative(this.output) * (desired_result - this.output);
        }
    }
}
