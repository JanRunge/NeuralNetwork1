using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    interface Function
    {
        double calculate(double x);
        double derivative(double x);
    }
}
