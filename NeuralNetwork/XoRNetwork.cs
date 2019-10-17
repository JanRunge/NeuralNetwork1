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
            
        }

    }
}
