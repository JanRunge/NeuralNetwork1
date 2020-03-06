using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class InputLayer : Layer
    {
        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        InputNeuron[] neurons;
        public InputLayer(int size,Network n) : base(size, n)
        {
            InputNeuron[] InputNeurons = new InputNeuron[size];
            for (int i = 0; i < size; i++)
            {
                InputNeurons[i] = new InputNeuron(this);
                InputNeurons[i].name = "inputNeuron " + i;
            }
            this.neurons = InputNeurons;
        }
        public override void adjustWeights()
        {
            throw new InvalidOperationException("Inputs cannot change their weight!");
        }
        public override Neuron[] getNeurons()
        {
            return (Neuron[])this.neurons;
        }

    }
}
