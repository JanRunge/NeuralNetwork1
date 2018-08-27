using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Network
    {
        public Neuron hiddenNeuron1;
        public Neuron hiddenNeuron2;
        public Neuron outputNeuron;
        public Network()
        {
            hiddenNeuron1 = new Neuron();
            hiddenNeuron2 = new Neuron();
            outputNeuron = new Neuron();
            hiddenNeuron1.randomizeWeights();
            hiddenNeuron2.randomizeWeights();
            outputNeuron.randomizeWeights();
        }
        public  void train()
        {
            // the input values
            double[,] inputs =
             {
                 { 0, 0},
                 { 0, 1},
                 { 1, 0},
                 { 1, 1}
             };

             // desired results
             double[] results = { 0, 1, 1, 0 };

            int epoch = 0;

            while (epoch < 2000)
            {
                epoch++;
                for (int i = 0; i < 4; i++)  // very important, do NOT train for only one example
                {
                    // 1) forward propagation (calculates output)
                    hiddenNeuron1.inputs = new double[] { inputs[i, 0], inputs[i, 1] };
                    hiddenNeuron2.inputs = new double[] { inputs[i, 0], inputs[i, 1] };

                    outputNeuron.inputs = new double[] { hiddenNeuron1.output, hiddenNeuron2.output };

                    //Console.WriteLine("{0} xor {1} = {2}", inputs[i, 0], inputs[i, 1], outputNeuron.output);

                    // 2) back propagation (adjusts weights)

                    // adjusts the weight of the output neuron, based on its error
                    outputNeuron.error = Sigmoid.derivative(outputNeuron.output) * (results[i] - outputNeuron.output);
                    outputNeuron.adjustWeights();

                    // then adjusts the hidden neurons' weights, based on their errors
                    hiddenNeuron1.error = Sigmoid.derivative(hiddenNeuron1.output) * outputNeuron.error * outputNeuron.weights[0];
                    hiddenNeuron2.error = Sigmoid.derivative(hiddenNeuron2.output) * outputNeuron.error * outputNeuron.weights[1];

                    hiddenNeuron1.adjustWeights();
                    hiddenNeuron2.adjustWeights();
                }
            }
            
        }
    }
}
