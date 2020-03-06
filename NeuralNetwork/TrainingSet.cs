using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class TrainingSet
    {
        public double[][] inputs;
        /*
         {
            {1,1,0,0,1}, --each number will be mapped unto an inputneuron
            {1,1,0,0,1}, --each array of inputs corresponds to a resultset
            {1,1,0,0,1},
            
             */

        public double[][] results;

        public void toFile(String path)
        {
            if (File.Exists(path))
            {
                path = path + DateTime.UtcNow.ToString("MM_dd_yyyy_HH_mm_ss");

            }
            var json1 = JsonConvert.SerializeObject(this);
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(json1.ToString());
                tw.Close();
            }
        }

        public static TrainingSet getFromFile(String path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("trainingsset could not be loaded from " + path + " : no file found");
                return null;
            }
                string json = File.ReadAllText(path, Encoding.UTF8);
            TrainingSet results = JsonConvert.DeserializeObject<TrainingSet>(json);
            
            return results;
        }
    }
}
