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
        public override double output
        {
            //the input is a blank number
            get {  return input; }
            set { input = value; }
        }

    }
}
