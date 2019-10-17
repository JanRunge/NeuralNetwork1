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
            //the output always equals the input
            get {  return input; }
            set { input = value; }
        }
        public InputNeuron(Random r): base(r)
        {

        }
        
        public override void calculateError(double desired_result)
        {
            throw new Exception("Inputneurons cannot have an Error!");
        }
    }
}
