using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class OutputNeuron : Neuron
    {
        public OutputNeuron(Random r) : base(r)//call the base constructor
        {

        }
        public override void calculateError(double desired_result)
        {
            this.error = this.output * (1 - this.output) * (this.output - desired_result);//backpropagated error
            
        }
    }
}
