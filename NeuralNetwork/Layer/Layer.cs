using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    abstract class Layer
    {
        public Function ActivationFunction = new Sigmoid();

        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        protected Network myNetwork;

        public Layer(int size, Network n)
        {
            myNetwork = n;
        }

        public double getLearningRate()
        {
            return this.myNetwork.learningRate;
        }

        public abstract Neuron[] getNeurons();

        public double[] getOutputs()
        {
            int i = 0;
            double[] erg = new double[getNeurons().Length];
            foreach (Neuron n in getNeurons())
            {
                erg[i] = n.output;
                i++;
            }
            return erg;
        }

        public void fire()
        {
            foreach (Neuron n in getNeurons())
            {
                n.fire();
            }
        }
        
        
        public void connectToInputLayer(Layer l)
        {
            //connect every neuron in layer1 to every neuron in layer 2
            foreach (Neuron myNeuron in getNeurons())
            {
                foreach (Neuron inputneuron in l.getNeurons())
                {
                    myNeuron.addInput(inputneuron);
                }
            }
            
        }
        public void randomizeAll()
        {
            foreach (Neuron n in getNeurons())
            {
                n.randomizeWeights();
            }
        }
       
        public virtual void adjustWeights()
        {
            foreach (Neuron n in getNeurons())
            {
                n.adjustWeights();
            }
        }

       

    }
}
