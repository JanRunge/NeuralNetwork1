using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NeuralNetwork
{
    class Network
    {
        [JsonPropertyAttribute(DefaultValueHandling=0)]
        protected InputLayer InputLayer;
        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        protected HiddenLayer[] hiddenLayer;
        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        protected OutputLayer outputLayer;

        public string type;
        public int epoch = 0;


        public double learningRate = 0.005f;

        DBHelper db = DBHelper.getInstance();

        [JsonPropertyAttribute(DefaultValueHandling = 0)]
        int DatabaseID;


        public TrainingSet trainingsset;

        
        public Network(int InputLayerSize, int[] HiddenLayerSizes, int OutputLayerSize, Function[] activationFuncs )
        {
            Random r = new Random();
            //will create a network with InputLayerSize inputneurons, HiddenLayerSizes.count Hiddenlayers (Each with the size of the array at that position), and OutputLayerSize outputneurons
            /*****************************
             InputNeurons
             ****************************/
            InputLayer = new InputLayer(InputLayerSize, this);
            /*****************************
             HiddenNeurons
             ****************************/

            hiddenLayer = new HiddenLayer[HiddenLayerSizes.Length];

            for (int i = 0; i < hiddenLayer.Length; i++) //for each layer
            {

                hiddenLayer[i] = new HiddenLayer(HiddenLayerSizes[i], this);
                if (i == 0)
                {
                    hiddenLayer[i].connectToInputLayer(InputLayer);//connect the hiddenlayer to the inputlayer
                }
                else {
                    hiddenLayer[i].connectToInputLayer(hiddenLayer[i-1]);//connect the hiddenlayer to the hiddenlayer before
                }
                hiddenLayer[i].ActivationFunction = activationFuncs[i];
             }

            /*****************************
             OutputNeurons
             ****************************/
            outputLayer = new OutputLayer(OutputLayerSize, this);
            outputLayer.connectToInputLayer(hiddenLayer[HiddenLayerSizes.Length - 1]);
            outputLayer.ActivationFunction = activationFuncs[activationFuncs.Length-1];
            this.RandomizeAllWeights();
            this.DatabaseID=db.getNetworkID();
        }
        public void trainWithLogging(DateTime until)
        {
            validateTraingsset();
            DateTime now = DateTime.Now;
            double avgError = 1;
            double errorSum;
            int epoch = 0;
            while (now < until)
            {
                errorSum = trainOneEpochWithLogging(true);
                OutputTrainingLogmessages(epoch, avgError, errorSum);
                avgError = errorSum ;
                epoch++;
            }
        }
        public void trainWithLogging(int maxEpoch, int saveEveryNEpoch)
        {
            validateTraingsset();
            int epoch = 0;
            double lastError = 1;
            double currentErrAvg = 1;
            while (epoch <= maxEpoch)
            {
                if(epoch>1 && epoch% saveEveryNEpoch == 0)
                {
                    this.toFile("automatedSave.json");
                }
                currentErrAvg = trainOneEpochWithLogging(true);
                OutputTrainingLogmessages(epoch, currentErrAvg, lastError);
                epoch++;
                lastError = currentErrAvg;
            }
            outputTrainingResult(epoch, currentErrAvg);
        }
        public void trainWithLogging(int maxEpoch, double desiredAbsoluteError, int saveEveryNEpoch)
        {
            //stops when one of the following is true:
            //1 the network has been Training for maxEpoch epochs
            //2 average (sum(Absolute errors of all Neurons)) across all trainingsCases is below  desiredAbsoluteError
            validateTraingsset();
            int epoch = 0;
            double lastError = 1;
            double currentErrAvg = 1;
            while (epoch <= maxEpoch && currentErrAvg> desiredAbsoluteError)
            {
                if (epoch > 1 && epoch % saveEveryNEpoch == 0)
                {
                    this.toFile("automatedSave.json");
                }
                currentErrAvg = trainOneEpochWithLogging(true);
                OutputTrainingLogmessages(epoch, currentErrAvg, lastError);
                epoch++;
                lastError = currentErrAvg;
            }
            outputTrainingResult(epoch, currentErrAvg);
        }
        
        public void test()//output the actual vs desired results fopr every trainingcase
        {
            for (int i = 0; i < this.trainingsset.inputs.Length; i++)  //Loop over every input dataset
            {
                double[] erg = this.calculateForInput(this.trainingsset.inputs[i]);
                Console.WriteLine("------ ");
                Console.WriteLine(trainingsset.results.ToString() + " ");
                Console.WriteLine(erg.ToString() + " ");
                Console.WriteLine("------ ");
            }
        }


        public void RandomizeAllWeights()//randomize weights
        {
            InputLayer.randomizeAll();
            foreach(Layer h in hiddenLayer)
            {
                h.randomizeAll();
            }
            outputLayer.randomizeAll();
        }

        public double[] calculateForInput(double[] inputs)
        {
            
            this.setInputs(inputs);
            this.fire();
            return getResult();
            
        }
        protected double[] getResult()
        {
            return outputLayer.getOutputs();
        }
        protected virtual void validateTraingsset()
        {
            if (this.trainingsset == null)
            {
                throw new Exception("No Traingsset Provided");
            }
            if (this.trainingsset.inputs.Length != this.trainingsset.results.Length)
            {
                throw new Exception("Length of inputs and results in trainingsset dont match");
            }
        }
        protected void fire()
        {
            InputLayer.fire();
            
            foreach (HiddenLayer Layer in this.hiddenLayer)
            {
                Layer.fire();
                
            }
            outputLayer.fire();
        }
        protected void setInputs(double[] inputs)
        {
            int i = 0;
            foreach (InputNeuron n in this.InputLayer.getNeurons())
            {
                n.input = inputs[i];
                i++;
            }
        }
        protected double calculateErrors(double[] desiredResult)
        {
            outputLayer.setDesiredResults(desiredResult);
            double errorsum= outputLayer.calculateErrors();
            for (int l = hiddenLayer.Count(); l > 0; l--)
            {
                hiddenLayer[l - 1].calculateErrors();
            }
            return errorsum;
        }
        

        protected void asjustWeights()
        {
            foreach (HiddenLayer Layer in hiddenLayer)
            {
                Layer.adjustWeights();
            }
            outputLayer.adjustWeights();
        }

        private double trainOneEpochWithLogging(bool WriteToDataBase)
        {
            this.epoch++;
            double[] sumOfAbsoluteErrors = new double[outputLayer.getNeurons().Count()];
            double[] sumOfErrors = new double[outputLayer.getNeurons().Count()];
            for (int i = 0; i < outputLayer.getNeurons().Count(); i++)
            {
                sumOfAbsoluteErrors[i] = 0f;

            }
            for (int i = 0; i < this.trainingsset.inputs.Length; i++)  //Loop over every input dataset
            {
                setInputs(this.trainingsset.inputs[i]);
                this.fire();
                calculateErrors(this.trainingsset.results[i]);
                this.asjustWeights();

                double[] betw = outputLayer.getAbsoluteErrors();

                sumOfAbsoluteErrors = sumOfAbsoluteErrors.Zip(betw, (x, y) => x + y).ToArray<double>();
            }
            sumOfAbsoluteErrors = sumOfAbsoluteErrors.Select(d => d / this.trainingsset.inputs.Length).ToArray<double>();// divide by the amount of trainings-inmputs, so we get the average error for every outputneuron
            if (WriteToDataBase)
            {
                DBHelper.HandleInsert(DatabaseID, sumOfAbsoluteErrors, epoch);
            }
            print(sumOfAbsoluteErrors);

            return sumOfAbsoluteErrors.Sum();
        }



        private void outputTrainingResult(int epoch, double avgError)
        {
            Console.WriteLine("Epoch " + epoch + " Training ended. Error= " + avgError);
        }
        private void OutputTrainingLogmessages(int epoch, double currentError, double LastError)
        {
            if (epoch % 250 == 0)
            {
                Console.WriteLine("Epoch " + epoch + "current error=" + currentError);
            }
            if (currentError > LastError)
            {
                Console.WriteLine("Epoch " + epoch + "current error=" + currentError + " the error is ingreasing.");
            }
        }
        /*
         functions for calculation of sums, averages of output sets. 
             */
        void print(double[] thing)
        {
            for(int i=0; i< thing.Length; i++)
            {
                Console.Write(thing[i]+" | ");
            }
            Console.Write("\n");
        }

        public void toFile(String path)
        {
            Console.WriteLine("saving network to "+path);
            if (File.Exists(path))
            {
                path = path + DateTime.UtcNow.ToString("MM_dd_yyyy_HH_mm_ss");
            }
            string json1 = JsonConvert.SerializeObject(this, Formatting.Indented,new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All });
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(json1.ToString());
                tw.Close();
            }
            Console.WriteLine("done Saving Network");
        }

        public static Network getFromFile(String path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);
            Network results = JsonConvert.DeserializeObject<Network>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, TypeNameHandling = TypeNameHandling.All, ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor });
            return results;
        }

    }
}
