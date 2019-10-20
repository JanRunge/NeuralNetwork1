using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class XoRNetwork : Network
    {
        public XoRNetwork() : base(2, new int[1] { 2 }, 1)// create a network of the size 1-2-1
        {
            Console.WriteLine("creating xor");
            this.TrainingSet = new TrainingSet();
            this.TrainingSet.inputs= new double[4][]{
                 new double[2]{ 0, 0},
                 new double[2]{ 0, 1},
                 new double[2]{ 1, 0},
                 new double[2]{ 1, 1}
             };
            this.TrainingSet.results = new double[4][]{
                 new double[1]{ 0},
                 new double[1]{ 1},
                 new double[1]{ 1},
                 new double[1]{ 0}
             };
        }

    }
}
