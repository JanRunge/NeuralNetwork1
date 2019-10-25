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
            double outputGammas = 1;
            foreach (Axon a in this.outputs)
            {
                outputGammas *= a.output.gamma;
            }

            gamma = 0;
            foreach (Axon a in this.outputs)
            {
                gamma += a.output.gamma * (a.weight);
            }
            gamma *= TanHDer(this.output);

            foreach (Axon a in this.inputs)
            {
                a.weightdelta = this.gamma * a.input.output;
            }
            this.biasdelta = outputGammas * biasweight;
        }
    }
}
