using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class HiddenNeuron :Neuron
    {
        public HiddenNeuron(Random r) : base(r)
        {

        }
        //@TODO if there is more than one output, we need to calc differently
        public override void calculateError(double desired_result)
        {
            double sum = 0;
            foreach (Axon a in this.outputs)
            {
                sum += a.output.error * (a.weight);
            }
            this.error = sum * (this.output * (1 - this.output));
        }
    }
}
