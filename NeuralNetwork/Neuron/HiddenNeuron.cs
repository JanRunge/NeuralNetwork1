using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class HiddenNeuron :Neuron
    {
        public HiddenNeuron(Layer l) : base(l)
        {
        }

        //@TODO if there is more than one output, we need to calc differently
        public override void calculateError(double desired_result)
        {
            double Gammasum = 0;
            foreach (Axon a in this.outputs)
            {
                Gammasum += a.output.error*a.weight;
            }
            this.error = Gammasum * myLayer.ActivationFunction.derivative(this.output);
        }

    }
}
