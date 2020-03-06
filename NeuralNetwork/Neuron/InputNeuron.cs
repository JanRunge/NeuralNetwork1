using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class InputNeuron : Neuron
    {
        //Inputneurons dont use weight or mutliple inputs
        //they just return their input as output.
        public double input;

        public InputNeuron(Layer l) :base(l)
        {
        }

        public override void fire()
        {
            output = input;
        }
        public override void calculateError(double desired_result)
        {
            throw new Exception("Inputneurons cannot have an Error!");
        }
    }
}
