using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class relu : Function
    {
        public double calculate(double x)
        {
            return (0 >= x) ? 0 : x;
        }

        public double derivative(double x)
        {
            return (0 >= x) ? 0 : 1;
        }
    }
}
