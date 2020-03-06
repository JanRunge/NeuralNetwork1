using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class tanh : Function
    {
        public double calculate(double x)
        {
            return (double)Math.Tanh(x);
        }

        public double derivative(double x)
        {
            return 1 - (x * x);
        }
    }
}
