using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        double[] results = { 0, 1, 1, 1 };


        public Network(int InputLayerSize, int[] HiddenLayerSizes, int OutputLayerSize )
        {
            startweights = new double[HiddenLayerSizes.Length+1][];
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
            }
            this.RandomizeAllWeights();
        }
        public static Network CreateXOR()
        {
            return new Network(2, new int[1] { 2 }, 1);
        }
        public void RandomizeAllWeights()//randomize weights
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
        public void Save(String path)
        {
            XElement xDocumentHead = new XElement("Network");

            XElement xInputLayer = new XElement("Input");
            XElement xHiddenLayers = new XElement("Hidden");
            XElement xOuputLayer = new XElement("Output");
            XElement xAxons = new XElement("Axons");
            xDocumentHead.Add(xInputLayer);
            xDocumentHead.Add(xHiddenLayers);
            xDocumentHead.Add(xOuputLayer);
            xDocumentHead.Add(xAxons);

            foreach (Neuron n in InputNeurons)
            {
                XAttribute xType = new XAttribute("Type", "input");
                XAttribute xName = new XAttribute("Name", n.name);
                XElement xn = new XElement("Neuron", xType, xName);
                xInputLayer.Add(xn);
            }
            foreach (Neuron[] Layer in hiddenNeurons)
            {
                XElement xHiddenLayer = new XElement("Layer");
                xHiddenLayers.Add(xHiddenLayer);
                foreach (Neuron n in Layer)
                {
                    XAttribute xType = new XAttribute("Type", "hidden");
                    XAttribute xName = new XAttribute("Name", n.name);
                    XElement xn = new XElement("Neuron", xType, xName);


                    xHiddenLayer.Add(xn);
                }
            }

            foreach (Neuron n in outputNeurons)
            {
                XAttribute xType = new XAttribute("Type", "output");
                XAttribute xName = new XAttribute("Name", n.name);
                XElement xn = new XElement("Neuron", xType, xName);
                xOuputLayer.Add(xn);
            }
            xDocumentHead.Save("Root.xml");



        }
        public void fire()
        {
            foreach (Neuron[] Layer in this.hiddenNeurons)
            {
                foreach (Neuron n in Layer)
                {
                    n.fire();
                }
            }
            foreach (Neuron n in this.outputNeurons)
            {
                n.fire();
            }
        }
        public void train(double faultTolerance, int maxEpochs)
        {
            int epoch = 0;
            double highestError = 1;//the current maximum error
            double ErrorSumPerSet=0;
            double lastErrorSumPerSet = 0;
            bool abortflag = false;
            while (highestError > faultTolerance && epoch <= maxEpochs && !abortflag)
            {
                epoch++;
                
                if(lastErrorSumPerSet< ErrorSumPerSet)
                {
                    Console.WriteLine("The error is increasing");
                    abortflag = true;
                }
                for (int i = 0; i < inputs.Length; i++)  //Loop over every input dataset
                {
                    highestError = 0;
                    lastErrorSumPerSet = ErrorSumPerSet;
                    ErrorSumPerSet = 0;
                    for (int k = 0; k < this.InputNeurons.Length; k++)  // fill the input-neurons
                    {
                        this.InputNeurons[k].output = inputs[i][k];
                    }
                    this.fire();
                    foreach (OutputNeuron n in outputNeurons)
                    {
                        n.calculateError(results[i]);
                        ErrorSumPerSet += Math.Abs(n.error);
                        if (Math.Abs(n.error)> highestError)
                        {   
                            
                            
                            if (Math.Abs(n.error) - highestError < (faultTolerance / 10) && Math.Abs(n.error) >faultTolerance)
                            {
                                Console.WriteLine("The Training has reached a minimum (correcting the error by "+( Math.Abs(n.error) - highestError) + "), but is still " + highestError + " away from the correct Result. This might indicate that the Network ran into a local minimum");
                                abortflag = true;
                            }
                            highestError = Math.Abs(n.error);
                            
                        }
                    }
                    for(int l=0; l < hiddenNeurons.Count(); l++)
                    {
                        Neuron[] layer = hiddenNeurons[hiddenNeurons.Count()-1 - l];
                        foreach (Neuron n in layer)
                        {
                            n.calculateError(results[i]);
                        }
                    }


                    //adjust weights now

                    foreach (Neuron[] Layer in hiddenNeurons)
                    {
                        foreach (Neuron n in Layer)
                        {
                            n.adjustWeights();
                        }
                    }


                    foreach (Neuron n in outputNeurons)
                    {
                        n.adjustWeights();
                    }

                }
            }
            if (abortflag)
            {
                Console.WriteLine("Training was aborted. Current Error: " + highestError);
            }else if (epoch >= maxEpochs)
            {
                Console.WriteLine("Training finished all Epochs ("+epoch+"). Remaining Error:" + highestError);
            }else
            {
                Console.WriteLine("Successfull training after "+epoch+" epochs: "+ highestError);
            }
           

        }
        
    }
}
