using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Network
    {
        public InputNeuron[] InputNeurons;
        private Neuron[][] hiddenNeurons;
        public Neuron[] outputNeurons;
        public string type;
        public Network(int InputLayerSize, int[] HiddenLayerSizes, int OutputLayerSize )
        {
            /*****************************
             InputNeurons
             ****************************/
            InputNeurons = new InputNeuron[InputLayerSize];
            for (int i=0; i < InputLayerSize; i++ )
            {
                InputNeurons[i] = new InputNeuron();
                InputNeurons[i].name = "inputNeuron " + i;
            }

            hiddenNeurons = new Neuron[HiddenLayerSizes.Length][];
            for (int i = 0; i < hiddenNeurons.Length; i++) //for each layer
            {
                hiddenNeurons[i] = new Neuron[HiddenLayerSizes[i]];//the size of the Layer is given in the input param
                for (int k = 0; k < hiddenNeurons[i].Length; k++)
                {
                    hiddenNeurons[i][k] = new Neuron();
                    hiddenNeurons[i][k].name = "HiddenNeuron " + k+"|"+i;

                    if (i == 0)
                    {
                        //the inputs come from the inputlayer
                        foreach (InputNeuron inputneuron in InputNeurons)
                        {
                            hiddenNeurons[i][k].addInput(inputneuron);
                        }
                    }
                    else {
                        foreach (Neuron inputneuron in hiddenNeurons[i - 1])
                        {
                            hiddenNeurons[i][k].addInput(inputneuron);
                        }
                    }
                    hiddenNeurons[i][k].randomizeWeights();

                }
            }

            outputNeurons = new Neuron[OutputLayerSize];
            for (int i = 0; i < outputNeurons.Length; i++)
            {
                outputNeurons[i] = new Neuron();
                outputNeurons[i].name = "outputNeuron " + i;
                foreach (Neuron inputneuron in hiddenNeurons[hiddenNeurons.Length-1])
                {
                    outputNeurons[i].addInput(inputneuron);
                }
                outputNeurons[i].randomizeWeights();
            }





        }
        public static Network CreateXOR()
        {
            return new Network(2, new int[1] { 2 }, 1);
        }
        public void trainXOR()
        {
            Console.WriteLine("starting training");
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
                    //set Inputvars
                    InputNeurons[0].output = inputs[i, 0];
                    InputNeurons[1].output = inputs[i, 1];

                    //Console.WriteLine("{0} xor {1} = {2}", inputs[i, 0], inputs[i, 1], outputNeurons[0].output);

                    // 2) back propagation (adjusts weights)

                    // adjusts the weight of the output neuron, based on its error
                    outputNeurons[0].error = Sigmoid.derivative(outputNeurons[0].output) * (results[i] - outputNeurons[0].output);
                    outputNeurons[0].adjustWeights();

                    // then adjusts the hidden neurons' weights, based on their errors
                    hiddenNeurons[0][0].error = Sigmoid.derivative(hiddenNeurons[0][0].output) * outputNeurons[0].error * outputNeurons[0].weights[0];
                    hiddenNeurons[0][1].error = Sigmoid.derivative(hiddenNeurons[0][1].output) * outputNeurons[0].error * outputNeurons[0].weights[1];

                    hiddenNeurons[0][0].adjustWeights();
                    hiddenNeurons[0][1].adjustWeights();


                }
            }
            Console.WriteLine("Training finished");

        }
    }
}
