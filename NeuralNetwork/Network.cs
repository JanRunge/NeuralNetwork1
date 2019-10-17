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
        private HiddenNeuron[][] hiddenNeurons;
        public OutputNeuron[] outputNeurons;
        public string type;

        double[][] inputs =
             new double[4][]{
                 new double[2]{ 0, 0},
                 new double[2]{ 0, 1},
                 new double[2]{ 1, 0},
                 new double[2]{ 1, 1}
             };

        // desired results
        double[] results = { 0, 1, 1, 0 };


        public Network(int InputLayerSize, int[] HiddenLayerSizes, int OutputLayerSize )
        {
            Random r = new Random();
            //will create a network with InputLayerSize inputneurons, HiddenLayerSizes.count Hiddenlayers (Each with the size of the array at that position), and OutputLayerSize outputneurons
            /*****************************
             InputNeurons
             ****************************/
            InputNeurons = new InputNeuron[InputLayerSize];
            for (int i=0; i < InputLayerSize; i++ )
            {
                InputNeurons[i] = new InputNeuron(r);
                InputNeurons[i].name = "inputNeuron " + i;
            }
            /*****************************
             HiddenNeurons
             ****************************/
            hiddenNeurons = new HiddenNeuron[HiddenLayerSizes.Length][];
            for (int i = 0; i < hiddenNeurons.Length; i++) //for each layer
            {
                hiddenNeurons[i] = new HiddenNeuron[HiddenLayerSizes[i]];//the size of the Layer is given in the input param
                for (int k = 0; k < hiddenNeurons[i].Length; k++)
                {
                    hiddenNeurons[i][k] = new HiddenNeuron(r);
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
            /*****************************
             OutputNeurons
             ****************************/
            outputNeurons = new OutputNeuron[OutputLayerSize];
            for (int i = 0; i < outputNeurons.Length; i++)
            {
                outputNeurons[i] = new OutputNeuron(r);
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
        public void reInitialize()//randomize weights
        {

            foreach (Neuron n in outputNeurons)
            {
                n.randomizeWeights();
            }
            foreach (Neuron[] Layer in hiddenNeurons)
            {
                foreach (Neuron n in Layer)
                {
                    n.randomizeWeights();
                }
            }

        }


        public void train(double faultTolerance, int maxEpochs)
        {
            int epoch = 0;
            double error=1;//the current maximum error
            bool abortflag=false;
            while (error > faultTolerance && epoch <= maxEpochs && !abortflag)
            {
                epoch++;
                error = 0;
                for (int i = 0; i < inputs.Length ; i++)  //Loop over every input dataset
                {

                    for (int k = 0; k < this.InputNeurons.Length; k++)  // fill the input-neurons
                    {
                        this.InputNeurons[k].output = inputs[i][ k]; 
                    }
                    foreach (Neuron n in outputNeurons) {
                        n.calculateError(results[i]);
                        n.adjustWeights();
                        if (Math.Abs(n.error)> error)
                        {
                            if (Math.Abs(n.error) - error <0.00001)
                            {
                                Console.WriteLine("The Training has reached a minimum (correcting the error by "+( Math.Abs(n.error) - error) + "), but is still " + error + " away from the correct Result. This might indicate, that the Network ran into a local minimum");
                                abortflag = true;
                            }
                            error = Math.Abs(n.error);
                            
                        }
                    }
                    foreach (Neuron[] Layer in hiddenNeurons)
                    {
                        foreach (Neuron n in Layer)
                        {
                            n.calculateError(results[i]);
                            n.adjustWeights();
                        }
                    }

                }
            }
            if (epoch >= maxEpochs || abortflag)
            {
                Console.WriteLine("Unsuccessfull training: " + error); //the network might have run into a local minimum
            }
            else
            {
                Console.WriteLine("Successfull training: "+ error);
            }
           

        }
    }
}
