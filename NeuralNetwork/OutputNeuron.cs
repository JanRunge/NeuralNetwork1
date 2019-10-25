using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class OutputNeuron : Neuron
    {

        public double error;
        public OutputNeuron(Random r) : base(r)//call the base constructor
        {

        }
        public override void calculateError(double desired_result)
        {
            this.error = this.output - desired_result;
            this.gamma = this.error * TanHDer(this.output);
            foreach(Axon a in this.inputs)
            {
                a.weightdelta = this.gamma * a.input.output;
            }
            this.biasdelta = this.gamma * biasweight;
            
        }
    }
}
