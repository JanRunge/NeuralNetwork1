using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class leakyrelu : Function
    {
        public double calculate(double x)
        {
            return (0 >= x) ? 0.01d * x : x;
        }

        public double derivative(double x)
        {
            return (0 >= x) ? 0.01d : 1;
        }
    }
}
