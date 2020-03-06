using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Sigmoid : Function
    {
        public double calculate(double x)
        {
            double k = (double)Math.Exp(x);
            return k / (1.0d + k);
        }

        public double derivative(double x)
        {
            return x * (1 - x);
        }
    }
}
