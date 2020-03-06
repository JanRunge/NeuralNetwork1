using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace NeuralNetwork
{
    class DBHelper
    {
        static readonly object _object = new object();
        private static DBHelper _instance= new DBHelper();

        MySqlConnection connection;
        
        private DBHelper()
        {
            string myConnectionString = "SERVER=localhost;" +
                            "DATABASE=neuralttt;" +
                            "UID=root;" +
                            "PASSWORD=;";

            connection = new MySqlConnection(myConnectionString);
        }
        public static DBHelper getInstance()
        {
            return _instance;
        }
        public static void HandleInsert(int NetworkID1, double[] neurons1, int epoch1)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(InsertStatic), new object[] { NetworkID1, neurons1, epoch1 });
        }
        private static void InsertStatic(object parameters)
        {
            object[] param = (object[])parameters;

            int NetworkID1 = (int)param[0];
            double[] neurons1 = (double[])param[1];
            int epoch1 = (int)param[2];
            _instance._Insert(NetworkID1,neurons1,epoch1);
        }
        public int getNetworkID() {
            int NetworkID1;
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "Insert into network (networktime) values (CURRENT_TIMESTAMP);"
                + "select last_insert_id();";
            NetworkID1 = Convert.ToInt32(command1.ExecuteScalar());
            Console.WriteLine("Network ID =" + NetworkID1);
            connection.Close();
            return (int)NetworkID1;
        }
        
        private void _Insert(int NetworkID1, double[] neurons1, int epoch1)
        {
            int EpochID;
            lock (_object)
            {
                connection.Open();
                MySqlCommand command2 = connection.CreateCommand();
                command2.CommandText = "Insert into epoch (network, epochtimeStamp, epochnum) values (" + NetworkID1 + ",CURTIME(3)," + epoch1 + " );"
                    + "select last_insert_id();";
                EpochID = Convert.ToInt32(command2.ExecuteScalar());

                MySqlCommand command = connection.CreateCommand();
                String text = "Insert into neuron (idneuron, epochID ,neuronval ) values";
                text += "(" + 0 + "," + EpochID + ", @i" + 0 + "   )";
                for (int i = 1; i < neurons1.Length; i++)
                {
                    text += ",(" + i + "," + EpochID + ", @i" + i + "   )";
                }
                text += ";";
                for (int i = 0; i < neurons1.Length; i++)
                {
                    command.Parameters.AddWithValue("@i" + i, neurons1[i]);
                }
                command.CommandText = text;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
