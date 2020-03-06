using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class OutputLayer : Layer
    {
        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        OutputNeuron[] neurons;
        private double[] desiredResults;
        public OutputLayer(int size, Network n) : base(size,n)
        {
            OutputNeuron[] InputNeurons = new OutputNeuron[size];
            for (int i = 0; i < size; i++)
            {
                InputNeurons[i] = new OutputNeuron(this);
                InputNeurons[i].name = "HiddenNeuron " + i;
            }
            this.neurons = InputNeurons;
        }
        public void setDesiredResults(double[] desiredResults)
        {
            this.desiredResults = desiredResults;
        }
        public double calculateErrors()
        {
            double errorsum = 0;
            int i = 0;
            foreach (OutputNeuron n in neurons)
            {
                n.calculateError(desiredResults[i]);
                i++;
                errorsum += Math.Abs(n.error);
            }
            return errorsum / neurons.Length;
        }
        public double[] getAbsoluteErrors()
        {
            double[] errors = new double[neurons.Length];
            int i = 0;
            foreach (OutputNeuron n in neurons)
            {
                errors[i] = Math.Abs( (double) (desiredResults[i] - n.output));
                i++;
            }
            return errors;
        }

        public override Neuron[] getNeurons()
        {
            return (Neuron[])this.neurons;
        }
    }
}
