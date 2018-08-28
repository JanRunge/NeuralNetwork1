using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class InputNeuron : Neuron
    {
       public double input;
        public override double output
        {
            get {  return input; }
            set { input = value; }
        }

    }
}
