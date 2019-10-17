using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class HiddenNeuron :Neuron
    {
        public HiddenNeuron() : base()
        {

        }
        //@TODO if there is more than one output, we need to calc differently
        public override void calculateError(double desired_result)
        {
            error= Sigmoid.derivative(this.output) * this.outputs[0].output.error * this.outputs[0].weight;
        }
    }
}
